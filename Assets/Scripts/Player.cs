using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    // Inventory
    [Header ("Inventory")]
    public UI_Inventory ui_inventory;
    public int slotsCount;

    private Inventory inventory;
    private List<GameObject> invItems;
    private GameObject ui_pockets;
    private GameObject ui_abilities;
    private ItemSlot activeSlot;
    private Item heldItem;

    // rotation variables
    private Vector3 mousePos;
    private Vector3 objectPos;
    private float angle;

    // Stats
    [Header ("Stats")]
    public float speed;
    public float maxHealth;
    public float currentHealth;
	public float grabRadius;
	private Collider2D[] grabArea;
	private float grabMultiplier;

    // iFrames
    [Header ("iFrames")]
    public float iFramesDuration;
    public int numberOfFlashes;
    private SpriteRenderer sprender;

    // Dodge Animation
    [Header ("Dodge Variables")]
    // [SerializeField] private Animator dodgeAnimator;
    [SerializeField] private GameObject dodgeAnim;
    [SerializeField] private float dodgeCooldown;
    private float dodgeLength;
    private float dodgeLast;

    private bool isTekkai;
    private bool isBusy;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        rb = gameObject.GetComponent<Rigidbody2D>();
        speed *= 100;
        currentHealth = maxHealth;
        sprender = gameObject.GetComponent<SpriteRenderer>();

        inventory = new Inventory(slotsCount);
        invItems = new List<GameObject>();

        dodgeLength = dodgeAnim.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

        isTekkai = false;
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement Checks
        if(Input.GetKey("w")){
            rb.AddForce(new Vector3(0, speed, 0));
        }
        if(Input.GetKey("a")){
            rb.AddForce(new Vector3(-speed, 0, 0));
        }
        if(Input.GetKey("s")){
            rb.AddForce(new Vector3(0, -speed, 0));
        }
        if(Input.GetKey("d")){
            rb.AddForce(new Vector3(speed, 0, 0));
        }

		// 
		grabArea = Physics2D.OverlapCircleAll(transform.position, grabRadius);
		foreach (Collider2D collider in grabArea){
			if (collider.gameObject.CompareTag("Item")) {
				// Pick up Items
				Rigidbody2D itemBody = collider.gameObject.GetComponent<Rigidbody2D>();
				Vector3 direction = transform.position - collider.transform.position;
				float distance = Vector3.Distance(transform.position, collider.transform.position);
				itemBody.AddForce((direction * (Mathf.Pow(distance, 1.5f)/5f)), ForceMode2D.Force);
			}
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
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

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
        if(Input.GetKeyDown(KeyCode.I)){
            if(ui_pockets.activeSelf == false){
                ui_pockets.SetActive(true);
                ui_abilities.SetActive(true);
            }
            else{
                ui_abilities.SetActive(false);
                ui_pockets.SetActive(false);
            }

            ui_inventory.ResetStage();
        }

        if (!isBusy) {
            // Use Item in Active Slot
            if(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.realtimeSinceStartup) && heldItem.isMoveReady(3, Time.realtimeSinceStartup)){
                    heldItem.SetLastUse(3, Time.realtimeSinceStartup);

                    heldItem.UseTertiary(gameObject);
                }
            }
            else if(Input.GetMouseButtonDown(0)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.realtimeSinceStartup) && heldItem.isMoveReady(1, Time.realtimeSinceStartup)){
                    heldItem.SetLastUse(1, Time.realtimeSinceStartup);

                    heldItem.UsePrimary(gameObject);
                }
            }
            else if(Input.GetMouseButtonDown(1)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.realtimeSinceStartup) && heldItem.isMoveReady(2, Time.realtimeSinceStartup)){
                    heldItem.SetLastUse(2, Time.realtimeSinceStartup);

                    heldItem.UseSecondary(gameObject);
                }
            }
            
            // Dodging
            if(inventory.HasAbility("Dodge") 
				&& Input.GetKeyDown(KeyCode.Space) 
				&& Time.realtimeSinceStartup > dodgeLast + dodgeCooldown + dodgeLength) {
				// Dodge
                Instantiate(dodgeAnim, transform.position, transform.rotation, transform);
                dodgeLast = Time.realtimeSinceStartup;
                Busy(dodgeLength);
				StartCoroutine(Dodge());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // Pick up Items
        if(collider.gameObject.CompareTag("Item")){ // GET ITEM
            PickUpItem(collider.gameObject);
        }
        else if (collider.gameObject.CompareTag("Ability")) { // GET ABILITY
            Ability ability = collider.GetComponent<Ability>();
            inventory.AddAbility(ability.abilityName);

            Destroy(collider.gameObject);
        }
    }

	private void PickUpItem(GameObject itemObject) {
		invItems.Add(itemObject);

		Item item = itemObject.GetComponent<Item>();

		// Add item to inventory and refresh activeslot
		inventory.AddItem(item);
		heldItem = inventory.GetItemIn(activeSlot);

		// Hide the item
		itemObject.SetActive(false);

		RefreshAnimations();
	}

    private void RefreshAnimations() {
        // Set the held item animation info on pick up ///WORKS BCS PASSING BY REFERENCE///
        heldItem = inventory.GetItemIn(activeSlot);
    }

    // Get Hurt and Start iFrames
    public void Hurt(float damage) {
        if (!isTekkai) {
            currentHealth = Mathf.Clamp(currentHealth - damage, -1, maxHealth);
            StartCoroutine(Invulnerability()); // Needs research ##########

            // Die
            if(currentHealth < 0) {
                Debug.Log("You Died!");
                Destroy(gameObject);
            }
        }
    }

    public void Heal(float healing) {
        currentHealth = Mathf.Clamp(currentHealth + healing, -1, maxHealth);
    }

    public void Knockback(Transform attacker, float kb) {
		float distance = Vector3.Distance(attacker.position, transform.position);
		float cos = (transform.position.x - attacker.position.x) / distance;
		float sin = (transform.position.y - attacker.position.y) / distance;
		Vector3 direction = new Vector3(cos, sin, 0);
        rb.AddForce(direction * kb * 20000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        // rb.AddForce(-transform.up * kb * 10000 * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void Busy(float seconds) {
        StartCoroutine(IEBusy(seconds));
    }

    private IEnumerator IEBusy (float seconds) {
        this.isBusy = true;
        yield return new WaitForSeconds(seconds);
        this.isBusy = false;
    }

    public void Tekkai (float seconds) {
        StartCoroutine(IETekkai(seconds));
    }

    private IEnumerator IETekkai (float seconds) {
        rb.mass *= 1.2f;
        isTekkai = true;

        yield return new WaitForSeconds(seconds);

        isTekkai = false;
        rb.mass /= 1.2f;
        Debug.Log("TEKKAI, GOU!!" + Time.time);
    }

	private IEnumerator Dodge() {
        Physics2D.IgnoreLayerCollision(6, 7, true);
		sprender.color = new Color(1, 1, 1, 0.5f);
		yield return new WaitForSeconds(0.6f);
		sprender.color = new Color(1, 1, 1, 1f);
        Physics2D.IgnoreLayerCollision(6, 7, false);
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

	public void AddInventoryUI (UI_Inventory ui_inventory) {
		inventory.AddUI(ui_inventory);
		this.ui_inventory = ui_inventory;
        this.ui_pockets = ui_inventory.ui_pockets;
        this.ui_abilities = ui_inventory.ui_abilities;
		ui_abilities.SetActive(false);
        ui_pockets.SetActive(false);

		activeSlot = inventory.ActivateSlot(0);
        heldItem = inventory.GetItemIn(activeSlot);
	}

    public Inventory GetInventory() {
        return this.inventory;
    }

    public List<GameObject> GetItems() {
        return this.invItems;
    }
}
