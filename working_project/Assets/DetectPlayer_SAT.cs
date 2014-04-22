using UnityEngine;
using System.Collections;

struct ProjPoints
{
    public float max;
    public float min;
};
public class DetectPlayer_SAT : MonoBehaviour 
{
    const int VERTS_PER_SHAPE = 4;
    const int SQ_TILE = 0;
    const int SQ_PLAYER = 1;
    ProjPoints[] projPoints = new ProjPoints[VERTS_PER_SHAPE];
    public bool isNotColliding = false;

    MeshFilter filter;
    Vector2 normal;

    Vector3 meshVerts;
    Vector2[] endTileVertPositions = new Vector2[VERTS_PER_SHAPE];
    Vector2[] tileVertNormals = new Vector2[2];

    GameObject player;
    float colliderMagnitude;
    float[] vertAngles = new float[VERTS_PER_SHAPE];
    float eulerRotation;
    Vector2[] playerColliderVertPositions = new Vector2[VERTS_PER_SHAPE];
    Vector2[] playerColliderNormals = new Vector2[2];
    Vector3 centre;
    Vector3 extents;

	void Start ()
    {
        Debug.Log(transform.position);

        //////////////////////////////////////////////  get end tile verts

        filter = GetComponent("MeshFilter") as MeshFilter;

        // Since traingles are used, these positions are not in order around the square one to the next.
        // This is not looped to manually put vertices in order before calculating normals
        meshVerts = transform.TransformPoint(filter.mesh.vertices[0]);
        endTileVertPositions[0] = new Vector2(meshVerts.x, meshVerts.z);
        meshVerts = transform.TransformPoint(filter.mesh.vertices[2]);
        endTileVertPositions[1] = new Vector2(meshVerts.x, meshVerts.z);
        meshVerts = transform.TransformPoint(filter.mesh.vertices[1]);
        endTileVertPositions[2] = new Vector2(meshVerts.x, meshVerts.z);
        meshVerts = transform.TransformPoint(filter.mesh.vertices[3]);
        endTileVertPositions[3] = new Vector2(meshVerts.x, meshVerts.z);
        Debug.Log(endTileVertPositions[0] + ", " + endTileVertPositions[1] + ", " + endTileVertPositions[2] + ", " + endTileVertPositions[3]);
        // Calcutae normals (only 2 needed for squares, and without needed to use negative reciprocals)
        for (int i = 0; i < tileVertNormals.Length; i++)
        {
            tileVertNormals[i] = endTileVertPositions[i] - endTileVertPositions[i+1];
            tileVertNormals[i].Normalize();
        }
        Debug.Log(tileVertNormals[0] + ", " + tileVertNormals[1]);

        //////////////////////////////////////////////  Find collision box verts

        player = GameObject.Find("player(Clone)");
        // verts aren't available for collision boxes. have to calculate using centre position & half-widths(extents)
        centre = player.collider.bounds.center;
        extents = player.collider.bounds.extents;

        // start calculating angle to each vertex/corner. Will be needed tp add to player's rotation
        Vector2 temp = new Vector2(extents.x, extents.z);
        colliderMagnitude = temp.magnitude;

        vertAngles[0] = Mathf.Atan2(extents.z, extents.x) * (180/Mathf.PI);
        vertAngles[1] = Mathf.Atan2(extents.z, -extents.x) * (180 / Mathf.PI);
        vertAngles[2] = Mathf.Atan2(-extents.z, -extents.x) * (180 / Mathf.PI);
        vertAngles[3] = Mathf.Atan2(-extents.z, extents.x) * (180 / Mathf.PI);

        // just compensating for angles > 180 and < 360
        for (int i = 0; i < vertAngles.Length; i++)
            vertAngles[i] = (vertAngles[i] > 0) ? vertAngles[i] : vertAngles[i] + 360;
        //Debug.Log(vertAngles[0] + ", " + vertAngles[1] + ", " + vertAngles[2] + ", " + vertAngles[3]);

        for (int i = 0; i < playerColliderVertPositions.Length; i++)
            playerColliderVertPositions[i] = new Vector2();
	}
	
	void Update ()
    {
        // To see tile vertices & normals
        for (int i = 0; i < endTileVertPositions.Length; i++)
            Debug.DrawRay(new Vector3(endTileVertPositions[i].x, -2.5f, endTileVertPositions[i].y), Vector3.up, Color.red, 0.01f);
        for (int i = 0; i < tileVertNormals.Length; i++)
            Debug.DrawRay(transform.position, new Vector3(tileVertNormals[i].x, 0, tileVertNormals[i].y), Color.green, 0.01f);

        // need to constantly update as player moves
        centre = player.collider.bounds.center;
        eulerRotation = player.transform.eulerAngles.y;
        // Debug.Log((eulerRotation + vertAngles[0]) + ", " + (eulerRotation + vertAngles[1]) + ", " + (eulerRotation + vertAngles[2]) + ", " + (eulerRotation + vertAngles[3]));
        // getting the positions of the box collider vertices, accounting for player rotation
        // don't forget to convert back to radians! - Could probably do it all in radians from the start, but I needed degrees to know I had things working
        for (int i = 0; i < playerColliderVertPositions.Length; i++)
        {
            playerColliderVertPositions[i].Set(centre.x + (colliderMagnitude * Mathf.Sin((eulerRotation + vertAngles[i]) * (Mathf.PI / 180))), centre.z + (colliderMagnitude * Mathf.Cos((eulerRotation + vertAngles[i]) * (Mathf.PI / 180))));
            Debug.DrawRay(new Vector3(playerColliderVertPositions[i].x, -2.5f, playerColliderVertPositions[i].y), Vector3.up, Color.red, 0.01f);
        }
        //Debug.Log(playerColliderVertPositions[0] + ", " + playerColliderVertPositions[1] + ", " + playerColliderVertPositions[2] + ", " + playerColliderVertPositions[3]);

        for (int i = 0; i < playerColliderNormals.Length; i++)
        {
            playerColliderNormals[i] = playerColliderVertPositions[i] - playerColliderVertPositions[i + 1];
            playerColliderNormals[i].Normalize();
            Debug.DrawRay(centre, new Vector3(playerColliderNormals[i].x, 0, playerColliderNormals[i].y), Color.green, 0.01f);
        }
        //Debug.Log(playerColliderNormals[0] + ", " + playerColliderNormals[1]);

        // Data is collected, now do Separating axis theorem
        // Since the tile and player have two normals each, use either of their lengths to loop, but use both sets inside
        isNotColliding = false;
        for (int i = 0; i < tileVertNormals.Length; i++)
        {
            for (int j = 0; j < VERTS_PER_SHAPE; j++)
            {
                float projPoint = Vector3.Dot(endTileVertPositions[j], tileVertNormals[i]);
                // Need to capture some initial value
                if (j == 0)
                {
                    projPoints[SQ_TILE].min = projPoint;
                    projPoints[SQ_TILE].max = projPoint;
                }
                projPoints[SQ_TILE].min = (projPoints[SQ_TILE].min < projPoint) ? projPoints[SQ_TILE].min : projPoint;
                projPoints[SQ_TILE].max = (projPoints[SQ_TILE].max > projPoint) ? projPoints[SQ_TILE].max : projPoint;

                projPoint = Vector3.Dot(playerColliderVertPositions[j], tileVertNormals[i]);
                if (j == 0)
                {
                    projPoints[SQ_PLAYER].min = projPoint;
                    projPoints[SQ_PLAYER].max = projPoint;
                }
                projPoints[SQ_PLAYER].min = (projPoints[SQ_PLAYER].min < projPoint) ? projPoints[SQ_PLAYER].min : projPoint;
                projPoints[SQ_PLAYER].max = (projPoints[SQ_PLAYER].max > projPoint) ? projPoints[SQ_PLAYER].max : projPoint;
            }
            if ((projPoints[SQ_TILE].max < projPoints[SQ_PLAYER].min || projPoints[SQ_PLAYER].max < projPoints[SQ_TILE].min))
            {
                isNotColliding = true;
                break;
            }

            for (int j = 0; j < VERTS_PER_SHAPE; j++)
            {
                float projPoint = Vector3.Dot(endTileVertPositions[j], playerColliderNormals[i]);
                if (j == 0)
                {
                    projPoints[SQ_TILE].min = projPoint;
                    projPoints[SQ_TILE].max = projPoint;
                }
                projPoints[SQ_TILE].min = (projPoints[SQ_TILE].min < projPoint) ? projPoints[SQ_TILE].min : projPoint;
                projPoints[SQ_TILE].max = (projPoints[SQ_TILE].max > projPoint) ? projPoints[SQ_TILE].max : projPoint;

                projPoint = Vector3.Dot(playerColliderVertPositions[j], playerColliderNormals[i]);
                if (j == 0)
                {
                    projPoints[SQ_PLAYER].min = projPoint;
                    projPoints[SQ_PLAYER].max = projPoint;
                }
                projPoints[SQ_PLAYER].min = (projPoints[SQ_PLAYER].min < projPoint) ? projPoints[SQ_PLAYER].min : projPoint;
                projPoints[SQ_PLAYER].max = (projPoints[SQ_PLAYER].max > projPoint) ? projPoints[SQ_PLAYER].max : projPoint;
            }
            if ((projPoints[SQ_TILE].max < projPoints[SQ_PLAYER].min || projPoints[SQ_PLAYER].max < projPoints[SQ_TILE].min))
            {
                isNotColliding = true;
                break;
            }
        }

        /*if (isNotColliding)
            Debug.Log("Player is not at the exit");
        else
            Debug.Log("Player is at the exit");*/
	}
}
