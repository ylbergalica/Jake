using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public ItemCategory itemCategory;
    public Sprite sprite;
    public ItemType itemType;
    public ScriptableObject itemReference;
    private IItem item;
    private float lastPrimary;
    private float primaryLength;
    private float lastSecondary;
    private float secondaryLength;
    private float lastTertiary;
    private float tertiaryLength;
    private float lastEffect;

	private float randomTimeFactor;

    // Start is called before the first frame update
    void Start()
    {
        item = (IItem)itemReference;
		randomTimeFactor = Random.Range(0f, 2.5f);

        try {
            primaryLength = item.GetMoves()[0].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        }
        catch (System.Exception) { primaryLength = 0f; }
        try {
            secondaryLength = item.GetMoves()[1].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        }
        catch (System.Exception) { secondaryLength = 0f; }
        try {
            tertiaryLength = item.GetMoves()[2].GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length;
        }
        catch (System.Exception) { tertiaryLength = 0f; }
    }

	void FixedUpdate()	{
		//Bob up and down
		float bobbingValue = Mathf.Sin((Time.time + randomTimeFactor) * 2.5f) * 0.4f;
		transform.position = new Vector3(transform.position.x, transform.position.y + bobbingValue, transform.position.z);
	}

    public Dictionary<string, float> GetStats() {
		item = (IItem)itemReference;
        return item.GetStats();
    }

    public void UsePrimary(GameObject player){
        item.UsePrimary(player);

		if (itemType == ItemType.Throwable) {
			Inventory inventory = player.GetComponent<Player>().GetInventory();
			inventory.RemoveItem(this);
			Destroy(gameObject);
		}
    }

    public void UseSecondary(GameObject player) {
        item.UseSecondary(player);

        if(itemCategory == ItemCategory.Consumable) {
            Inventory inventory = player.GetComponent<Player>().GetInventory();
            inventory.RemoveItem(this);
            Destroy(gameObject);
        }
    }
    
    public void UseTertiary(GameObject player){
        item.UseTertiary(player);
    }
    
    public bool isItemReady(float current) {
        // Time.time > heldItem.GetLastUse(1) + heldItem.GetStats()["primary_cooldown"]){
                // heldItem.SetLastUse(1, Time.time);
        for (int i=1; i < 5; i++) {
            if (current < GetLastUse(i)){
                return false;
            }
        }
        
        return true;
    }

    public bool isMoveReady(int move, float current) {
        switch(move) {
            case 1:
                if (current > GetLastUse(move) + GetStats()["primary_cooldown"]){
                    return true;
                }
                break;
            case 2:
                if (current > GetLastUse(move) + GetStats()["secondary_cooldown"]){
                    return true;
                }
                break;
            case 3:
                if (current > GetLastUse(move) + GetStats()["tertiary_cooldown"]){
                    return true;
                }
                break;
            case 4:
                if (current > GetLastUse(move) + GetStats()["effect_cooldown"]){
                    return true;
                }
                break;
            default:
                throw new System.Exception();
        }

        return false;
    }

    public float GetLastUse(int move) {
        switch(move) {
            case 1:
                return lastPrimary;
            case 2:
                return lastSecondary;
            case 3:
                return lastTertiary;
            case 4:
                return lastEffect;
            default:
                throw new System.Exception();
        }
    }
    public void SetLastUse(int move, float current) {
        switch(move) {
            case 1:
                lastPrimary = current + primaryLength;
                break;
            case 2:
                lastSecondary = current + secondaryLength;
                break;
            case 3:
                lastTertiary = current + tertiaryLength;
                break;
            case 4:
                lastEffect = current;
                break;
        }
    }
}
