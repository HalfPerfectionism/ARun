using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //get�����ⲿ�����ȡ�����Ե�ֵ, private setֻ����ǰ���ڲ��޸ĸ����Ե�ֵ���ⲿ�����޷��޸�
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
