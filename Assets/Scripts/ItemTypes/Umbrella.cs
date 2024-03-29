using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Umbrella", menuName = "Item/Melee/Umbrella")]
public class Umbrella : ScriptableObject, IItem
{
	public float size;
    public float offset;
    public float swing_damage;
    public float pull_damage;
    public float thrust_damage;
    public float swing_knockback;
    public float thrust_knockback;
	public float pull_force;
    public float primary_cooldown;
    public float secondary_cooldown;
    public float tertiary_cooldown;
    public GameObject primary;
    public GameObject secondary;
    public GameObject tertiary;

    private Dictionary<string, float> stats;

    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"size", size},
            {"offset", offset},
            {"swing_damage", swing_damage},
            {"pull_damage", pull_damage},
            {"thrust_damage", thrust_damage},
            {"swing_knockback", swing_knockback},
            {"thrust_knockback", thrust_knockback},
			{"pull_force", pull_force},
            {"primary_cooldown", primary_cooldown},
            {"secondary_cooldown", secondary_cooldown},
            {"tertiary_cooldown", tertiary_cooldown},
        };

        if (primary) {
            primary.transform.localScale = new Vector3(size, size, 1);
        }
		if (secondary) {
            secondary.transform.localScale = new Vector3(size, size, 1);
        }
		if (tertiary) {
			tertiary.transform.localScale = new Vector3(size, size, 1);
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
		Vector3 realOffset = player.transform.up * offset * 2.2f;

        Instantiate(secondary, player.transform.position + realOffset, player.transform.rotation, player.transform);
	}

	public void UseTertiary(GameObject player)
	{
		Vector3 realOffset = player.transform.up * offset;

        Instantiate(tertiary, player.transform.position + realOffset, player.transform.rotation, player.transform);
		
		Camera.main.GetComponent<CameraFollow>().ShakeCamera(0.1f, 25f);
	}

    public GameObject[] GetMoves() {
        GameObject[] moves = {this.primary, this.secondary, this.tertiary};

        return moves;
    }
}
