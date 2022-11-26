using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    private Inventory inventory;
    public UI_Inventory ui_inventory;
    private ItemSlot activeSlot;
    private Item heldItem;

    // rotation variables
    private Vector3 mousePos;
    private Vector3 objectPos;
    private float angle;
    
    // Swing Animation and Cooldown
    // public GameObject swing;
    private Animator heldItemAnimator;
    private AnimationClip heldItemFire;
    private float cooldown;
    private float lastAttack;

    // Movement
    public float speed;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed *= 1000;

        inventory = new Inventory(3, ui_inventory);
        activeSlot = inventory.ActivateSlot(0);
        heldItem = inventory.GetItemIn(activeSlot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement Checks
        if(Input.GetKey("w")){
            rb.AddForce(new Vector3(0, speed*Time.deltaTime, 0));
        }
        if(Input.GetKey("a")){
            rb.AddForce(new Vector3(-speed*Time.deltaTime, 0, 0));
        }
        if(Input.GetKey("s")){
            rb.AddForce(new Vector3(0, -speed*Time.deltaTime, 0));
        }
        if(Input.GetKey("d")){
            rb.AddForce(new Vector3(speed*Time.deltaTime, 0, 0));
        }
    }

    void Update(){
        // Rotate Player Towards Mouse
        mousePos = Input.mousePosition;
        mousePos.z = 0;
        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Hotbar Selected Slot
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            // Hotbar Slot 1
            activeSlot = inventory.ActivateSlot(0);
            heldItem = inventory.GetItemIn(activeSlot);
        }
        else if(Input.GetKey(KeyCode.Alpha2)){
            // Hotbar Slot 2
            activeSlot = inventory.ActivateSlot(1);
            heldItem = inventory.GetItemIn(activeSlot);
        }
        else if(Input.GetKey(KeyCode.Alpha3)){
            // Hotbar Slot 3
            activeSlot = inventory.ActivateSlot(2);
            heldItem = inventory.GetItemIn(activeSlot);
        }

        // Use Item in Active Slot
        if(Input.GetMouseButtonDown(0) && heldItem != null && Time.realtimeSinceStartup > heldItem.GetLastUse() + heldItem.cooldown + heldItemFire.length){
            heldItem.SetLastUse(Time.realtimeSinceStartup);
            Vector3 offset = transform.right * heldItem.offset;

            Instantiate(heldItem.use, transform.position + offset, transform.rotation, gameObject.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // Pick up Items
        if(collider.tag == "Item"){
            Item item = collider.GetComponent<Item>();

            // Add item to inventory and refresh activeslot
            inventory.AddItem(item);
            heldItem = inventory.GetItemIn(activeSlot);

            // Hide the 
            collider.gameObject.SetActive(false);

            // Set the held item animation info on pick up ///WORKS BCS PASSING BY REFERENCE///
            heldItemAnimator = heldItem.GetComponent<Item>().use.GetComponent<Animator>();
            heldItemFire = heldItemAnimator.runtimeAnimatorController.animationClips[0];
        }
    }
}
