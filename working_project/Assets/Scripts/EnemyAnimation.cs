using UnityEngine;
using System.Collections;

public class EnemyAnimation : EnemyAI {
    EnemyAI.EnemyState enemyState;
	// Use this for initialization
	void Start () {
        enemyState = EnemyState.wander;
        animation["walk"].speed = 0.5f;
        animation.CrossFade("walk");
	}
	
	// Update is called once per frame
	void Update () {
        switch (enemyState) 
        {
            case EnemyState.wander:
                animation["walk"].speed = 0.5f;
                animation.CrossFade("walk");
                break;
            case EnemyState.idle:
                animation.CrossFade("idle");
                break;
            case EnemyState.chaseFast:
                animation["run"].speed = 1.0f;
                animation.CrossFade("run");
                break;
            case EnemyState.chaseSlow:
                animation["run"].speed = 0.7f;
                animation.CrossFade("run");
                break;
            case EnemyState.searchFast:
                animation["walk"].speed = 1.0f;
                animation.CrossFade("walk");
                break;
            case EnemyState.searchSlow:
                animation["walk"].speed = 0.5f;
                animation.CrossFade("walk");
                break;
            case EnemyState.fight:
                animation.CrossFade("fight");
                break;
            case EnemyState.dead:
                break;
        }
	}
}
