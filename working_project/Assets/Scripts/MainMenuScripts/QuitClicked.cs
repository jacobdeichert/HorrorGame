using UnityEngine;
using System.Collections;

public class QuitClicked : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
    }

    void OnMouseDown()
    {
        Application.Quit();
    }
}
