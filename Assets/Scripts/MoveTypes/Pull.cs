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
        player = GameObject.Find("Player");

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

            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.Hurt(item.GetStats()["pull_damage"]);

            Vector3 pullLine = player.transform.position - transform.position; 
            enemy.gameObject.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position) * 20, ForceMode2D.Impulse);
            enemy.Stun(0.7f);

            // Do hit idicator when enemy gets hurtd
            Vector3 contact = collider.bounds.ClosestPoint(transform.position);
            GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
            Instantiate(hitPrefab, contact, Quaternion.identity);
        }
    }
}