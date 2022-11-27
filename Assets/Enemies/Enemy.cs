using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    public float damage;
    public float attackCooldown;

    // Rotation Variables
    private Collider2D[] senseArea;
    public float rotationSpeed;
    public float rotationModifier;
    public float senseRadius;

    private Rigidbody2D rb;

    private float lastAttack;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed *= 1000;
    }

    private void FixedUpdate()
    {
        senseArea = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), senseRadius);

        // Check Sense Area for a Player
        foreach (Collider2D collider in senseArea){

            if (collider.gameObject.tag == "Player") {
                
                // roses are red, violets are blue, your code is my code too
                Vector3 vectorToTarget = collider.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
                Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, quart, Time.deltaTime * rotationSpeed);

                // Chase Player
                rb.AddForce(transform.up * speed * Time.deltaTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Can pass away into the other world of forgiveness where it shall lie in peace and forever prosper..
        if(health < 1) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        // Tage Damage from a Weapon
        if(collider.gameObject.tag == "Weapon"){
            GameObject itemReference = collider.GetComponent<SwingLock>().itemReference;
            Item item = itemReference.GetComponent<Item>();

            health -= item.damage;
            rb.AddForce(-transform.up * item.knockback * 10000 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collider) {
        // Do Damage to Player
        // CHANGE COOLDOWN TO PLAYER I-FRAMES
        if(collider.gameObject.tag == "Player" && Time.realtimeSinceStartup > attackCooldown + lastAttack) {
            lastAttack = Time.realtimeSinceStartup;

            collider.gameObject.GetComponent<Player>().health -= damage;
            rb.AddForce(-transform.up * 20000 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
