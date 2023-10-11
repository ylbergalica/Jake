using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : MonoBehaviour, IEnemy {
	public ScriptableObject enemyReference;
	private IGoblin enemyType;
	private Dictionary<string, float> stats;

    private Rigidbody2D rb;
    private float currentHealth;
	private float speed;
	private Coroutine stunned;
	private float stunnedUntil;
    private bool lockRot;

	// Targetting
    private Collider2D[] senseArea;
	private GameObject target;
	private Vector3 senseOffset;
	private bool isBusy;
	private bool isCornered;

	private float timeToReady;
    private float lastPrimary;
    private float primaryLength;
    private float lastSecondary;
	private float secondaryChance;
    private float secondaryLength;

	void Awake() {
		enemyType = (IGoblin)enemyReference;
		stats = enemyType.GetStats();

		rb = gameObject.GetComponent<Rigidbody2D>();
        speed = stats["speed"] * 100;
        currentHealth = stats["maxHealth"];
		// secondaryChance = (stats["secondaryChance"]/100) * 10000;

		primaryLength = enemyType.GetMoves()[0].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
		secondaryLength = enemyType.GetMoves()[1].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
	}

	private void FixedUpdate() {
		senseOffset = transform.up * (stats["senseRadius"] / 3f);
		senseArea = Physics2D.OverlapCircleAll(transform.position + senseOffset, stats["senseRadius"]);

        // Check Sense Area for a Player
        foreach (Collider2D collider in senseArea){
            if (collider.gameObject.tag == "Player" && !isBusy && !lockRot) {
				// roses are red, violets are blue, your code is my code too
				Vector3 vectorToTarget = collider.transform.position - transform.position;
				float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - stats["rotationModifier"];
				Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
				transform.rotation = Quaternion.Slerp(transform.rotation, quart, stats["rotationSpeed"]);

				target = collider.gameObject;
            }
        }

		// Check if target is still in range
		if (target != null) {
			float distance = Vector3.Distance(target.transform.position, transform.position);

			if (distance > 600f) {
                // Chase Player
                rb.AddForce(transform.up * speed);
			}
			else if ((distance > 200f || isCornered)
				&& lastSecondary + stats["secondaryCooldown"] < Time.time
				&& timeToReady < Time.time) {
				// Shoot if far enough away
				lastSecondary = Time.time;
				timeToReady = Time.time + secondaryLength + 0.1f;
				StartCoroutine(StopToAim());
				enemyType.UseSecondary(gameObject);
			}
			else if (distance < 200f && !isCornered
				&& lastPrimary + stats["primaryCooldown"] < Time.time
				&& timeToReady < Time.time) {
				// Too close, swing and back up
				lastPrimary = Time.time;
				timeToReady = Time.time + primaryLength + 0.1f;
				StartCoroutine(BackUp());
				enemyType.UsePrimary(gameObject);
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

	private void OnCollisionStay2D(Collision2D collider) {
		if(collider.gameObject.tag == "Wall") {
			isCornered = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collider) {
		if(collider.gameObject.tag == "Wall") {
			isCornered = false;
		}
	}

	private IEnumerator StopToAim() {
		yield return new WaitForSeconds(1f);
		lockRot = true;
		yield return new WaitForSeconds(0.5f);
		lockRot = false;
	}

	public IEnumerator BackUp() {
		yield return new WaitForSeconds(0.55f);
		rb.AddForce(-transform.up * 90000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
	}

    public void Hurt(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, -1, stats["maxHealth"]);

		foreach(Transform child in transform) {
			if (child.gameObject.CompareTag("EnemyAttack")) {
				Destroy(child.gameObject);
			}
		}

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
		if (stunned != null && Time.time+seconds > stunnedUntil) {
			StopCoroutine(IEStun(seconds));
			speed = stats["speed"] * 100;
		}
		stunnedUntil = Time.time + seconds;
		stunned = StartCoroutine(IEStun(seconds));
    }

    private IEnumerator IEStun(float seconds) {
        float tempSpeed = speed;
        speed = 0;

        yield return new WaitForSeconds(seconds);
        speed = tempSpeed;
    }
}