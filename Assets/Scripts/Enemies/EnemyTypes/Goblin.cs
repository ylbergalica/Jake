using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin", menuName = "Enemy/Goblin")]
public class Goblin : ScriptableObject, IDummy
{
    public float maxHealth;
    public float speed;
    
	public float primaryDamage;
	public float primaryKnockback;
	public float primaryCooldown;
	public float primaryOffset;

	public float secondaryChance;
	public float secondaryDamage;
	public float secondaryCooldown;
	public float secondaryOffset;

    // Rotation Variables
    public float rotationSpeed;
    public float rotationModifier;
    public float senseRadius;

    public GameObject primary;
    public GameObject secondary;

	private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"maxHealth", maxHealth},
			{"speed", speed},
			{"rotationSpeed", rotationSpeed},
			{"rotationModifier", rotationModifier},
			{"senseRadius", senseRadius},
			{"primaryDamage", primaryDamage},
			{"primaryKnockback", primaryKnockback},
			{"primaryCooldown", primaryCooldown},
			{"secondaryChance", secondaryChance},
			{"secondaryDamage", secondaryDamage},
			{"secondaryCooldown", secondaryCooldown},
        };
    }

	public Dictionary<string, float> GetStats() {
        return this.stats;
    }

    public void UsePrimary(GameObject goblin) {
		Vector3 realOffset = goblin.transform.up * primaryOffset;

        Instantiate(primary, goblin.transform.position + realOffset, goblin.transform.rotation, goblin.transform);
	}
	
	public void UseSecondary(GameObject goblin) {
		Vector3 realOffset = goblin.transform.up * secondaryOffset;

        Instantiate(secondary, goblin.transform.position + realOffset, goblin.transform.rotation, goblin.transform);
	}

	public GameObject[] GetMoves() {
        GameObject[] moves = {this.primary, this.secondary};

        return moves;
    }
}
