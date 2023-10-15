using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{ 
    public void SetStats(float lifetime, float speed, float damage, float knockback);
	public void AddHitObject(string objectName);
	public Item GetItem();
	public void PreparePickUp();
}
