using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {
	public void Hurt(float damage);
	public void Knockback(Transform attacker, float kb);
	public void Stun(float time);
}