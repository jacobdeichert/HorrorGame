using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GUITexture guiHealthBarFill;
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
        foreach (GUITexture t in textures)
		{
            if (t.name == "health_bar_fill_gui")
			{
                guiHealthBarFill = t;
                break;
            }
        }
	}
	

	void Update () {
        updateTorch();
	}

    void updateTorch() {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward * 2);

        Debug.DrawRay(camera.transform.position, camera.transform.forward * 2);

        if (Physics.Raycast(ray, out hit) && hit.collider.name == "Torch")
        {
            Debug.Log("YES");
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

