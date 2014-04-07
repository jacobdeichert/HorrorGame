using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour
{
    private enum AnimationState { idle, walk, run };
    private AnimationState state;

    // walk and run both use the "run" animation
    // example use: animationNames[(int)state]
    private string[] animationNames = {"idle", "run", "run" };


    void Start() {
        state = AnimationState.idle;
        animation.wrapMode = WrapMode.Loop;
    }


    void Update() {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            // walk
            state = AnimationState.walk;
            animation["run"].speed = 0.5f;

            // check sprinting
            if (Input.GetButton("Sprint")) {
                state = AnimationState.run;
                animation["run"].speed = 1.0f;
            }
        } else {
            // idle
            state = AnimationState.idle;
        }

        // crossfade to the correct animation
        animation.CrossFade(animationNames[(int)state]);
    }
}
