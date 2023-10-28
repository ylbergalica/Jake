using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwing : MonoBehaviour
{
    private Quaternion lockRot;
    private Quaternion lockRotParent;

	public ScriptableObject enemyReference;
	private IDummy enemyType;
	private GoblinAI goblin;

    // Start is called before the first frame update
    void Start()
    {
        lockRotParent = transform.parent.rotation;
        lockRot = transform.rotation;
		float swingLength = gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

		enemyType = (IDummy)enemyReference;
		goblin = transform.parent.GetComponent<GoblinAI>();
		goblin.Stun(swingLength);
		goblin.Busy(swingLength);
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = lockRotParent;
        transform.rotation = lockRot;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Damage an Enemy
        if(collider.gameObject.tag == "Player"){
            Player player = collider.gameObject.GetComponent<Player>();
            player.Hurt(enemyType.GetStats()["primaryDamage"]);
            player.Knockback(transform, enemyType.GetStats()["primaryKnockback"]);

            // Do hit idicator when enemy gets hurt
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);
        }
    }
}
