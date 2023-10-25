using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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

	public void OnPointerEnter(PointerEventData eventData) {
		if (transform.parent.name == "UI_Abilities") {
			return;
		}
		
		item = Inventory.instance.GetItemIn(index);
		string metricType = "Damage";
		if (item.itemCategory == ItemCategory.Consumable) {
			metricType = "Healing";
		}

		string metric = $"{metricType}: {item.itemMetric}";

		TooltipManager.instance.SetAndShowTooltip(item.itemName, item.itemDescription, metric);
	}

	public void OnPointerExit(PointerEventData eventData) {
		TooltipManager.instance.HideTooltip();
	}
}
