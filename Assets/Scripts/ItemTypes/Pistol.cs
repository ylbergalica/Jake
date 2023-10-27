using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pistol", menuName = "Item/Ranged/Pistol")]
public class Pistol : ScriptableObject, IItem
{
    public float size;
    public float offset;
    public float primary_cooldown;
	public float primary_damage;
	public float primary_knockback;
	public float primary_projectile_size;
	public float primary_projectile_speed;
	public float primary_projectile_lifetime;
    public float secondary_cooldown;
	public float secondary_damage;
	public float secondary_knockback;
	public float secondary_projectile_size;
	public float secondary_projectile_speed;
	public float secondary_projectile_lifetime;

    public GameObject primary;
	public GameObject secondary;

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
			{"primary_projectile_size", primary_projectile_size},
			{"secondary_cooldown", secondary_cooldown},
			{"secondary_damage", secondary_damage},
			{"secondary_knockback", secondary_knockback},
			{"secondary_projectile_lifetime", secondary_projectile_lifetime},
			{"secondary_projectile_speed", secondary_projectile_speed},
			{"secondary_projectile_size", secondary_projectile_size}
		};

        if (primary != null) {
            primary.transform.localScale = new Vector3(size, size, 1);
		}
		if (secondary != null) {
			secondary.transform.localScale = new Vector3(size, size, 1);
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
		Vector3 realOffset = player.transform.up * offset;

		Instantiate(secondary, player.transform.position + realOffset, player.transform.rotation, player.transform);
	}

	public void UseTertiary(GameObject player)
	{
		throw new System.NotImplementedException();
	}

    public GameObject[] GetMoves() {
        GameObject[] moves = {this.primary, this.secondary};

        return moves;
    }
}
