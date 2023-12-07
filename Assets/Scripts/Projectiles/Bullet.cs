using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour, IProjectile
{
	public float speed = 10f;
	public float damage = 10f;
	public float knockback = 1.5f;
	public Item itemObject;

	private Rigidbody2D rb;
	private Animator animator;
	private Collider2D collider;
	private UnityEngine.Rendering.Universal.Light2D light;

	private List<string> hitObjects = new List<string>();

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
		rb.AddForce(transform.up * speed * 50, ForceMode2D.Impulse);
	}

	public void SetStats(float lifetime, float speed, float damage, float knockback) {
		// No lifetime for bullet
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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!HasHit(collision.gameObject.name)) {
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
				// Do hit idicator when enemy gets hurtd
				Vector3 contact = collision.bounds.ClosestPoint(transform.position);
				GameObject hitPrefab = (GameObject)Resources.Load("Hit/HitImpact", typeof(GameObject));
				Instantiate(hitPrefab, contact, Quaternion.identity);

				// Shake the camera
				Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f + knockback*0.005f, 5f + knockback*1.1f);
			}
		}

		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")
			|| collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Destroyable")) {
			hitObjects.Add(collision.gameObject.name);

			light.enabled = false;
			rb.velocity = Vector2.zero;
			collider.enabled = false;
			if (animator) animator.SetBool("Hit", true);
			else Destroy(gameObject);
		}
	}

	public Item GetItem() {
		return itemObject;
	}

	public void PreparePickUp() {
		return;
	}
}
