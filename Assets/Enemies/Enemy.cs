using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;
    public float speed;

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
    }

    private void FixedUpdate()
    {
        senseArea = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), senseRadius);

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
        if(health < 1) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Weapon"){
            GameObject item = collision.GetComponent<SwingLock>().itemReference;
            health -= item.GetComponent<Item>().damage;
        }
    }
}
