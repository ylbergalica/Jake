using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetFollow;
	private Vector3 targetPos;
	
	private bool shaking = false;
	private float magnitude = 0f;

	float xChange;
	float yChange;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

		xChange = Random.Range(-1f, 1f) * magnitude;
		yChange = Random.Range(-1f, 1f) * magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetFollow != null && !shaking){
			targetPos = targetFollow.transform.position;

			transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, 10), 0.2f);
        }
		else if (shaking) {
			targetPos = targetFollow.transform.position;
			xChange = Random.Range(-1f, 1f) * magnitude;
			yChange = Random.Range(-1f, 1f) * magnitude;

			transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x + xChange, targetPos.y + yChange, 10), 0.2f);
		}
    }

	public void ShakeCamera(float duration, float magnitude) {
		if (magnitude >= this.magnitude) { // Only shake if the magnitude is greater than the current magnitude
			StartCoroutine(Shake(duration, magnitude));
		}
	}

	IEnumerator Shake(float duration, float magnitude) {
		shaking = true;
		this.magnitude = magnitude;
		yield return new WaitForSeconds(duration);
		this.magnitude = 0.1f;
		shaking = false;
	}
}
