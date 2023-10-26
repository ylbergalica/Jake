using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shovel", menuName = "Item/Melee/Shovel")]
public class Shovel : ScriptableObject, IItem
{
	public float size;
    public float swing_offset;
    public float swing_damage;
    public float swing_knockback;
    public float primary_cooldown;
    public float thrust_offset;
    public float thrust_damage;
    public float thrust_knockback;
	public float thrust_multiplier;
    public float secondary_cooldown;
    public GameObject primary;
    public GameObject secondary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
			{"swing_offset", swing_offset},
			{"thrust_offset", thrust_offset},
            {"swing_damage", swing_damage},
            {"thrust_damage", thrust_damage},
            {"swing_knockback", swing_knockback},
            {"thrust_knockback", thrust_knockback},
			{"thrust_multiplier", thrust_multiplier},
            {"primary_cooldown", primary_cooldown},
            {"secondary_cooldown", secondary_cooldown},
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
		Vector3 realOffset = player.transform.up * swing_offset;

        Instantiate(primary, player.transform.position + realOffset, player.transform.rotation, player.transform);
	}

	public void UseSecondary(GameObject player)
	{
		Vector3 realOffset = player.transform.up * thrust_offset;

        GameObject secondaryInst = Instantiate(secondary, player.transform.position + realOffset, player.transform.rotation, player.transform);

        player.GetComponent<Player>().Knockback(secondaryInst.transform, thrust_knockback * thrust_multiplier);
		Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f, 20f);
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
