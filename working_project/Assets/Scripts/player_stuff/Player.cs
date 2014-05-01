using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private GUITexture guiHealthBarFill;
    private GUIText infoText;
    private float health;
    private bool isAlive;
    private Camera camera;
    private Light light;
    private bool reachedEnd;
    private DetectPlayer_SAT satEndTile;
    private GameObject endWhiteCube;
    private bool foundEndTile = false;

	void Start () {
        Screen.showCursor = false;
        Screen.lockCursor = true;
        health = 100f;
        isAlive = true;
        reachedEnd = false;
        camera = GetComponentInChildren<Camera>();
        light = GetComponentInChildren<Light>();

        // find the gui health bar
        GUITexture[] textures = GameObject.FindObjectsOfType(typeof(GUITexture)) as GUITexture[];
        foreach (GUITexture t in textures) {
            if (t.name == "health_bar_fill_gui") {
                guiHealthBarFill = t;
                break;
            }
        }

        // find gui info text
        GUIText[] texts = GameObject.FindObjectsOfType(typeof(GUIText)) as GUIText[];
        foreach (GUIText t in texts) {
            if (t.name == "info_text_gui") {
                infoText = t;
                break;
            }
        }
        infoText.enabled = false;
	}

	public bool IsAlive(){
		return isAlive;
	}


    public void OnCollisionEnter(Collision c)
    {
        // if the player touches the white wall or the end tile detects the player with SAT
        if (c.collider.name == "OuterWall(Clone)" || (foundEndTile && !satEndTile.isNotColliding))
        {
            if (!reachedEnd) {
                endWhiteCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                endWhiteCube.transform.position = transform.position + new Vector3(0, 0, 2.1f);
                endWhiteCube.transform.rotation = new Quaternion(0, -45f, 0, 1);
                endWhiteCube.transform.localScale = new Vector3(8f, 10f, 3f);
                endWhiteCube.transform.parent = transform;
                endWhiteCube.collider.enabled = false;
                reachedEnd = true;
            }
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.freezeRotation = true;
        }
    }



	void Update () {
        if (Input.GetButton("Back")) {
            Application.LoadLevel("MainMenu");
        }


        if (!foundEndTile && FindObjectOfType<DetectPlayer_SAT>() != null)
        {
            foundEndTile = true;
            satEndTile = FindObjectOfType<DetectPlayer_SAT>();
        }


        updateTorch();

        if (isAlive && health <= 0) {
            isAlive = false;
            transform.Rotate(new Vector3(90, 0, 0));
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (!isAlive) {
            StartCoroutine(decreaseLightIntensity(0.5f));
        }

        if (reachedEnd) {
            light.spotAngle += 0.5f;
            light.intensity += 0.1f;
        }
	}



    void updateTorch() {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray, out hit, 3.0f) && hit.collider.name == "Torch") {
            infoText.text = "[LEFT CLICK]  Light Torch";
            infoText.enabled = true;

            Torch t;
            if ((t = hit.collider.gameObject.GetComponent<Torch>() as Torch) != null
                && Input.GetButtonDown("Action")) {
                t.ToggleFlame();
            }
        } else {
            infoText.enabled = false;
        }
    }



    public void decreaseHealth(float damageAmount) {
        if (health > 0) {
            health -= damageAmount;
            // calc new x position of health bar fill texture
            float newX = -(guiHealthBarFill.pixelInset.width - guiHealthBarFill.pixelInset.width * (health / 100));
            guiHealthBarFill.pixelInset = new Rect(newX, guiHealthBarFill.pixelInset.y, guiHealthBarFill.pixelInset.width, guiHealthBarFill.pixelInset.height);
        } 
    }



    public IEnumerator decreaseLightIntensity(float repeatRate) {
        light.intensity -= 0.01f;
        yield return new WaitForSeconds(repeatRate);
    }

}

