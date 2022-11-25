using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private ItemSlot slot;

    public ItemSlot itemSlot;
    public GameObject ui;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Setup(Inventory inventory){
        this.inventory = inventory;

        for(int i = 0; i < inventory.itemSlots; i++){
            slot = Instantiate(itemSlot, ui.transform);
            slot.name = "ItemSlot" + i;
            slot.GetComponent<RectTransform>().anchoredPosition += new Vector2(slot.width * i, 0);

            inventory.AddItemSlot(slot);
        }
        
    }

    public void RefreshInventory(){
        List<ItemSlot> slots = inventory.GetAllSlots();
        List<Item> items = inventory.GetItems();

        foreach(Item item in items){
            slots[items.IndexOf(item)].SetSprite(item.GetSprite());
        }
    }
}
