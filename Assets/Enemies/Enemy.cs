using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health < 1) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("OUCH!");

        if(collision.gameObject.tag == "Weapon"){
            GameObject item = collision.GetComponent<SwingLock>().itemReference;
            health -= item.GetComponent<Item>().damage;
        }
    }
}
