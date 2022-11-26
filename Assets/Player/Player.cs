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

            if(heldItem != null) {
                Debug.Log(heldItem.name);
            }
        }
        else if(Input.GetKey(KeyCode.Alpha2)){
            // Hotbar Slot 2
            activeSlot = inventory.ActivateSlot(1);
            heldItem = inventory.GetItemIn(activeSlot);
            
            if(heldItem != null) {
                Debug.Log(heldItem.name);
            }
        }
        else if(Input.GetKey(KeyCode.Alpha3)){
            // Hotbar Slot 3
            activeSlot = inventory.ActivateSlot(2);
            heldItem = inventory.GetItemIn(activeSlot);
            
            if(heldItem != null) {
                Debug.Log(heldItem.name);
            }
        }

        // Use Item in Active Slot
        if(Input.GetMouseButtonDown(0) && heldItem != null && Time.realtimeSinceStartup > lastAttack + heldItem.cooldown + heldItemFire.length){
            lastAttack = Time.realtimeSinceStartup;
            Vector3 offset = transform.right * heldItem.offset;

            Instantiate(heldItem.use, transform.position + offset, transform.rotation, gameObject.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Item"){
            Item item = other.GetComponent<Item>();

            inventory.AddItem(item);
            heldItem = inventory.GetItemIn(activeSlot);

            other.gameObject.SetActive(false);

            heldItemAnimator = heldItem.GetComponent<Item>().use.GetComponent<Animator>();
            heldItemFire = heldItemAnimator.runtimeAnimatorController.animationClips[0];
        }
    }
}
