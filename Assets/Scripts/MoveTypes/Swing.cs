using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public ScriptableObject itemReference;
    private IItem item;

    private Quaternion lockRot;
    private Quaternion lockRotParent;
    // private Vector3 lockPos;

    // Start is called before the first frame update
    void Start()
    {
		if (Random.Range(-1, 1) < 0) {
			transform.Rotate(0, 180, 0);
		}

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

    void OnTriggerEnter2D(Collider2D collider) {
        // Damage an Enemy
        if(collider.gameObject.tag == "Enemy"){
            IEnemy enemy = collider.GetComponent(typeof(IEnemy)) as IEnemy;
            enemy.Hurt(item.GetStats()["swing_damage"]);
			
			float knockback = item.GetStats()["swing_knockback"];
            enemy.Knockback(transform, knockback);

            // Do hit idicator when enemy gets hurtd
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);

			// Shake the camera
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f + knockback*0.005f, 20f + knockback*1.1f);
        }

		if (collider.gameObject.tag == "Destroyable") {
			// DEPRECATED
			// Vector2 contact = contacts[0];
			// Vector2 contact = collider.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);
			// Look ahead of the contact point to see if there is a tile there
			// Vector2 direction = contact - (Vector2)transform.parent.position;
			// collider.GetComponent<TileDestroyer>().DestroyTile(contact + direction.normalized * 10f);
		}
    }
}
