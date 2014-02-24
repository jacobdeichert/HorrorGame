using UnityEngine;
using System.Collections;

public class EnemyAIType1 : EnemyProperties
{
	// ANIMATION ADJUSTMENTS
	float animationSpeed;
	string animationName;

	// SOUND/PLAYER DETECTION
	Vector3 chaseDirection;

	// Wander Redirection
	RaycastHit redirectionHit;
	bool turned;
	float turnTimer = 0.0f;
	Vector3[] rayDirection = new Vector3[50];
	float theta;
	const float REDIRECTIONRADIUS = 5.0f;
	
	// Chasing player
	RaycastHit attackHit;
	const float ATTACKRADIUS = 2.25f;
	
	//Attacking
	bool isAttacking;
	bool canDamagePlayer;
	

	void Start () 
	{
		maxHealth = 120;
		currentHealth = 120;
		enemyState = EnemyState.idle;
		moveSpeed = 0.0f;
		turnTimer = 2.0f;
	}
	
	void Update ()
	{
		// Death
		if(currentHealth <= 0)
			enemyState = EnemyState.dead;
		switch(enemyState)
		{
		case EnemyState.idle:
			animation.Play("Idle");
			break;
		case EnemyState.wander:
			if(turnTimer > 4.0f)
			{
				transform.Rotate(0.0f, Random.Range(135.0f, 225.0f), 0.0f);
				turnTimer = 0.0f;
			}
			turnTimer += Time.deltaTime;
			break;
		case EnemyState.searchSlow:
		case EnemyState.searchFast:
		case EnemyState.chaseSlow:
		case EnemyState.chaseFast:
			transform.Translate(Vector3.forward * (moveSpeed*Time.deltaTime));
			transform.rotation = FaceTarget(false, chaseDirection);
			animation.CrossFade(animationName);
			animation[animationName].speed = animationSpeed;
			DetermineAttackRange();
			break;
		case EnemyState.fight:
			animation.CrossFade("Slam");
			DetermineAttackRange();
			break;
		case EnemyState.dead:
			// StartCoroutine("EnemyDeath");
			break;
		}
	}


	void OnTriggerStay(Collider c)
	{
		if(c.gameObject.tag == "Player" && enemyState != EnemyState.fight)
		{
			chaseDirection = c.transform.position - transform.position;
			chaseDirection.Normalize();

			enemyState = EnemyState.chaseFast;
			if(canGrowl)
			{
				moveSpeed = 7.5f;
				animationSpeed = 0.9f;
				animationName = "Run";
				audio.PlayOneShot(sfxGrowl);
				canGrowl = false;
			}
		}
	}
	void OnTriggerExit(Collider c)
	{
		if(c.gameObject.tag == "Player")
		{
			canGrowl = true;
			enemyState = EnemyState.wander;
		}
	}
	
	private void DetermineAttackRange()
	{
		// ATTACK RAYCAST
		isAttacking = false;
		for(int i=0; i<rayDirection.Length; i++)
		{
			theta = (i+1)*(7.2f);
			theta *= (Mathf.PI/180.0f);
			rayDirection[i] = Vector3.Normalize(new Vector3(transform.localRotation.x + ATTACKRADIUS*Mathf.Sin(theta), 1.0f,
				transform.localRotation.z + ATTACKRADIUS*Mathf.Cos(theta)));
			Debug.DrawRay(transform.position, rayDirection[i] * ATTACKRADIUS, Color.red);
			
			if(Physics.Raycast(transform.position, rayDirection[i], out attackHit, ATTACKRADIUS))
			{
				if(attackHit.transform.gameObject.tag == "Player")
				{
					enemyState = EnemyState.fight;
					isAttacking = true;
				}
			}
		}
		if(isAttacking == false)
			enemyState = EnemyState.chaseSlow;
	}

	private Quaternion FaceTarget(bool isRotatingPitch, Vector3 direction)
	{
		if(isRotatingPitch == false)
			direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction);
		return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TURNSPEED);
	}

	void OnHearSound(SoundSourceInfo sourceInfo)
	{
		// DETECT SOUND
		chaseDirection  = sourceInfo.transform.position - transform.position;
		float audioSourceDistance = chaseDirection.magnitude;

		// Volume is increased 100 fold for ease of use with distance.
		// Distance is subtracted to resemble the monster's perception of the source's volume, making volume decay linear, which might be inaccurate.
		float audioSourceRelativeVolume = (sourceInfo.volume * 100) - audioSourceDistance;
		chaseDirection.Normalize();

		// DETERMINE RESPONSE LEVEL
		if(audioSourceRelativeVolume > 0 && audioSourceRelativeVolume < 20)
		{
			enemyState = EnemyState.wander;
			moveSpeed = 2.0f;
			animationSpeed = 0.5f;
			animationName = "Walk";
		}
		else if(audioSourceRelativeVolume > 19 && audioSourceRelativeVolume < 40)
		{
			enemyState = EnemyState.searchSlow;
			moveSpeed = 3.0f;
			animationSpeed = 0.7f;
			animationName = "Walk";
		}
		else if(audioSourceRelativeVolume > 39 && audioSourceRelativeVolume < 60)
		{
			enemyState = EnemyState.searchFast;
			moveSpeed = 4.0f;
			animationSpeed = 0.9f;
			animationName = "Walk";
		}
		else if(audioSourceRelativeVolume > 59 && audioSourceRelativeVolume < 80)
		{
			enemyState = EnemyState.chaseSlow;
			moveSpeed = 5.0f;
			animationSpeed = 0.7f;
			animationName = "Run";
		}
		else if(audioSourceRelativeVolume > 79)
		{
			enemyState = EnemyState.chaseFast;
			moveSpeed = 6.0f;
			animationSpeed = 0.9f;
			animationName = "Run";
		}
	}
}
