using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private UI_Inventory ui_inventory;
    private List<Item> itemList;
    private List<ItemSlot> allSlots;

    private int itemCount;

    public int itemSlots;
    public ItemSlot itemSlot;

    // Start is called before the first frame update
    public Inventory(int itemSlots, UI_Inventory ui_inventory)
    {
        itemList = new List<Item>();
        this.itemSlots = itemSlots;
        this.ui_inventory = ui_inventory;
        this.allSlots = new List<ItemSlot>();
        ui_inventory.Setup(this);
    }

    public void AddItemSlot(ItemSlot slot){
        allSlots.Add(slot);
        // itemList.Add(slot, null);
    }

    public void AddItem(Item item){
        for(int i = 0; i <= itemCount; i++){
            if(itemList.Count < itemSlots) {
                itemList.Add(item);
                break;
            }
        }

        itemCount++;

        ui_inventory.RefreshInventory();
    }

    public ItemSlot ActivateSlot(int index){
        ItemSlot activeSlot = this.GetAllSlots()[index];

        foreach(ItemSlot slot in this.GetAllSlots()){
            if(slot == activeSlot){
                activeSlot.transform.Find("Active").gameObject.SetActive(true);
            }
            else{
                slot.transform.Find("Active").gameObject.SetActive(false);
            }
        }

        return activeSlot;
    }

    public Item GetItemIn(ItemSlot slot){
        if(itemList.Count > allSlots.IndexOf(slot)){
            return itemList[allSlots.IndexOf(slot)];
        }
        else{
            return null;
        }
    }

    public List<Item> GetItems(){
        return itemList;
    }

    public List<ItemSlot> GetAllSlots(){
        return allSlots;
    }

    public int GetItemCount() {
        return itemCount;
    }

    public void RemoveItem(ItemSlot slot){
        // itemList[slot] = null;
    }

    
    override public string ToString(){
        string output = "Inventory: ";

        foreach(Item item in itemList){
            output += item + " , ";
        }

        return output;
    }
}
