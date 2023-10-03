using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private UI_Inventory ui_inventory;
    private Item[] itemList;
    private List<ItemSlot> allSlots;

    // private Item[] abilityList;
    // private ItemSlot[] abilities;
    private Dictionary<string, bool> abilities;

    private int itemCount;

    public int itemSlots;
    public ItemSlot itemSlot;

    // Start is called before the first frame update
    public Inventory(int itemSlots, UI_Inventory ui_inventory)
    {
        itemList = new Item[itemSlots];
        this.itemSlots = itemSlots;
        // abilityList = new Item[5];
        abilities = new Dictionary<string, bool> {
            {"Dodge", false}
        };

        this.ui_inventory = ui_inventory;
        this.allSlots = new List<ItemSlot>();
        ui_inventory.Setup(this);
    }

    public void AddItemSlot(ItemSlot slot){
        allSlots.Add(slot);
        // itemList.Add(slot, null);
    }

    public void AddItem(Item item){
        for(int i = 0; i <= itemList.Length; i++){
            if(itemList[i] == null) {
                itemList[i] = item;
                break;
            }
        }

        itemCount++;

        ui_inventory.RefreshInventory();
    }

    public void AddAbility(string abilityName) {
        string toChange = null;

        foreach (KeyValuePair<string, bool> ability in abilities) {
            if (ability.Key == abilityName && !ability.Value) {
                toChange = abilityName;
				break;
            }
        }

        if (toChange != null) {
            abilities[toChange] = true;
            ui_inventory.RefreshInventory();
			Debug.Log("Ability added!");
			Debug.Log(abilities[toChange]);
        }
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

    public void Swap(ItemSlot slot1, ItemSlot slot2) {
        //copyright arbnor rama
        Item item1 = GetItemIn(slot1);
        Item item2 = GetItemIn(slot2);
        Item temp = item1;

        SetItemIn(slot1, item2);
        SetItemIn(slot2, temp);
    }

    public Item GetItemIn(ItemSlot slot){
        if(itemList.Length > allSlots.IndexOf(slot)){
            return itemList[allSlots.IndexOf(slot)];
        }
        else{
            return null;
        }
    }

    public void SetItemIn(ItemSlot slot, Item item){
        itemList[allSlots.IndexOf(slot)] = item;
    }

    public Item[] GetItems(){
        return itemList;
    }

    public List<ItemSlot> GetAllSlots(){
        return allSlots;
    }

    public Dictionary<string, bool> GetAbilities() {
        return this.abilities;
    }

    public int GetItemCount() {
        return itemCount;
    }

    public void RemoveItem(Item item){
        // itemList[slot] = null;
        // var result = Array.Find(itemList, element => element == item);
        itemList[Array.IndexOf(itemList, item)] = null;
        ui_inventory.RefreshInventory();
    }
        
    override public string ToString(){
        string output = "Inventory: ";

        foreach(Item item in itemList){
            output += item + " , ";
        }

        return output;
    }
}
