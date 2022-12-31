using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Item/Consumable/Food")]
public class Food : ScriptableObject, IItem
{
    public float healing;
    public float cooldown;

    private Dictionary<string, float> stats;
    private void OnEnable() {
        stats = new Dictionary<string, float> {
            {"healing", healing},
            {"cooldown", cooldown},
        };
    }

	public void Effect()
	{
		throw new System.NotImplementedException();
	}

	public Dictionary<string, float> GetStats()
	{
		return this.stats;
	}

	public void UsePrimary(GameObject player)
	{
		throw new System.NotImplementedException();
	}

	public void UseSecondary(GameObject player)
	{
		player.GetComponent<Player>().Heal(healing);
	}

	public void UseTertiary(GameObject player)
	{
		throw new System.NotImplementedException();
	}

	public GameObject[] GetMoves()
	{
		throw new System.NotImplementedException();
	}
}
