using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
	public static Inventory instance;
	private Player player;

    private UI_Inventory ui_inventory;
    private Item[] itemList;
    private List<ItemSlot> allSlots;

    private Dictionary<string, bool> abilities;

    private int itemCount;

    public int itemSlots;
    public ItemSlot itemSlot;

    // Start is called before the first frame update
    public Inventory(int itemSlots)
    {
		instance = this;

        itemList = new Item[itemSlots];
        this.itemSlots = itemSlots;
        // abilityList = new Item[5];
        abilities = new Dictionary<string, bool> {
            {"Dodge", false},
			{"Dash", false},
			{"Light", false},
			{"Silent", false},
			{"Slam", false}
        };

        this.allSlots = new List<ItemSlot>();
    }

	public void AddUI (UI_Inventory ui_inventory) {
		this.ui_inventory = ui_inventory;
        ui_inventory.Setup(this);
	}

    public void AddItemSlot(ItemSlot slot){
        allSlots.Add(slot);
        // itemList.Add(slot, null);
    }

    public int AddItem(Item item){
		int slot = -1;
        for(int i = 0; i <= itemList.Length; i++){
            if(itemList[i] == null) {
                itemList[i] = item;
				slot = i;
                break;
            }
        }
        itemCount++;

        ui_inventory.RefreshInventory();
		return slot;
    }

	public bool HasAbility(string abilityName) {
		return abilities[abilityName];
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

	public Item GetItemIn(int slotIndex) {
		if (itemList.Length > slotIndex) return itemList[slotIndex];
		else return null;
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

	public void ThrowItemIn(int index){
		// Set it back to active, set the position to the player, and push it away in the direction the player is looking
		Item item = GetItemIn(index);
		item.gameObject.SetActive(true);
		item.SetCanPickup(false);
		item.gameObject.transform.position = player.gameObject.transform.position;
		item.GetComponent<Rigidbody2D>().AddForce(player.gameObject.transform.up * 30000f * Time.fixedDeltaTime, ForceMode2D.Impulse);
		
		RemoveItemIn(index);
	}

	public void ThrowItemIn(ItemSlot slot) {
		Item item = GetItemIn(slot);
		item.gameObject.SetActive(true);
		item.SetCanPickup(false);
		item.gameObject.transform.position = player.gameObject.transform.position;
		item.GetComponent<Rigidbody2D>().AddForce(player.gameObject.transform.up * 30000f * Time.fixedDeltaTime, ForceMode2D.Impulse);
		
		RemoveItemIn(allSlots.IndexOf(slot));
	}

    public void RemoveItem(Item item){
        // itemList[slot] = null;
        // var result = Array.Find(itemList, element => element == item);
        itemList[Array.IndexOf(itemList, item)] = null;
        ui_inventory.RefreshInventory();
    }

	public void RemoveItemIn(ItemSlot slot) {
		if (itemList.Length > allSlots.IndexOf(slot)) {
			itemList[allSlots.IndexOf(slot)] = null;
			ui_inventory.RefreshInventory();
		}
	}

	public void RemoveItemIn(int slotIndex) {
		if (itemList.Length > slotIndex) {
			itemList[slotIndex] = null;
			ui_inventory.RefreshInventory();
		}
	}

	public Player GetPlayer() { return this.player; }
	public void SetPlayer(Player player) { this.player = player; }
        
    override public string ToString(){
        string output = "Inventory: ";

        foreach(Item item in itemList){
            output += item + " , ";
        }

        return output;
    }
}
