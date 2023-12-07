using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	private Animator animator;
	
	// grabbing area
	private bool isClosing = false;
	private Collider2D[] grabArea;
	public float grabRadius = 130f;

	// arraylist of items
	public List<Item> startingItems = new List<Item>();
	private List<Item> items = new List<Item>();

    void Start()
    {
        animator = GetComponent<Animator>();
		
		Item spawnedItem;
		foreach (Item item in startingItems) {
			spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
			items.Add(spawnedItem);
			spawnedItem.gameObject.SetActive(false);
		}
    }

	void FixedUpdate()
	{
		if (isClosing) {
			foreach (Collider2D collider in grabArea) {
				if (collider.gameObject.CompareTag("Item")) {
					Rigidbody2D itemBody = collider.gameObject.GetComponent<Rigidbody2D>();
					Vector2 direction = transform.position - collider.transform.position;
					itemBody.AddForce(direction * 100, ForceMode2D.Force);
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.layer == 8) Open();
	}

	void OnCollisionEnter2D(Collision2D collider) {
		if (collider.gameObject.layer == 8) Open();
	}

	void OnParticleCollision(GameObject collider)
	{
		if (collider.gameObject.layer == 8) Open();
	}
	
	void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Item") && isClosing) {
			// store the item in the chest as prefab Item type then destroy the game object
			items.Add(collider.gameObject.GetComponent<Item>());
			collider.gameObject.SetActive(false);
		}
	}

	void Open() {
		if (animator.GetBool("isOpen")) return;
		animator.SetBool("isOpen", true);

		foreach (Item item in items) {
			item.gameObject.SetActive(true);
			item.transform.position = transform.position;
			item.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 30000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}

		items.Clear();
		StartCoroutine(Close());
	}

	IEnumerator Close() {
		yield return new WaitForSeconds(10f);
		grabArea = Physics2D.OverlapCircleAll(transform.position, grabRadius);
		isClosing = true;

		yield return new WaitForSeconds(.3f);
		animator.SetBool("isOpen", false);
		isClosing = false;
	}
}
