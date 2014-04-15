using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public enum EnemyState { idle, wander, searchSlow, searchFast, chaseSlow, chaseFast, fight, shoot, dead }
	public EnemyState enemyState;

	// ANIMATION
	float animationSpeed;
	string animationName;

	// PATH FINDING
	PathFinder pathFinder;
    // random selection while wandering
    const float MIN_RANDOM_PATH_DIS = 5.0f;
    Vector3 target;
    Vector3 distToTarget = new Vector3();
    bool needNewPath;

	GameObject playerTarget = null;

	// MOVEMENT
	const float TURNSPEED = 4.0f;
	float wanderTargetTimer = 0.0f;
	bool turned;

	Vector3 chaseDirection;
	float moveSpeed;

    //lerp things
    Vector2 lerpStart, lerpEnd, lerpDistance;
    float lerpTime, elapsedTime, lerpSpeed = 0.5f;
	//path target time
	private const float PATH_RESET_TIME = 5.0f;
	private float pathReset = 0.0f;

	// DETECTION
	const float REDIRECTIONRADIUS = 5.0f;
	const float ATTACKRADIUS = 2.25f;
	RaycastHit redirectionHit;
	RaycastHit attackHit;
	bool canAttack, isAttacking, canDamagePlayer;
	Vector3[] rayDirection = new Vector3[50];
	float theta;

	public AudioClip sfxHurt;
	public AudioClip sfxGrowl;
	public AudioClip sfxAttack;
	public AudioClip sfxDeath;
	bool canGrowl = true;

	void Start()
	{
		pathFinder = GetComponent<PathFinder>();
        gameObject.AddComponent<PathGen>();
		//need to set pathGen's nodeSize && mapSize
		gameObject.GetComponent<PathGen>().nodeSize = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>().wallSize;
		gameObject.GetComponent<PathGen>().mapSize = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>().mapwidth;

        enemyState = EnemyState.wander;
        needNewPath = true;
		moveSpeed = 0.0f;

		wanderTargetTimer = 2.0f;
        distToTarget = target = Vector3.zero;	
	}

	void Update ()
	{
		switch(enemyState)
		{
		case EnemyState.idle: // This may not exist for much longer
			animation.Play("Idle");
			if (Input.GetKeyDown(KeyCode.P))
				enemyState = EnemyState.wander;
			break;
		case EnemyState.wander:
			animation.Play("Run");
            if (Input.GetKeyDown(KeyCode.P))
                enemyState = EnemyState.idle;

            distToTarget = transform.position - target;

            if (needNewPath)
            {
				target = Vector3.zero;

				while (target == Vector3.zero)
                {
                    int randIndex = Random.Range(0, pathFinder.nodes.Count - 1);
                    if (!pathFinder.nodes[randIndex].wall)
                    {
                        target = pathFinder.nodes[randIndex].transform.position;
					}
				}
				pathFinder.ResetPath(target);
                wanderTargetTimer = 0.0f;
                BeginLerp();
                needNewPath = false;
            }

            wanderTargetTimer += Time.deltaTime;
            Move();
			//check if any players are visible
			SearchForPlayers();

			if(wanderTargetTimer > 30.0f || distToTarget.magnitude < MIN_RANDOM_PATH_DIS){
                //needNewPath = true;
			}

			break;
		case EnemyState.searchSlow:
		case EnemyState.searchFast:
			//if enemy heard a sound
			Move();
			animation.CrossFade(animationName);
			animation[animationName].speed = animationSpeed;
			DetermineAttackRange();
			break;
		case EnemyState.chaseSlow:
		case EnemyState.chaseFast:
			//if enemy has a target
			if (playerTarget != null){
				pathReset += Time.deltaTime;
				//path towards target player
				if (pathReset > PATH_RESET_TIME){
					pathFinder.ResetPath(playerTarget.transform.position);
					pathReset = 0.0f;
				}
				//if player gets to far away set to wander
				if (pathFinder.path.Count > 15){
					playerTarget = null;
					enemyState = EnemyState.wander;
					needNewPath = true;
					pathReset = 0.0f;
				}
				//if enemy in range attack
				else if (Vector3.Distance(transform.position, playerTarget.transform.position) < 5 || pathFinder.path.Count <= 1){
					enemyState = EnemyState.fight;
					//isAttacking = true;
				}
				else{
					animation.Play("Run");
					Move();
				}
			}
			//if playerTarget is for some reason null start to wander
			else{
				enemyState = EnemyState.wander;
				needNewPath = true;
			}
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

    void Move()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > lerpTime)
            elapsedTime = lerpTime;
        transform.position = new Vector3((lerpStart.x + lerpDistance.x * (elapsedTime / lerpTime)),
                                         transform.position.y,
                                         (lerpStart.y + lerpDistance.y * (elapsedTime / lerpTime)));
        if (elapsedTime == lerpTime)
        {
            //lerp to next node
			if (pathFinder.path.Count > 0){
	            pathFinder.path.RemoveAt(0);
	            if (pathFinder.path.Count > 0)
	            {
	                BeginLerp();
				}
	            else
	            {
					Debug.Log("arrived");
					enemyState = EnemyState.wander;
					needNewPath = true;
				}
			}
			//if new path is 0 need new path
			else{
				needNewPath = true;
			}
        }
        // transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
        //transform.rotation = FaceTarget(false, chaseDirection);
    }
    private void BeginLerp()
    {
        lerpStart = new Vector2(transform.position.x, transform.position.z);
        lerpEnd = new Vector2(pathFinder.path[0].transform.position.x, pathFinder.path[0].transform.position.z);
		//faces enemy towards next node
		transform.LookAt(new Vector3(lerpEnd.x, transform.position.y, lerpEnd.y));

        lerpDistance = lerpEnd - lerpStart;

        lerpTime = lerpSpeed;
        elapsedTime = 0.0f;
    }

	void OnHearSound(SoundSourceInfo sourceInfo)
	{
		// DETECT SOUND
		chaseDirection  = sourceInfo.transform.position - transform.position;
		float audioSourceDistance = chaseDirection.magnitude;

		// PATH FINDING
		pathFinder.ResetPath(sourceInfo.transform.position);
		BeginLerp();

		// Volume is increased 100 fold for ease of use with distance.
		// Distance is subtracted to resemble the monster's perception of the source's volume, making volume decay linearly, which might be inaccurate.
		//float audioSourceRelativeVolume = (sourceInfo.volume * 100) - audioSourceDistance;
		float audioSourceRelativeVolume = (sourceInfo.volume * 100) - pathFinder.path.Count;
		chaseDirection.Normalize();

		// DETERMINE RESPONSE LEVEL
		if(audioSourceRelativeVolume < 0)
		{
			/*enemyState = EnemyState.wander;
			lerpSpeed = 0.5f;
			moveSpeed = 0.0f;
			animationSpeed = 0.5f;*/
		}
		else if(audioSourceRelativeVolume > 0 && audioSourceRelativeVolume < 20)
		{
			enemyState = EnemyState.wander;
			lerpSpeed = 0.45f;
			moveSpeed = 2.0f;
			animationSpeed = 1.0f;
			animationName = "Walk";
		}
		else if(audioSourceRelativeVolume > 19 && audioSourceRelativeVolume < 40)
		{
			enemyState = EnemyState.searchSlow;
			lerpSpeed = 0.45f;
			moveSpeed = 3.0f;
			animationSpeed = 0.7f;
			animationName = "Walk";
		}
		else if(audioSourceRelativeVolume > 39 && audioSourceRelativeVolume < 60)
		{
			enemyState = EnemyState.searchFast;
			lerpSpeed = 0.4f;
			moveSpeed = 4.0f;
			animationSpeed = 0.9f;
			animationName = "Run";
		}
		/*else if(audioSourceRelativeVolume > 59 && audioSourceRelativeVolume < 80)
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
		}*/
	}

	void OnTriggerStay(Collider c)  // <------- These refer to the player being in a particularly close proximity, and might need to be deleted/changed
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
	void OnTriggerExit(Collider c)  // <------- These refer to the player being in a particularly close proximity, and might need to be deleted/changed
    {
        if(c.gameObject.tag == "Player")
        {
            canGrowl = true;
            enemyState = EnemyState.wander;
        }
	}

	void ApplyDamage()
	{
		audio.PlayOneShot(sfxHurt);

		// Set a timer delay or something if monster is lured into a trap?
	}
	
	Quaternion FaceTarget(bool isRotatingPitch, Vector3 direction)
	{
		if(isRotatingPitch == false)
			direction.y = 0;
		Quaternion rotation = Quaternion.LookRotation(direction);
		return Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * TURNSPEED);
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
	void SearchForPlayers(){
		int maxRange = 70;
		RaycastHit hit;
		//if player is visible player is set to current target
		foreach (Player player in GameObject.FindObjectsOfType<Player>()){
			//only raycast if player in within range
			if(Vector3.Distance(transform.position, player.transform.position) < maxRange ){
				//if in range raycast
				if(Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, maxRange)){
					//if raycast hits player
					if(hit.transform == player.transform){
						Debug.Log("player targeted.");
						playerTarget = player.gameObject;
						enemyState = EnemyState.chaseFast;
						//for now break, but we may have to take into account both players at once
						break;
					}
				}
			}
		}
	}
}
