using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour
{
    public ScriptableObject itemReference;
    private IItem item;

    private Quaternion lockRot;
    private Quaternion lockRotParent;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
		if (Random.Range(-1, 1) < 0) {
			transform.Rotate(0, 180, 0);
		}
		
        player = GameObject.FindWithTag("Player");

        lockRotParent = transform.parent.rotation;
        lockRot = transform.rotation;

        item = (IItem)itemReference;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = lockRotParent;
        transform.rotation = lockRot;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        // Damage an Enemy
        if(collider.gameObject.tag == "Enemy"){
            player.GetComponent<Player>().Tekkai(0.5f);

            IEnemy enemy = collider.GetComponent(typeof(IEnemy)) as IEnemy;
            enemy.Hurt(item.GetStats()["pull_damage"]);

			Rigidbody2D colliderRb = collider.gameObject.GetComponent<Rigidbody2D>();
            Vector3 pullLine = player.transform.position - transform.position;
			float pullForce = item.GetStats()["pull_force"] * colliderRb.mass * 100f;
            colliderRb.AddForce((player.transform.position - transform.position) * pullForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
            enemy.Stun(0.7f);

            // Do hit idicator when enemy gets hurtd
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);

			// Shake the camera
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f, 30f);
        }
    }
}
