using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		// Start from transparent sprite
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Fade in
		if (GetComponent<SpriteRenderer>().color.a < 0.7f) {
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, GetComponent<SpriteRenderer>().color.a + 0.05f);
		}
    }
}
