using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slime", menuName = "Enemy/Slime")]
public class Slime : ScriptableObject, IDummy
{
    public float speed = 40f;
    
	public float primaryCooldown;

    // Rotation Variables
    public float rotationSpeed;
    public float rotationModifier;
    public float senseRadius = 500f;

	private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
			{"speed", speed},
			{"rotationSpeed", rotationSpeed},
			{"rotationModifier", rotationModifier},
			{"senseRadius", senseRadius},
			{"primaryCooldown", primaryCooldown}
        };
    }

	public Dictionary<string, float> GetStats() {
        return this.stats;
    }

    public void UsePrimary(GameObject slime) {
		// Vector3 realOffset = slime.transform.up * primaryOffset;

        // Instantiate(primary, slime.transform.position + realOffset, slime.transform.rotation, slime.transform);
		Debug.Log("JUMP!");
	}
	
	public void UseSecondary(GameObject slime) {
		return;
	}

	public GameObject[] GetMoves() {
        GameObject[] moves = {};

        return moves;
    }
}
