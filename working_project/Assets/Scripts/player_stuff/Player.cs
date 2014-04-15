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

	void Start () {
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


    public void OnCollisionEnter(Collision c)
    {
        if (c.collider.name == "OuterWall(Clone)")
        {
            reachedEnd = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            rigidbody.freezeRotation = true;
        }
    }


	void Update () {
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

