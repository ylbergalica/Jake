using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Player player;
    public Image totalHealthBar;
    public Image currentHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        totalHealthBar.fillAmount = player.maxHealth / 600;
        // Debug.Log(player.maxHealth);
        // Debug.Log(totalHealthBar.fillAmount);
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthBar.fillAmount = player.currentHealth / 600;
    }
}
