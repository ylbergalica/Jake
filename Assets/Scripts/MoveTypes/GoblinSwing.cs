using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwing : MonoBehaviour
{
    private Quaternion lockRot;
    private Quaternion lockRotParent;

	public ScriptableObject enemyReference;
	private IGoblin enemyType;
	private GoblinAI goblin;

    // Start is called before the first frame update
    void Start()
    {
        lockRotParent = transform.parent.rotation;
        lockRot = transform.rotation;

		enemyType = (IGoblin)enemyReference;
		goblin = transform.parent.GetComponent<GoblinAI>();
		goblin.Stun(gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
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
            player.Hurt(enemyType.GetStats()["damage"]);
            player.Knockback(transform, enemyType.GetStats()["damage"] / 4);

            // Do hit idicator when enemy gets hurt
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);
        }
    }
}
