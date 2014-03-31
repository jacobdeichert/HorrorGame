using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour 
{
	bool isIdle;
	bool isWalking;
	bool isSprinting;
	public string animationName;
	// Use this for initialization
	void Start()
	{
		if (isIdle)
		{
			animationName = "idle";
			animation.wrapMode = WrapMode.Loop;
			animation[animationName].wrapMode = WrapMode.Loop;
		}
		else if(!isIdle)
		{
			animationName = "run";
			animation.wrapMode = WrapMode.Loop;
			animation[animationName].wrapMode = WrapMode.Loop;
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A)||
			Input.GetKeyDown(KeyCode.W)||
			Input.GetKeyDown(KeyCode.S)||
			Input.GetKeyDown(KeyCode.D))
		{
			animation["run"].speed = 0.5f;
			isIdle = false;
			isWalking = true;
			animation.CrossFade("run");
			
		}
		else if (Input.GetKeyUp(KeyCode.A)||
				 Input.GetKeyUp(KeyCode.W)||
				 Input.GetKeyUp(KeyCode.S)||
				 Input.GetKeyUp(KeyCode.D))
		{
			animationName = "idle";
			isIdle = true;
			isWalking = false;
			animation.CrossFade("idle");
		}
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
		{
			animation["run"].speed = 1.0f;
			animation.CrossFade("run");
		}
		 if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) 
		{
			if (isWalking) 
			{
                animation["run"].speed = 0.5f;
                animation.CrossFade("run");
			}
			else if (!isWalking)
			{
				if (isIdle)
				{
					animation.Play("Idle");
				}
				else if (!isIdle)
				{
					animation["run"].speed = 0.5f;
					animation.Play("run");
				}

			}
		}
	}
}
