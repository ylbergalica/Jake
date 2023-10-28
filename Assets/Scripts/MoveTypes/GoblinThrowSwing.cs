using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinThrowSwing : MonoBehaviour
{
	public GameObject throwable;
	public ScriptableObject enemyReference;
	private IDummy enemyType;
	private IEnemy goblin;

	private float length;

    // Start is called before the first frame update
    void Start()
    {
		length = gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;

		enemyType = (IDummy)enemyReference;
		goblin = transform.parent.GetComponent<IEnemy>();
		goblin.Stun(length);
		StartCoroutine(Throw());
    }

	private IEnumerator Throw() {
		yield return new WaitForSeconds(length-0.01f);
		GameObject projectile = Instantiate(throwable, transform.position + transform.up * 30, transform.rotation);
	}
}
