using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Item/Ranged/Bow")]
public class Bow : ScriptableObject, IItem
{
    public float size;
    public float offset;
    public float primary_cooldown;
	public float primary_damage;
	public float primary_knockback;
	public float primary_projectile_size;
	public float primary_projectile_speed;
	public float primary_projectile_lifetime;


    public GameObject primary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"offset", offset},
            {"primary_cooldown", primary_cooldown},
			{"primary_damage", primary_damage},
			{"primary_knockback", primary_knockback},
			{"primary_projectile_lifetime", primary_projectile_lifetime},
			{"primary_projectile_speed", primary_projectile_speed},
			{"primary_projectile_size", primary_projectile_size}
		};

        if (primary != null) {
            primary.transform.localScale = new Vector3(size, size, 1);
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
