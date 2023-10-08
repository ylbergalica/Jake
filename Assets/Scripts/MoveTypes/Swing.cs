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
            enemy.Knockback(transform, item.GetStats()["swing_knockback"]);

            // Do hit idicator when enemy gets hurtd
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);
        }

		if (collider.gameObject.tag == "Destroyable") {
			// Vector2 contact = contacts[0];
			Vector2 contact = collider.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

			// Look ahead of the contact point to see if there is a tile there
			Vector2 direction = contact - (Vector2)transform.parent.position;
			Debug.Log(direction.normalized * 10f);
			Debug.Log(contact + direction.normalized * 10f);
			Debug.Log(contact + new Vector2(10f, 10f));

			collider.GetComponent<TileDestroyer>().DestroyTile(contact + direction.normalized * 10f);
		}
    }
}
