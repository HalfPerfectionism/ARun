using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //get允许外部代码读取该属性的值, private set只允许当前类内部修改该属性的值，外部代码无法修改
    public static PlayerHealth instance;
    //{ get; private set; }

    public static int currentHealth;
    public static int maxHealth = 3000;
    public ParticleSystem hitPs; 


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        currentHealth = maxHealth;
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //Destroy(gameObject);
        }
    }


    void Update()
    {
    }
}
