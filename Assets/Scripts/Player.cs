using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    private Inventory inventory;
    public UI_Inventory ui_inventory;
    public int slotsCount;
    private GameObject ui_pockets;
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

    // Stats
    [Header ("Stats")]
    public float speed;
    public float maxHealth;
    public float currentHealth;

    // iFrames
    [Header ("iFrames")]
    public float iFramesDuration;
    public int numberOfFlashes;
    private SpriteRenderer sprender;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        speed *= 1000;
        currentHealth = maxHealth;
        sprender = gameObject.GetComponent<SpriteRenderer>();

        inventory = new Inventory(slotsCount, ui_inventory);
        this.ui_pockets = ui_inventory.ui_pockets;
        ui_pockets.SetActive(false);
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
            RefreshAnimations();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2)){
            // Hotbar Slot 2
            activeSlot = inventory.ActivateSlot(1);
            RefreshAnimations();
        }

        // Open Inventory Pockets
        if(Input.GetKeyDown(KeyCode.E)){
            if(ui_pockets.activeSelf == false){
                ui_pockets.SetActive(true);
            }
            else{
                ui_pockets.SetActive(false);
            }

            ui_inventory.ResetStage();
        }

        // Use Item in Active Slot
        if(Input.GetMouseButtonDown(0)){
            ui_inventory.RefreshInventory();
            RefreshAnimations();
            
            if(heldItem != null && Time.realtimeSinceStartup > heldItem.GetLastUse() + heldItem.GetStats()["cooldown"]){
                heldItem.SetLastUse(Time.realtimeSinceStartup);

                heldItem.UsePrimary();
            }
        }

        // Test Item Swap
        if(Input.GetKeyDown(KeyCode.Space)) {
            heldItem = inventory.GetItemIn(activeSlot);
            ui_inventory.RefreshInventory();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // Pick up Items
        if(collider.tag == "Item"){
            Item item = collider.GetComponent<Item>();

            // Add item to inventory and refresh activeslot
            inventory.AddItem(item);
            heldItem = inventory.GetItemIn(activeSlot);

            // Hide the item
            collider.gameObject.SetActive(false);

            RefreshAnimations();
        }
    }

    private void RefreshAnimations() {
        // Set the held item animation info on pick up ///WORKS BCS PASSING BY REFERENCE///
        heldItem = inventory.GetItemIn(activeSlot);
        
        // if (inventory.GetItemCount() != 0 && heldItem != null){
        //     if(heldItem.GetComponent<Item>().primary != null) {
        //         heldItemAnimator = heldItem.GetComponent<Item>().primary.GetComponent<Animator>();
        //         heldItemFire = heldItemAnimator.runtimeAnimatorController.animationClips[0];
        //     }
        // }
        // else {
        //     heldItemAnimator = null;
        //     heldItemFire = null;
        // }
    }

    // Get Hurt and Start iFrames
    public void Hurt(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, -1, maxHealth);
        StartCoroutine(Invulnerability()); // Needs research ##########

        // Die
        if(currentHealth < 0) {
            Debug.Log("You Died!");
            Destroy(gameObject);
        }
    }

    public void Heal(float healing) {
        currentHealth = Mathf.Clamp(currentHealth + healing, -1, maxHealth);
    }

    private IEnumerator Invulnerability(){ // Needs research ##########
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for(int i = 0; i < numberOfFlashes; i++) {
            sprender.color = new Color(1, 1, 1, 0f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2)); // Needs research ##########
            sprender.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    public Inventory GetInventory() {
        return this.inventory;
    }
}
