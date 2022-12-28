using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;
    public ItemType itemType;
    public ScriptableObject itemReference;
    private IItem item;
    private float lastUse;

    // Start is called before the first frame update
    void Start()
    {
        item = (IItem)itemReference;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log(item.GetStats()["damage"]);
        }
    }

    public Dictionary<string, float> GetStats() {
        return item.GetStats();
    }

    public void UsePrimary(){
        item.UsePrimary();
    }

    public float GetLastUse() {
        return lastUse;
    }
    public void SetLastUse(float current) {
        lastUse = current;
    }
}
