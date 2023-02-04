using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public float speed;
    public float damage;

    // Rotation Variables
    private Collider2D[] senseArea;
    public float rotationSpeed;
    public float rotationModifier;
    public float senseRadius;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed *= 1000;
        currentHealth = maxHealth;
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

    private void OnCollisionEnter2D(Collision2D collider) {
        // Do Damage to Player and get KB
        if(collider.gameObject.tag == "Player") {
            Player player = collider.gameObject.GetComponent<Player>();
            player.Hurt(damage);

            rb.AddForce(-transform.up * 20000 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    public void Hurt(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, -1, maxHealth);

        // Die
        if(currentHealth < 1) {
            Destroy(gameObject);
        }
    }

    public void Heal(float healing) {
        currentHealth = Mathf.Clamp(currentHealth + healing, -1, maxHealth);
    }

    public void Knockback(float kb) {
        rb.AddForce(-transform.up * kb * 10000 * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Stun (float seconds) {
        StartCoroutine(IEStun(seconds));
    }

    private IEnumerator IEStun(float seconds) {
        float tempSpeed = speed;
        this.speed = 0;

        yield return new WaitForSeconds(seconds);
        this.speed = tempSpeed;
        Debug.Log("WaitAndPrint " + Time.time);
    }
}
