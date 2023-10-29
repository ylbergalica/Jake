using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour, IEnemy {
	public ScriptableObject enemyReference;
	private IDummy enemyType;
	private Dictionary<string, float> stats;

    private Rigidbody2D rb;
	private Animator animator;
    private float currentHealth;
	public float bounce;
	public float bounceTime;
	private float tempSpeed;
	private Coroutine stunned;
	private float stunnedUntil;

	// Targetting
    private Collider2D[] senseArea;
	private GameObject target;
	private Vector3 senseOffset;
	private bool isBusy;
	private bool isChasing = false;
	private bool isResting;
	private bool isAttacking;

	private float secondaryChance;

	private float timeToReady;
    private float lastPrimary;
    private float primaryLength;

	public int type;
	// 1 BLUE 27C6F1 Size 1.5
	// 2 RED E0512E
	// 3 GOLD FFD400
	// 4 GREEN 3FDB37
	// 5 PURPLE C12BB7
	public float maxHealth;
	public float primaryDamage;
	public float primaryKnockback;
	private Color color;
	private float size;
	public float meleeReach;

	void Awake() {
		enemyType = (IDummy)enemyReference;
		stats = enemyType.GetStats();

		rb = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
        bounce *= 1000;
		tempSpeed = bounce;
        currentHealth = maxHealth;
		// secondaryChance = (stats["secondaryChance"]/100) * 10000;

		// primaryLength = enemyType.GetMoves()[0].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
		// secondaryLength = enemyType.GetMoves()[1].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
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
			else if (distance < meleeReach
				&& lastPrimary + stats["primaryCooldown"] + 0.1f < Time.time
				&& !isAttacking) {
				// Primary Attack if close enough
				lastPrimary = Time.time;
				enemyType.UsePrimary(gameObject);
			}	
		}
		else if (target == null) {
			isChasing = false;
		}

        // Check Sense Area for a Player
        foreach (Collider2D collider in senseArea){
            if (collider.gameObject.tag == "Player" && !isBusy) {
                // roses are red, violets are blue, your code is my code too
				if (isResting) {
					Vector3 vectorToTarget = collider.transform.position - transform.position;
					float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - stats["rotationModifier"];
					Quaternion quart = Quaternion.AngleAxis(angle, Vector3.forward);
					transform.rotation = Quaternion.Lerp(transform.rotation, quart, stats["rotationSpeed"]*0.01f);
				}

                // Chase Player by Bouncing towards them
				if (!isChasing)	StartCoroutine(Chase());
				else if (!isResting) {
					rb.AddForce(transform.up * bounce, ForceMode2D.Force);
				}
				target = collider.gameObject;

				break;
            }
        }
	}

	private void OnCollisionEnter2D(Collision2D collider) {
        // Do Damage to Player and get KB
        if(collider.gameObject.tag == "Player") {
            Player player = collider.gameObject.GetComponent<Player>();
            player.Hurt(primaryDamage);
			player.Knockback(transform, primaryKnockback/4f);

			float distance = Vector3.Distance(collider.transform.position, transform.position);
			float cos = (transform.position.x - collider.transform.position.x) / distance;
			float sin = (transform.position.y - collider.transform.position.y) / distance;
			Vector3 direction = new Vector3(cos, sin, 0);
            rb.AddForce(direction * 20000 * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }

	public IEnumerator Chase() {
		isChasing = true;
		while (isChasing) {
			isResting = true;
			yield return new WaitForSeconds(1f);
			isResting = false;
			if (rb.drag != 10) rb.drag = 10;
			yield return new WaitForSeconds(bounceTime);
		}
	}
	
	public void Attack() {
		isAttacking = true;
		animator.SetTrigger("Attack");
		StartCoroutine(IAttack());
	}

	private IEnumerator IAttack() {
		while (!isResting) {
			yield return new WaitForSeconds(0.1f);
		}
		bounce *= 2f;
		yield return new WaitForSeconds(1f + bounceTime);
		rb.drag -= 5;
		bounce /= 2f;
		isAttacking = false;
	}

    public void Hurt(float damage) {
        currentHealth = Mathf.Clamp(currentHealth - damage, -1, maxHealth);

        // Die
        if(currentHealth < 1) {
            Destroy(gameObject);
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
			bounce = tempSpeed;
		}
		stunnedUntil = Time.time + seconds;
		stunned = StartCoroutine(IEStun(seconds));
    }

    private IEnumerator IEStun(float seconds) {
        tempSpeed = bounce;
        bounce = 0;

        yield return new WaitForSeconds(seconds);
        bounce = tempSpeed;
    }
}