using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IProjectile
{
	public float speed = 10f;
	public float lifetime = 5f;
	public float damage = 10f;
	public float knockback = 1.5f;
	public Item itemObject;

	private Rigidbody2D rb;
	private bool isPicked = false;
	private IEnumerator impendingDeath;

	private List<string> hitObjects = new List<string>();

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.AddForce(transform.up * speed * 50, ForceMode2D.Impulse);
		impendingDeath = DestroyAfterTime(lifetime);
		StartCoroutine(impendingDeath);
	}

	private void FixedUpdate()
	{
		if (rb.velocity.magnitude < 150f) SetCanPickup(true);
		else if (!isPicked) SetCanPickup(false);
	}

	public void SetStats(float lifetime, float speed, float damage, float knockback) {
		this.lifetime = lifetime;
		this.speed = speed;
		this.damage = damage;
		this.knockback = knockback;
	}

	private bool HasHit(string objectName) {
		if (hitObjects.Contains(objectName)) {
			return true;
		}
		return false;
	}

	public void AddHitObject(string objectName) {
		hitObjects.Add(objectName);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if ((collision.relativeVelocity.magnitude > 500 || rb.velocity.magnitude > 200)
			&& !HasHit(collision.gameObject.name)) {
			if (collision.gameObject.CompareTag("Enemy"))
			{
				IEnemy enemy = collision.gameObject.GetComponent(typeof (IEnemy)) as IEnemy;
				if (enemy != null)
				{
					enemy.Hurt(damage);
					enemy.Knockback(transform, knockback / 1.5f);
				}
			}
			else if (collision.gameObject.CompareTag("Player"))
			{
				Player player = collision.gameObject.GetComponent<Player>();
				if (player != null)
				{
					player.Hurt(damage);
					player.Knockback(transform, knockback);

				}
			}

			if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) {
				// Do hit idicator when enemy gets hurt
				// Vector3 contact = collision.bounds.ClosestPoint(transform.position);
				Vector3 contact = collision.GetContact(0).point;
				GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
				Instantiate(hitPrefab, contact, Quaternion.identity);
			}
		}
		
		hitObjects.Add(collision.gameObject.name);
	}
	
	public void SetCanPickup(bool isPickup) {	
		if (isPickup) gameObject.tag = "Item";
		else gameObject.tag = "Projectile";
	}

	public Item GetItem() {
		return itemObject;
	}

	public void PreparePickUp() {
		isPicked = true;
		gameObject.tag = "Item";
		GetComponent<Collider2D>().isTrigger = true;
		StopCoroutine(impendingDeath);
	}

	public IEnumerator DestroyAfterTime(float time) {
		yield return new WaitForSeconds(time);
		Debug.Log("Destroying rock");
		Destroy(gameObject);
	}
}
