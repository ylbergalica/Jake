using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public Dictionary<string, float> GetStats(); 
    public void UsePrimary(GameObject player);
    public void UseSecondary(GameObject player);
    public void UseTertiary(GameObject player);
    public void Effect();

    public GameObject[] GetMoves();
}
