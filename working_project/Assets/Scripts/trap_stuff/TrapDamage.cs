using UnityEngine;
using System.Collections;

public class TrapDamage : MonoBehaviour {


    /* Have to attach this script to parts of each trap
     * prefab that have the neccessary box colliders
     * for the player to check collisions on for health damage.
     */

    public float damagePerSecond;
    private bool isTouchingPlayer = false;
    private Player playerToDamage;


    public void OnCollisionEnter(Collision c) {
        // touching player
        Player testPlayer;
        if ((testPlayer = c.contacts[0].otherCollider.transform.root.GetComponent<Player>() as Player) != null) {
            playerToDamage = testPlayer;
            if (!isTouchingPlayer) {
                isTouchingPlayer = true;
                // start the coroutine that lowers the player's health once per second
                StartCoroutine(decreaseHealthPerSecondRepeat(1f));
            }
        }
    }

    void OnCollisionExit(Collision c) {
        // stopped touching player
        if ((Player)(c.contacts[0].otherCollider.transform.root.GetComponent<Player>())) {
            isTouchingPlayer = false;
        }
    }

    public IEnumerator decreaseHealthPerSecondRepeat(float repeatRate) {
        while (isTouchingPlayer) {
            playerToDamage.decreaseHealth(damagePerSecond);
            yield return new WaitForSeconds(repeatRate);
        }
    }
}
