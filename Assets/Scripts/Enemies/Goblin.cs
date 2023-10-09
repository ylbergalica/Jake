using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin", menuName = "Enemy/Goblin")]
public class Goblin : ScriptableObject, IGoblin
{
	public float offset;
    public float maxHealth;
    public float speed;
    
	public float primaryDamage;
	public float primaryKnockback;

	public float throwChance;
	public float secondaryCooldown;

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
			{"throwChance", throwChance},
			{"secondaryCooldown", secondaryCooldown},
        };

        // if (primary != null) {
        //     primary.transform.localScale = new Vector3(1, 1, 1);
        // }
		// if (secondary != null) {
            // secondary.transform.localScale = new Vector3(1, 1, 1);
        // }
    }

	public Dictionary<string, float> GetStats() {
        return this.stats;
    }

    public void UsePrimary(GameObject goblin) {
		Vector3 realOffset = goblin.transform.up * offset;

        Instantiate(primary, goblin.transform.position + realOffset, goblin.transform.rotation, goblin.transform);
	}
	
	public void UseSecondary(GameObject goblin) {
		Vector3 realOffset = goblin.transform.up * offset * 2;

        Instantiate(secondary, goblin.transform.position + realOffset, goblin.transform.rotation, goblin.transform);
	}

	public GameObject[] GetMoves() {
        GameObject[] moves = {this.primary, this.secondary};

        return moves;
    }
}
