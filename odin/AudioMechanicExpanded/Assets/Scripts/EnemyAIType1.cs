using UnityEngine;
using System.Collections;

public class EnemyAIType1 : EnemyProperties
{
	// MOVEMENT
	Vector3 leashVector;
	float leashLength;
	const float MAXLEASHLENGTH = 40.0f;
	
	// Wander Redirection
	RaycastHit redirectionHit;
	bool detectedTerrain;
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
		enemyState = EnemyState.wander;
		detectedTerrain = true;
		turnTimer = 2.0f;
	}
	
	void Update ()
	{
		// Death
		if(currentHealth <= 0)
			enemyState = EnemyState.dead;
		switch(enemyState)
		{
		case EnemyState.wander:
			transform.Translate(Vector3.forward * (WALKSPEED*Time.deltaTime));
			animation.Play("Walk");
			leashVector = transform.position - transform.root.position;
			leashLength = Mathf.Sqrt((Mathf.Pow(leashVector.x, 2.0f) + Mathf.Pow(leashVector.y, 2.0f) + Mathf.Pow(leashVector.z, 2.0f)));
			if((leashLength > MAXLEASHLENGTH || detectedTerrain) && turnTimer > 2.0f)
			{
				transform.Rotate(0.0f, Random.Range(135.0f, 225.0f), 0.0f);
				turnTimer = 0.0f;
			}
			turnTimer += Time.deltaTime;
			detectedTerrain = false;

			// REDIRECTION RAYCAST
			for(int i=0; i<rayDirection.Length; i++)
			{
				theta = (i+1)*(7.2f);
				theta *= (Mathf.PI/180.0f);
				rayDirection[i] = Vector3.Normalize(new Vector3(transform.localRotation.x + REDIRECTIONRADIUS*Mathf.Sin(theta), 1.5f,
					transform.localRotation.z + REDIRECTIONRADIUS*Mathf.Cos(theta)));
				Debug.DrawRay(transform.position, rayDirection[i] * REDIRECTIONRADIUS, Color.blue);
				
				if(Physics.Raycast(transform.position, rayDirection[i], out redirectionHit, REDIRECTIONRADIUS))
				{
					if(redirectionHit.collider.name == "Terrain")
					{
						detectedTerrain = true;
					}
				}
			}
			break;
		case EnemyState.chase:
			transform.Translate(Vector3.forward * (RUNSPEED*Time.deltaTime));
			animation.CrossFade("Run");
			animation["Run"].speed = 0.75f;
			DetectPlayerToAttack();
			break;
		case EnemyState.fight:
			animation.CrossFade("Slam");
			DetectPlayerToAttack();
			break;
		case EnemyState.dead:
			StartCoroutine("EnemyDeath");
			break;
		}
	}

	/*
	void OnTriggerStay(Collider c)
	{

		if(c.gameObject.name == "Player" && enemyState != EnemyState.fight)
		{
			enemyState = EnemyState.chase;
			player = c.gameObject;
			if(canGrowl)
			{
				audio.PlayOneShot(sfxGrowl);
				canGrowl = false;
			}
		}
	}
	void OnTriggerExit(Collider c)
	{
		if(c.gameObject.name == "Player")
		{
			canGrowl = true;
			enemyState = EnemyState.wander;
		}
	}
	*/

	void DetectPlayerToAttack()
	{
		transform.rotation = FacePlayer(gameObject, player, false, TURNSPEED);
		
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
				if(attackHit.collider.name == "Player")
				{
					enemyState = EnemyState.fight;
					isAttacking = true;
				}
			}
		}
		if(isAttacking == false)
			enemyState = EnemyState.chase;
	}
}
