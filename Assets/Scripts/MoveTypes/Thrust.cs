using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : MonoBehaviour
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
            enemy.Hurt(item.GetStats()["thrust_damage"]);

			float knockback = item.GetStats()["thrust_knockback"];
            enemy.Knockback(transform, knockback);

            // Do hit idicator when enemy gets hurtd
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);

			// Shake the camera on hit
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f + knockback*0.005f, 40f + knockback*1.1f);
        }
    }
}
