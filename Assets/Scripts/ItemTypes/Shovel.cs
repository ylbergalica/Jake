using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shovel", menuName = "Item/Melee/Shovel")]
public class Shovel : ScriptableObject, IItem
{
	public float size;
    public float offset;
    public float swing_damage;
    public float thrust_damage;
    public float swing_knockback;
    public float thrust_knockback;
    public float primary_cooldown;
    public float secondary_cooldown;
    public GameObject primary;
    public GameObject secondary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"offset", offset},
            {"swing_damage", swing_damage},
            {"thrust_damage", thrust_damage},
            {"swing_knockback", swing_knockback},
            {"thrust_knockback", thrust_knockback},
            {"primary_cooldown", primary_cooldown},
            {"secondary_cooldown", secondary_cooldown},
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
		Vector3 realOffset = player.transform.up * offset;

        GameObject secondaryInst = Instantiate(secondary, player.transform.position + realOffset, player.transform.rotation, player.transform);

        player.GetComponent<Player>().Knockback(secondaryInst.transform, thrust_knockback * 2);
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
