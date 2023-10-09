using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAI : MonoBehaviour, IEnemy {
	public ScriptableObject enemyReference;
	private IGoblin enemyType;
	private Dictionary<string, float> stats;

    private Rigidbody2D rb;
    private float currentHealth;
	private float speed;

	// Targetting
    private Collider2D[] senseArea;
	private GameObject target;
	private Vector3 senseOffset;
	private bool isBusy;

	private float throwChance;

	private float timeToReady;
    private float lastPrimary;
    private float primaryLength;
    private float lastSecondary;
    private float secondaryLength;

	void Awake() {
		enemyType = (IGoblin)enemyReference;
		stats = enemyType.GetStats();

		rb = gameObject.GetComponent<Rigidbody2D>();
        speed = stats["speed"] * 100;
        currentHealth = stats["maxHealth"];
		throwChance = (stats["throwChance"]/100) * 10000;

		primaryLength = enemyType.GetMoves()[0].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
		secondaryLength = enemyType.GetMoves()[1].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
	}

	private void FixedUpdate() {
		senseOffset = transform.up * (stats["senseRadius"] / 3f);
		senseArea = Physics2D.OverlapCircleAll(transform.position + senseOffset, stats["senseRadius"]);

		// Check if target is still in range
		if (target != null) {
			float distance = Vector3.Distance(target.transform.position, transform.position);

			if (distance > stats["senseRadius"]
				|| target.GetComponent<Player>().currentHealth < 0) {
				target = null;
			}
			else if (distance < 700f && distance > 300f
				&& Random.Range(0, 100000) < throwChance
				&& lastSecondary + stats["secondaryCooldown"] + secondaryLength < Time.time
				&& timeToReady < Time.time) {
				// Secondary Attack if close enough
				lastSecondary = Time.time;
				timeToReady = Time.time + primaryLength + 0.1f;
				enemyType.UseSecondary(gameObject);
			}
			else if (distance < 120f
				&& lastPrimary + 0.1f + primaryLength < Time.time
				&& timeToReady < Time.time) {
				// Primary Attack if close enough
				lastPrimary = Time.time;
				timeToReady = Time.time + primaryLength + 0.1f;
				enemyType.UsePrimary(gameObject);
			}	
		}

        // Check Sense Area for a Player
        foreach (Collider2D collider in senseArea){
            if (collider.gameObject.tag == "Player" && !isBusy) {
                // roses are red, violets are blue, your code is my code too
                Vector3 vectorToTarget = collider.transform.position - transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - stats["rotationModifier"];
                Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, quart, stats["rotationSpeed"]);

                // Chase Player
                rb.AddForce(transform.up * speed);
				target = collider.gameObject;
            }
        }
	}

	private void OnCollisionEnter2D(Collision2D collider) {
        // Do Damage to Player and get KB
        if(collider.gameObject.tag == "Player") {
            Player player = collider.gameObject.GetComponent<Player>();
            player.Hurt(stats["primaryDamage"]);
			player.Knockback(transform, stats["primaryKnockback"]/4f);

			float distance = Vector3.Distance(collider.transform.position, transform.position);
			float cos = (transform.position.x - collider.transform.position.x) / distance;
			float sin = (transform.position.y - collider.transform.position.y) / distance;
			Vector3 direction = new Vector3(cos, sin, 0);
            rb.AddForce(direction * 20000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

    public void Hurt(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, -1, stats["maxHealth"]);

        // Die
        if(currentHealth < 1) {
            Destroy(gameObject);
        }
    }

    public void Heal(float healing) {
        currentHealth = Mathf.Clamp(currentHealth + healing, -1, stats["maxHealth"]);
    }

    public void Knockback(Transform attacker, float kb) {
		float distance = Vector3.Distance(attacker.position, transform.position);
		float cos = (transform.position.x - attacker.position.x) / distance;
		float sin = (transform.position.y - attacker.position.y) / distance;
		Vector3 direction = new Vector3(cos, sin, 0);
        rb.AddForce(direction * kb * 20000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }
	
	public void Busy (float seconds) {
		StartCoroutine(IEBusy(seconds));
	}

	private IEnumerator IEBusy(float seconds) {
		isBusy = true;
		yield return new WaitForSeconds(seconds);
		isBusy = false;
	}

    public void Stun (float seconds) {
        StartCoroutine(IEStun(seconds));
    }

    private IEnumerator IEStun(float seconds) {
        float tempSpeed = speed;
        speed = 0;

        yield return new WaitForSeconds(seconds);
        speed = tempSpeed;
    }
}