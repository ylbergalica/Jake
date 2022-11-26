using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite sprite;

    public GameObject use;
    public float size;
    public float offset;
    public float damage;
    public float cooldown;
    public float knockback;

    private float lastUse;

    // Start is called before the first frame update
    void Start()
    {
        use.transform.localScale = new Vector3(size/10, size/10, 1);
    }

    public float GetLastUse() {
        return lastUse;
    }

    public void SetLastUse(float current) {
        lastUse = current;
    }
}
