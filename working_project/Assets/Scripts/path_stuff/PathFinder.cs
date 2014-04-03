using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour
{
	public List<Node> nodes = new List<Node>();
	public List<Node> path = new List<Node>();
	Node startNode;
	Node endNode;

	void Start () 
	{
		foreach (Node node in GameObject.FindObjectsOfType<Node>())
			nodes.Add(node);
	}

	public void ResetPath(Vector3 sourcePos)
	{
		Vector2 startPos = new Vector2(transform.position.x, transform.position.z);
		Vector2 endPos = new Vector2(sourcePos.x, sourcePos.z);
		float _nodeSize = gameObject.GetComponent<PathGen>().nodeSize;
		foreach (Node node in nodes)
		{
			node.path = false;
			node.parent = null;

			if (node.transform.position.x - _nodeSize / 2 < startPos.x && node.transform.position.x + _nodeSize / 2 > startPos.x &&
			    node.transform.position.z - _nodeSize / 2 < startPos.y && node.transform.position.z + _nodeSize / 2 > startPos.y){
				startNode = node;
			}
			if (node.transform.position.x - _nodeSize / 2 < endPos.x && node.transform.position.x + _nodeSize / 2 > endPos.x &&
			    node.transform.position.z - _nodeSize / 2 < endPos.y && node.transform.position.z + _nodeSize / 2 > endPos.y){
				endNode = node;
			}
		}
		if (startNode && endNode){
			path = gameObject.GetComponent<PathGen>().CalculatePath(startNode, endNode, nodes);
		}
		if (path.Count > 0){
			/*foreach (Node node in path){
				node.path = true; //don't think we need this anymore
			}*/
		}
        else
            Debug.Log("No path.");
	}
}
