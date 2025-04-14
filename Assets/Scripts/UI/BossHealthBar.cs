using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image bossHealthFill;
    public Text bossName;
    public EnemyHealth eh;

    void Start()
    {
        bossName.text = eh.name;
    }

    // Update is called once per frame
    void Update()
    {
        bossHealthFill.fillAmount = (float)eh.currentHealth / (float)eh.maxHealth;
    }
}
