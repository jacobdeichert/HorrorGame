using UnityEngine;
using System.Collections;

public class FootstepControl : MonoBehaviour
{
    PlayerController playerController;

	void Start () 
    {
        playerController = GetComponent<PlayerController>();
        
	}

	void Update ()
    {
        if (playerController.rigidbody.velocity.x != 0)
        {
            if (!audio.isPlaying)
            {
               audio.Play();
            }

            //Debug.Log("FootStepControl: should make footsteps");
            // Trying to get footsteps sound to play at different speeds based on movement speed
            // Everything here debugs well enough, but still I hear nothing, maybe because of the ambient sound already playing.
            
            if (playerController.IsRunning)
                audio.pitch = 2.0f;
            else
                audio.pitch = 1.0f;
             
        }
        else
            audio.Pause();
	}
}
