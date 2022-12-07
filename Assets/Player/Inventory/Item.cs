using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public string itemType;
    public Sprite sprite;

    private GameObject player;

    [Header ("MELEE")]
    public GameObject primary;
    public GameObject alt;
    public GameObject tertiary;
    public float size;
    public float offset;
    public float damage;
    public float cooldown;
    public float knockback;

    [Header ("CONSUMALBE")]
    public float healing;

    private float lastUse;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        if (primary != null) {
            primary.transform.localScale = new Vector3(size/10, size/10, 1);
        }
    }

    public void UsePrimary(){
        if(itemType == "Melee") {
            Vector3 realOffset = player.transform.right * offset;

            Instantiate(primary, player.transform.position + realOffset, player.transform.rotation, player.transform);
        }
        else if(itemType == "Consumable") {
            player.GetComponent<Player>().Heal(healing);

            Inventory inventory = player.GetComponent<Player>().GetInventory();
            inventory.RemoveItem(this);
            // Destroy(gameObject);
        }
    }

    public float GetLastUse() {
        return lastUse;
    }

    public void SetLastUse(float current) {
        lastUse = current;
    }
}
