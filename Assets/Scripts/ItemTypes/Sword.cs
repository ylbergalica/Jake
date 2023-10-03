using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Item/Melee/Sword")]
public class Sword : ScriptableObject, IItem
{
    public float size;
    public float offset;
    public float swing_damage;
    public float primary_cooldown;
    public float swing_knockback;
    public GameObject primary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"offset", offset},
            {"swing_damage", swing_damage},
            {"swing_knockback", swing_knockback},
            {"primary_cooldown", primary_cooldown}
        };

        if (primary != null) {
            primary.transform.localScale = new Vector3(size/10, size/10, 1);
        }
    }

    public Dictionary<string, float> GetStats() {
        return this.stats;
    }

	public void Effect()
	{
		throw new System.NotImplementedException();
	}

	public void UsePrimary(GameObject player)
	{
		Vector3 realOffset = player.transform.up * offset;

        Instantiate(primary, player.transform.position + realOffset, player.transform.rotation, player.transform);
	}

	public void UseSecondary(GameObject player)
	{
		throw new System.NotImplementedException();
	}

	public void UseTertiary(GameObject player)
	{
		throw new System.NotImplementedException();
	}

    public GameObject[] GetMoves() {
        GameObject[] moves = {this.primary};

        return moves;
    }
}
