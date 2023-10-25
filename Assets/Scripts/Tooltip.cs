using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private Item item;
	private int slotIndex;

	private void OnMouseEnter() {
		Debug.Log("Mouse entered");

		slotIndex = GetComponent<ItemSlot>().index;
		item = Inventory.instance.GetItemIn(slotIndex);
		// TooltipManager.instance.SetAndShowTooltip(item.itemName, item.itemDescription, item.itemMetric);
	}

	private void OnMouseExit() {
		TooltipManager.instance.HideTooltip();
	}
}
