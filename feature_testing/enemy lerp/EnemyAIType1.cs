using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	private List<Node> nodes = new List<Node>();
	private List<Node> path = new List<Node>();
	private Node startNode;
	private Node endNode;

	//lerp things
	Vector2 lerpStart, lerpEnd, lerpDistance;
	float lerpTime, elapsedTime;

	void Start () 
	{
		maxHealth = 120;
		currentHealth = 120;
		enemyState = EnemyState.idle;
		moveSpeed = 0.0f;
		turnTimer = 2.0f;

		gameObject.AddComponent<PathGen>();
		foreach (Node node in GameObject.FindObjectsOfType<Node>())
			nodes.Add(node);
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
			//lerp
			elapsedTime += Time.deltaTime;
			if (elapsedTime > lerpTime){
				elapsedTime = lerpTime;
			}
			transform.position = new Vector3((lerpStart.x + lerpDistance.x * (elapsedTime / lerpTime)),
			                                 transform.position.y,
			                                 (lerpStart.y + lerpDistance.y * (elapsedTime / lerpTime)));
			if (elapsedTime == lerpTime){
				//lerp to next node
				path.RemoveAt(0);
				if (path.Count > 0){
					BeginLerp();
				}
				else{
					enemyState = EnemyState.idle;
				}
			}
			//^^
			/*transform.Translate(Vector3.forward * (moveSpeed*Time.deltaTime));
			transform.rotation = FaceTarget(false, chaseDirection);
			animation.CrossFade(animationName);
			animation[animationName].speed = animationSpeed;
			DetermineAttackRange();*/
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
	private void BeginLerp(){
		lerpStart = new Vector2(transform.position.x, transform.position.z);
		lerpEnd = new Vector2(path[0].transform.position.x, path[0].transform.position.z);

		lerpDistance = lerpEnd - lerpStart;
		
		lerpTime = 0.2f;
		elapsedTime = 0.0f;
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
	
	void DetermineAttackRange()
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

	Quaternion FaceTarget(bool isRotatingPitch, Vector3 direction)
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

		// PATH FINDING
		if (path.Count == 0){
			ResetPath(sourceInfo.transform.position);
			path = gameObject.GetComponent<PathGen>().CalculatePath(startNode, endNode, nodes);
			if (path.Count > 0){
				foreach (Node node in path){
					if (node != null){
						node.path = true; //sets nodes path true to change it's color
					}
				}
				BeginLerp();
			}
			else{
				Debug.Log("No path.");
			}
		}

		// DETERMINE RESPONSE LEVEL
		if(audioSourceRelativeVolume < 0)
		{
			enemyState = EnemyState.idle;
			moveSpeed = 0.0f;
			animationSpeed = 1.0f;
		}
		else if(audioSourceRelativeVolume > 0 && audioSourceRelativeVolume < 20)
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

	private void ResetPath(Vector3 sourcePos)
	{
		Vector2 startPos = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.z));
		Vector2 endPos = new Vector2(Mathf.Round(sourcePos.x), Mathf.Round(sourcePos.z));
		foreach (Node node in nodes)
		{
			node.path = false;
			node.parent = null;
			if (new Vector2(node.transform.position.x, node.transform.position.z) == startPos)
				startNode = node;
			else if(new Vector2(node.transform.position.x, node.transform.position.z) == endPos)
				endNode = node;
		}
	}
}
