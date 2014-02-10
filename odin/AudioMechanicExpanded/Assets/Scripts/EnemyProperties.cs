using UnityEngine;
using System.Collections;

public class EnemyProperties : MonoBehaviour
{
	protected enum EnemyState { idle, wander, chase, fight, shoot, dead }
	protected EnemyState enemyState;
	
	protected int maxHealth;
	protected int currentHealth;
	protected bool canHitPlayer;
	protected const float TURNSPEED = 4.0f;
	protected const float WALKSPEED = 2.0f;
	protected const float RUNSPEED = 6.0f;

	/*
	public AudioClip sfxHurt;
	public AudioClip sfxGrowl;
	public AudioClip sfxAttack;
	public AudioClip sfxDeath;
	*/
	protected bool canGrowl = true;
	
	protected GameObject player;

	/*
	void ApplyDamage(int damage)
	{
		currentHealth -= damage;
		audio.PlayOneShot(sfxHurt);
		if(currentHealth < 0)
			currentHealth = 0;
		HUDandMenus.LastEnemyHitHealth(currentHealth, maxHealth);
	}

	IEnumerator EnemyDeath()
	{
		animation.CrossFade("Fall");
		audio.PlayOneShot(sfxDeath);
		yield return new WaitForSeconds(animation.clip.length);
		Destroy(gameObject);
	}
	*/
	protected Quaternion FacePlayer(GameObject enemy, GameObject player, bool isRotatingPitch, float turnSpeed)
	{
		Vector3 playerDirection = player.transform.position - enemy.transform.position;
		if(isRotatingPitch == false)
			playerDirection.y = 0;
		Quaternion rotation = Quaternion.LookRotation(playerDirection);
		return Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * turnSpeed);
	}
}
