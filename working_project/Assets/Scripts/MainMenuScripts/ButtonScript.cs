using UnityEngine;
using System.Collections;

public class ButtonScript : MonoBehaviour {

    public Texture HoveredTexture;
    public Texture DefaultTexture;
    public Texture DisabledTexture;

    bool enabled = true;

	// Use this for initialization
	void Start () {
        guiTexture.texture = DefaultTexture;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Disable()
    {
        enabled = false;
        guiTexture.texture = DisabledTexture;
    }

    public void Enable()
    {
        enabled = true;
        guiTexture.texture = DefaultTexture;
    }

    void OnMouseOver()
    {
        if(enabled)
            guiTexture.texture = HoveredTexture;
    }

    void OnMouseExit()
    {
        if (enabled)
            guiTexture.texture = DefaultTexture;
        else
            guiTexture.texture = DisabledTexture;
    }
}
