using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Item/Ranged/Projectile")]
public class Projectile : ScriptableObject, IItem
{
    public float size;
    public float damage;
	public float speed;
	public float lifetime;
	public float knockback;
	public float primary_cooldown;
    public GameObject throwable;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"damage", damage},
			{"speed", speed},
			{"lifetime", lifetime},
			{"knockback", knockback},
			{"primary_cooldown", primary_cooldown}
        };
    }

    public Dictionary<string, float> GetStats() {
        return stats;
    }

	public void Effect()
	{
		throw new System.NotImplementedException();
	}

	public void UsePrimary(GameObject player)
	{
		GameObject projectile = Instantiate(throwable, player.transform.position + player.transform.up * 20, player.transform.rotation);
		IProjectile projectileRef = projectile.GetComponent(typeof(IProjectile)) as IProjectile;
		projectileRef.SetStats(lifetime, speed, damage, knockback);
		projectile.gameObject.layer = 8;
		// projectileRef.AddHitObject(player.gameObject.name);
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
        GameObject[] moves = {this.throwable};

        return moves;
    }
}
