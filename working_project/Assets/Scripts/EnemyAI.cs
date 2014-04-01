using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	enum EnemyState { idle, wander, searchSlow, searchFast, chaseSlow, chaseFast, fight, shoot, dead }
	EnemyState enemyState;

	// ANIMATION
	float animationSpeed;
	string animationName;

	// PATH FINDING
	PathFinder pathFinder;
    // random selection while wandering
    const float MIN_RANDOM_PATH_DIS = 5.0f;
    Vector3 randomPos = new Vector3();
    Vector3 randomDis = new Vector3();
    bool needNewPath;

	// MOVEMENT
	const float TURNSPEED = 4.0f;
	float wanderTargetTimer = 0.0f;
	bool turned;

	Vector3 chaseDirection;
	float moveSpeed;

    //lerp things
    Vector2 lerpStart, lerpEnd, lerpDistance;
    float lerpTime, elapsedTime;

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
		gameObject.GetComponent<PathGen>().nodeSize = 5;
		gameObject.GetComponent<PathGen>().mapSize = 31;

        enemyState = EnemyState.wander;
        needNewPath = true;
		moveSpeed = 0.0f;

		wanderTargetTimer = 2.0f;
        randomDis = randomPos = Vector3.zero;	
	}

	void Update ()
	{
		switch(enemyState)
		{
		case EnemyState.idle: // This may not exist for much longer
			animation.Play("Idle");
			break;
		case EnemyState.wander:
			if (Input.GetKeyDown(KeyCode.P)){
				pathFinder.ResetPath(Vector3.zero);
			}
            /*randomDis = transform.position - randomPos;

            if (needNewPath)
            {
                int index;

                do
                {
                    index = (int)((float)pathFinder.nodes.Count * Random.value);
                    randomPos = pathFinder.nodes[index].transform.position;
                    randomDis = randomPos - transform.position;
                }
                while (!pathFinder.nodes[index].path && randomDis.magnitude < MIN_RANDOM_PATH_DIS);

                pathFinder.ResetPath(randomPos);
                wanderTargetTimer = 0.0f;
                BeginLerp();
                needNewPath = false;
            }

            wanderTargetTimer += Time.deltaTime;
            Move();

			if(wanderTargetTimer > 30.0f || randomDis.magnitude < MIN_RANDOM_PATH_DIS)
                needNewPath = true;         
			*/
			break;
		case EnemyState.searchSlow:
		case EnemyState.searchFast:
		case EnemyState.chaseSlow:
		case EnemyState.chaseFast:
            Move();
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

    void Move()
    {
        Debug.Log("Here");
        elapsedTime += Time.deltaTime;
        if (elapsedTime > lerpTime)
            elapsedTime = lerpTime;
        transform.position = new Vector3((lerpStart.x + lerpDistance.x * (elapsedTime / lerpTime)),
                                         transform.position.y,
                                         (lerpStart.y + lerpDistance.y * (elapsedTime / lerpTime)));
        if (elapsedTime == lerpTime)
        {
            //lerp to next node
            pathFinder.path.RemoveAt(0);
            if (pathFinder.path.Count > 0)
                BeginLerp();
            else
                enemyState = EnemyState.idle;
        }
        // transform.Translate(Vector3.forward * (moveSpeed * Time.deltaTime));
        transform.rotation = FaceTarget(false, chaseDirection);
    }
    private void BeginLerp()
    {
        lerpStart = new Vector2(transform.position.x, transform.position.z);
        lerpEnd = new Vector2(pathFinder.path[0].transform.position.x, pathFinder.path[0].transform.position.z);

        lerpDistance = lerpEnd - lerpStart;

        lerpTime = 0.2f;
        elapsedTime = 0.0f;

        Debug.Log("Made it here");
    }

	void OnHearSound(SoundSourceInfo sourceInfo)
	{
		// DETECT SOUND
		chaseDirection  = sourceInfo.transform.position - transform.position;
		float audioSourceDistance = chaseDirection.magnitude;
		
		// Volume is increased 100 fold for ease of use with distance.
		// Distance is subtracted to resemble the monster's perception of the source's volume, making volume decay linearly, which might be inaccurate.
		float audioSourceRelativeVolume = (sourceInfo.volume * 100) - audioSourceDistance;
		chaseDirection.Normalize();
		
		// PATH FINDING
		pathFinder.ResetPath(sourceInfo.transform.position);


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
}
