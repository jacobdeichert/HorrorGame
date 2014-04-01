using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private GUITexture guiHealthBarFill;
    private GUIText infoText;
    private float health;
    private bool isAlive;
    private bool loweringHealth = false;
    private Camera camera;

	void Start () 
	{
        health = 100f;
        camera = GetComponentInChildren<Camera>();

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
	

	void Update () {
        updateTorch();
	}

    void updateTorch() {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray, out hit, 3.0f) && hit.collider.name == "Torch") {
            infoText.text = "[V]  Light Torch";
            infoText.enabled = true;

            Torch t;
            if ((t = hit.collider.gameObject.GetComponent<Torch>() as Torch) != null
                && Input.GetKeyDown(KeyCode.V)) {
                t.ToggleFlame();
            }
        } else {
            infoText.enabled = false;
        }
    }


    void OnCollisionEnter(Collision c) {
        // touching trap
        TrapDamage t;
        if ((t = c.collider.GetComponent<TrapDamage>() as TrapDamage) != null) {
            if (!loweringHealth) {
                loweringHealth = true;
                StartCoroutine(decreaseHealthPerSecondRepeat(t.damagePerSecond, 1f));
            }
        }
        
        // touching monster
        //else if ((Monster)(c.collider.GetComponent<Monster>())) {

        //}
    }



    void OnCollisionExit(Collision c) {
        if ((TrapDamage)(c.collider.GetComponent<TrapDamage>())) {
            loweringHealth = false;
        }
    }

    IEnumerator decreaseHealthPerSecondRepeat(float damageAmount, float repeatRate) {
        while(loweringHealth) {
            decreaseHealth(damageAmount);
            yield return new WaitForSeconds(repeatRate);
        }
    }

    void decreaseHealth(float damageAmount) {
        health -= damageAmount;
        // calc new x position of health bar fill texture
        float newX = -(guiHealthBarFill.pixelInset.width - guiHealthBarFill.pixelInset.width * (health / 100));
        guiHealthBarFill.pixelInset = new Rect(newX, guiHealthBarFill.pixelInset.y, guiHealthBarFill.pixelInset.width, guiHealthBarFill.pixelInset.height);
    }

}

