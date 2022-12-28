using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
    public Dictionary<string, float> GetStats(); 
    public void UsePrimary();
    public void UseSecondary();
    public void UseTertiary();
    public void Effect();
}
