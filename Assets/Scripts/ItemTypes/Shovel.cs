using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shovel", menuName = "Item/Melee/Shovel")]
public class Shovel : ScriptableObject, IItem
{
	public float size;
    public float offset;
    public float damage;
    public float cooldown;
    public float knockback;
    public GameObject primary;
    public GameObject secondary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"offset", offset},
            {"damage", damage},
            {"knockback", knockback},
            {"cooldown", cooldown}
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
		Vector3 realOffset = player.transform.right * offset;

        Instantiate(primary, player.transform.position + realOffset, player.transform.rotation, player.transform);
	}

	public void UseSecondary(GameObject player)
	{
		Vector3 realOffset = player.transform.right * offset;

        Instantiate(secondary, player.transform.position + realOffset, player.transform.rotation, player.transform);

        player.GetComponent<Player>().Knockback(knockback * 4);
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
