using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthFill : MonoBehaviour
{
    private Image playerHealthFill;
    public Text text;
    public Character player;

    void Start()
    {
        playerHealthFill = GetComponent<Image>();
    }


    void Update()
    {
        playerHealthFill.fillAmount = (float)player.currentHealth / (float)player.maxHealth;
        text.text = player.currentHealth.ToString() + " / " +
            player.maxHealth.ToString();
    }
}
