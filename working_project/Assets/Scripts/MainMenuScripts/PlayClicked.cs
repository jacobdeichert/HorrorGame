using UnityEngine;
using System.Collections;

public class PlayClicked : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        Application.LoadLevel("Level1");
        GetComponent<ButtonScript>().Disable();
    }
}
