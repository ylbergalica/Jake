using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    public ScriptableObject itemReference;
    private IItem item;
    public ParticleSystem particleSystem;

    private Quaternion lockRot;
    private Quaternion lockRotParent;
    // private Vector3 lockPos;

    // Start is called before the first frame update
    void Start()
    {
        lockRotParent = transform.parent.rotation;
        lockRot = transform.rotation;

        item = (IItem)itemReference;

        particleSystem.Play();
        // Debug.Log(particleSystem.main.duration);
        Destroy(gameObject, particleSystem.main.duration + 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.parent.rotation = lockRotParent;
        transform.rotation = lockRot;
    }

    // void OnTriggerEnter2D(Collider2D collider) {
    //     // Damage an Enemy
    //     if(collider.gameObject.tag == "Enemy"){
    //         Enemy enemy = collider.GetComponent<Enemy>();
    //         enemy.Hurt(item.GetStats()["thrust_damage"]);
    //         enemy.Knockback(item.GetStats()["thrust_knockback"]);

    //         // Do hit idicator when enemy gets hurtd
    //         Vector3 contact = collider.bounds.ClosestPoint(transform.position);
    //         GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
    //         Instantiate(hitPrefab, contact, Quaternion.identity);
    //     }
    // }
}
