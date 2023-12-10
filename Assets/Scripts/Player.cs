using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
	private UnityEngine.Rendering.Universal.Light2D shadow;
	private Vector3 scale;

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
	public float rotationSpeed;

    // Stats
    [Header ("Stats")]
    public float speed;
    public float maxHealth;
    public float currentHealth;
	public float grabRadius;
	private Collider2D[] grabArea;
	private float grabMultiplier;
	private ArrayList grabTags;
	private Dictionary<string, GameObject> indicators;

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
	private bool isWalking;
	private bool rightFoot = true;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        rb = gameObject.GetComponent<Rigidbody2D>();
		shadow = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
		scale = transform.localScale;
        speed *= 100;
        currentHealth = maxHealth;
        sprender = gameObject.GetComponent<SpriteRenderer>();
		grabTags = new ArrayList();
		indicators = new Dictionary<string, GameObject>();

        inventory = new Inventory(slotsCount);
        invItems = new List<GameObject>();
		if (ui_inventory != null) {
			AddInventoryUI(ui_inventory);
		}

        dodgeLength = dodgeAnim.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

        isTekkai = false;
        Physics2D.IgnoreLayerCollision(8, 9, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// Rotate Player Towards Mouse
        mousePos = Input.mousePosition;
        mousePos.z = 0;
        objectPos = Camera.main.WorldToScreenPoint(transform.position);

		// roses are red, violets are blue, your code is my code too
		Vector3 vectorToTarget = mousePos - objectPos;
		float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
		Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Lerp(transform.rotation, quart, rotationSpeed*0.01f);

		// Movement on equal 8 directions
		if (Input.GetKey("w") && Input.GetKey("a")) {
			rb.AddForce(new Vector3(-speed / 1.42f, speed / 1.42f, 0));
		}
		else if (Input.GetKey("w") && Input.GetKey("d")) {
			rb.AddForce(new Vector3(speed / 1.42f, speed / 1.42f, 0));
		}
		else if (Input.GetKey("s") && Input.GetKey("a")) {
			rb.AddForce(new Vector3(-speed / 1.42f, -speed / 1.42f, 0));
		}
		else if (Input.GetKey("s") && Input.GetKey("d")) {
			rb.AddForce(new Vector3(speed / 1.42f, -speed / 1.42f, 0));
		}
		else if (Input.GetKey("w")) {
			rb.AddForce(new Vector3(0, speed, 0));
		}
		else if (Input.GetKey("a")) {
			rb.AddForce(new Vector3(-speed, 0, 0));
		}
		else if (Input.GetKey("s")) {
			rb.AddForce(new Vector3(0, -speed, 0));
		}
		else if (Input.GetKey("d")) {
			rb.AddForce(new Vector3(speed, 0, 0));
		}

		if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
			if (!isWalking) StartCoroutine(Walking());
		}
		else isWalking = false;
		
		// Bobbing while walking
		if (isWalking && rightFoot) {
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(scale.x + 2f, scale.y - 2f, 1), 0.1f);
		}
		else if (isWalking && !rightFoot) {
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(scale.x - 2f, scale.y + 2f, 1), 0.1f);
		}
		else if (!isWalking) {
			gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(scale.x, scale.y, 1), 0.1f);
		}

		// Pick up Items
		grabArea = Physics2D.OverlapCircleAll(transform.position, grabRadius);
		GameObject closestItem = null;
		grabTags.Clear();
		foreach (Collider2D collider in grabArea) {
			if (collider.gameObject.CompareTag("Item")) {
				grabTags.Add(collider.gameObject.name);
				// Instantiate indicator and add to dictionary if it is not already there
				if (!indicators.ContainsKey(collider.gameObject.name)) {
					GameObject indicator = Instantiate((GameObject)Resources.Load("PickUp/IndicatePickUp", typeof(GameObject)), collider.transform.position, Quaternion.identity);
					indicator.transform.parent = collider.transform;
					indicators.Add(collider.gameObject.name, indicator);
				}

				// Find the closest Item
				if (closestItem == null) closestItem = collider.gameObject;
				
				float distance = Vector3.Distance(transform.position, collider.transform.position);
				if (distance < Vector3.Distance(transform.position, closestItem.transform.position)) {
					closestItem = collider.gameObject;
				}
			}
		}
		// Clear indicator dictionary
		if (indicators.Count > 0) {
			foreach (KeyValuePair<string, GameObject> entry in indicators) {
				if (!grabTags.Contains(entry.Key)) {
					Destroy(entry.Value);
					indicators.Remove(entry.Key);
				}
			}
		}

		if (Input.GetKey("e") && grabArea.Length > 0) {
			if (closestItem) {
				// Grab Item
				try {
					IProjectile projectileRef = closestItem.gameObject.GetComponent(typeof (IProjectile)) as IProjectile;
					projectileRef.PreparePickUp();
				}
				finally {
					Rigidbody2D itemBody = closestItem.gameObject.GetComponent<Rigidbody2D>();
					Vector3 direction = transform.position - closestItem.transform.position;
					float distance = Vector3.Distance(transform.position, closestItem.transform.position);
					itemBody.AddForce((direction * (Mathf.Pow(distance, 1.5f)/5f)), ForceMode2D.Force);
					closestItem.gameObject.GetComponent<Item>().SetCanPickup(true);
					
					// Destroy indicator and remove from dictionary
					Destroy(indicators[closestItem.name]);
					indicators.Remove(closestItem.name);
				}
			}
		}
    }

    void Update(){
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
        if(Input.GetKeyDown(KeyCode.Escape)){
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

		// Throw Away Item in Active Slot when pressing G
		if (Input.GetKeyDown(KeyCode.G)) {
			if (heldItem != null) {
				inventory.RemoveItemIn(activeSlot);
				// Set it back to active, set the position to the player, and push it away in the direction the player is looking
				heldItem.gameObject.SetActive(true);
				heldItem.SetCanPickup(false);
				heldItem.transform.position = transform.position;
				heldItem.GetComponent<Rigidbody2D>().AddForce(transform.up * 30000f * Time.fixedDeltaTime, ForceMode2D.Impulse);
				heldItem = null;
			}
		}

        if (!isBusy) {
            // Use Item in Active Slot
            if(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.time) && heldItem.isMoveReady(3, Time.time)){
                    heldItem.SetLastUse(3, Time.time);

                    heldItem.UseTertiary(gameObject);
                }
            }
            else if(Input.GetMouseButtonDown(0)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.time) && heldItem.isMoveReady(1, Time.time)){
                    heldItem.UsePrimary(gameObject);

					if (heldItem.itemType == ItemType.Throwable) {
						heldItem.SetLastUse(1, 0);
						// inventory.RemoveItemIn(activeSlot);
						// Destroy(heldItem.gameObject);
					} 
					else heldItem.SetLastUse(1, Time.time);
                }
            }
            else if(Input.GetMouseButtonDown(1)){
                ui_inventory.RefreshInventory();
                RefreshAnimations();
                
                if(heldItem != null && heldItem.isItemReady(Time.time) && heldItem.isMoveReady(2, Time.time)){
                    heldItem.SetLastUse(2, Time.time);

                    heldItem.UseSecondary(gameObject);
                }
            }
            
            // Dodging
            if(inventory.HasAbility("Dodge") 
				&& Input.GetKeyDown(KeyCode.Space) 
				&& Time.time > dodgeLast + dodgeCooldown + dodgeLength) {
				// Dodge
                Instantiate(dodgeAnim, transform.position, transform.rotation, transform);
                dodgeLast = Time.time;
                Busy(dodgeLength);
				StartCoroutine(Dodge());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider) {
        // Pick up Items
		bool canPickup = false;
		try {
			IProjectile projectile = collider.gameObject.GetComponent(typeof(IProjectile)) as IProjectile;
			canPickup = projectile.CanPickup();
		}
		catch {canPickup = collider.gameObject.GetComponent<Item>().CanPickup();}

        if(collider.gameObject.CompareTag("Item") && canPickup) { // GET ITEM
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
		if (item == null) {
			IProjectile projectileRef = itemObject.GetComponent(typeof (IProjectile)) as IProjectile;
			item = projectileRef.GetItem();
			Debug.Log(item.itemName);
		}

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
			// Shake the camera for .1s+.1% of damage, strength 60+70% of damage
			Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f + damage*0.001f, 60f + damage*0.7f);

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

	public void SetRotationSpeed(float value) {
		this.rotationSpeed = value;
	}

	public void SetRotationSpeed(float value, float duration) {
		StartCoroutine(RotationConstraint(value, duration));
	}

	public void ResetRotationSpeed() {
		this.rotationSpeed = 80f;
	}

	public IEnumerator RotationConstraint(float value, float duration) {
		this.rotationSpeed = value;
		yield return new WaitForSeconds(duration);
		this.rotationSpeed = 80f;
		yield break;
	}

	public IEnumerator Walking() {
		isWalking = true;
		while (isWalking) {
			yield return new WaitForSeconds(0.2f);
			rightFoot = !rightFoot;
		}

		yield break;
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
			shadow.color = new Color(1, 1, 1);
            sprender.color = new Color(0, 0, 0, 0f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2)); // Needs research ##########
			shadow.color = new Color(0, 0, 0);
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

	public ItemSlot GetActiveSlot() {
		return this.activeSlot;
	}

    public Inventory GetInventory() {
        return this.inventory;
    }

    public List<GameObject> GetItems() {
        return this.invItems;
    }
}
