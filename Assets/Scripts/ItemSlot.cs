using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[HideInInspector]public bool isHovered;
	public int index;
    public float width;
    public float offset;
    public Sprite sprite;

	private Item item;

    public void SetSprite(Sprite sprite) {
        this.sprite = sprite;

        GameObject icon = transform.Find("Icon").gameObject;
        icon.GetComponent<Image>().sprite = sprite;
        
        var tempColor = icon.GetComponent<Image>().color;

        if(sprite != null) {
            tempColor.a = 1f;
        }
        else{
            tempColor.a = 0f;
        }

        icon.GetComponent<Image>().color = tempColor;
    }

	// When pressing 'G', throw item and remove from inventory
	void Update() {
		if (Input.GetKeyDown(KeyCode.G) && isHovered) {
			Inventory.instance.ThrowItemIn(index);
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (transform.parent.name == "UI_Abilities") {
			return;
		}
		
		item = Inventory.instance.GetItemIn(index);
		TooltipManager.instance.SetAndShowTooltip(item.itemName, item.itemDescription, this);

		Debug.Log("Hovering over " + item.itemName + " at index " + index);
		this.isHovered = true;
	}

	public void OnPointerExit(PointerEventData eventData) {
		TooltipManager.instance.HideTooltip();

		this.isHovered = false;
	}
}
