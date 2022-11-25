using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public float width;
    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetSprite(Sprite sprite) {
        this.sprite = sprite;

        GameObject icon = transform.Find("Icon").gameObject;
        icon.GetComponent<Image>().sprite = sprite;

        var tempColor = icon.GetComponent<Image>().color;
        tempColor.a = 1f;
        icon.GetComponent<Image>().color = tempColor;
    }
}
