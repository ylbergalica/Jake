using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
	public static TooltipManager instance;

	public TextMeshProUGUI itemName;
	public TextMeshProUGUI itemDescription;
	public TextMeshProUGUI itemMetric;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.LogWarning("More than one instance of TooltipManager found!");
			Destroy(this.gameObject);
		}
		else {
			instance = this;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
		gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Input.mousePosition;
    }

	public void SetAndShowTooltip(string name, string description, string metric) {
		gameObject.SetActive(true);
		Cursor.visible = false;
		transform.position = Input.mousePosition;

		itemName.text = name;
		itemDescription.text = description;
		itemMetric.text = metric;
	}

	public void HideTooltip() {
		gameObject.SetActive(false);
		Cursor.visible = true;
	}
}
