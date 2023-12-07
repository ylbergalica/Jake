using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
	private Animator animator;
	// arraylist of items
	public List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == 8) Open();
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.layer == 8) Open();
	}

	void OnParticleCollision(GameObject collider)
	{
		if (collider.gameObject.layer == 8) Open();
	}

	void Open() {
		animator.SetBool("isOpen", true);

		Item spawnedItem;
		foreach (Item item in items)
		{
			spawnedItem = Instantiate(item, transform.position, Quaternion.identity);
			spawnedItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 30000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
		}
	}
}
