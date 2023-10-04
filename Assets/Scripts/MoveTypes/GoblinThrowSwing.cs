using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinThrowSwing : MonoBehaviour
{
	public ScriptableObject enemyReference;
	private IGoblin enemyType;
	private GoblinAI goblin;

    // Start is called before the first frame update
    void Start()
    {
		enemyType = (IGoblin)enemyReference;
		goblin = transform.parent.GetComponent<GoblinAI>();
		goblin.Stun(gameObject.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }
}
