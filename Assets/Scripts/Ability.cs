using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string abilityName;

	private void FixedUpdate() {
		transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * 1.5f) * 0.3f, transform.position.z);
	}
}
