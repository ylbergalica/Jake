using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;
    public ItemType itemType;
    public ScriptableObject itemReference;
    private IItem item;

    // Start is called before the first frame update
    void Start()
    {
        item = (IItem)itemReference;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            Debug.Log(item.GetStats()["damage"]);
        }
    }
}
