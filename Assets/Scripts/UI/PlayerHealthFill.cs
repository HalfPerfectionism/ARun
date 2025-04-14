using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthFill : MonoBehaviour
{
    private Image playerHealthFill;
    public Text text;

    void Start()
    {
        playerHealthFill = GetComponent<Image>();
    }


    void Update()
    {
        playerHealthFill.fillAmount = (float)PlayerHealth.currentHealth / (float)PlayerHealth.maxHealth;
        text.text = PlayerHealth.currentHealth.ToString() + " / " +
            PlayerHealth.maxHealth.ToString();
    }
}
