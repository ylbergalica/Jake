using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private ItemSlot slot;

    public ItemSlot itemSlot;
    public GameObject ui_hotBar;
    public GameObject ui_pockets;
    public GameObject ui_abilities;

    private ItemSlot initSlot;
    private ItemSlot targetSlot;
    private GameObject indicateSwap;
    private int stage;

    public void Setup(Inventory inventory){
        this.inventory = inventory;
        this.stage = 0;

        for(int i = 0; i < 2; i++){
            slot = Instantiate(itemSlot, ui_hotBar.transform);
            slot.name = "ItemSlot" + i;
            slot.GetComponent<RectTransform>().anchoredPosition += new Vector2(slot.width * i + slot.offset * i, 0);

            // add event listener to button
            slot.transform.Find("Button").GetComponent<Button>().onClick.AddListener(HandleSwap);

            inventory.AddItemSlot(slot);
        }

        for(int i = 2; i < inventory.itemSlots; i++){
            slot = Instantiate(itemSlot, ui_pockets.transform);
            slot.name = "ItemSlot" + i;

            // add event listener to button
            slot.transform.Find("Button").GetComponent<Button>().onClick.AddListener(HandleSwap);

            inventory.AddItemSlot(slot);
        }
    }

    public void HandleSwap(){
        GameObject temp = EventSystem.current.currentSelectedGameObject;

        if(stage == 0 && ui_pockets.activeSelf){
            ItemSlot initSlotTemp = temp.transform.parent.GetComponent<ItemSlot>();
            initSlot = initSlotTemp;
            
            indicateSwap = temp.transform.parent.Find("SwapIndicator").gameObject;
            indicateSwap.SetActive(true);

            stage = 1;
        }
        else if(stage == 1 && ui_pockets.activeSelf) {
            ItemSlot targetSlotTemp = temp.transform.parent.GetComponent<ItemSlot>();
            targetSlot = targetSlotTemp;

            inventory.Swap(initSlot, targetSlot);
            RefreshInventory();
            
            ResetStage();
        }
    }

    public void ResetStage(){
        stage = 0;
        if (indicateSwap != null) {
            indicateSwap.SetActive(false);
        }
    }

    public void RefreshInventory(){
        List<ItemSlot> slots = inventory.GetAllSlots();
        Item[] items = inventory.GetItems();

        foreach(ItemSlot slot in slots){
            if(inventory.GetItemIn(slot) != null){
                slot.SetSprite(items[slots.IndexOf(slot)].sprite);
            }
            else{
                slot.SetSprite(null);
            }
        }

        Dictionary<string, bool> abilities = inventory.GetAbilities();

        foreach (KeyValuePair<string, bool> ability in abilities) {
            if (ability.Value) {
                Transform abilitySlot = ui_abilities.transform.Find(ability.Key + "Slot");
                // Debug.Log(abilitySlot.name);
                Transform slotIcon = abilitySlot.transform.Find("Icon");
                // Debug.Log(slotIcon.gameObject.activeSelf);
                slotIcon.gameObject.SetActive(true);
            }
        }
    }
}
