using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Goblin", menuName = "Enemy/Goblin")]
public class Goblin : ScriptableObject, IGoblin
{
    public float maxHealth;
    public float speed;
    public float damage;

    // Rotation Variables
    public float rotationSpeed;
    public float rotationModifier;
    public float senseRadius;

	private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"maxHealth", maxHealth},
			{"speed", speed},
			{"damage", damage},
			{"rotationSpeed", rotationSpeed},
			{"rotationModifier", rotationModifier},
			{"senseRadius", senseRadius}
        };

        // if (primary != null) {
        //     primary.transform.localScale = new Vector3(size/10, size/10, 1);
        // }
    }

	public Dictionary<string, float> GetStats() {
        return this.stats;
    }

    public void Primary() {

	}
	
	public void Secondary() {

	}
}
