using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("数值")]
    public int currentHealth;
    public int maxHealth = 3000;
    public float invulnearableDuration; //无敌
    public float invulnearableCounter;
    public bool invulnearable;

    [Header("状态")]
    public bool isDead = false;

    //气泡
    public GameObject floatPoint;
    //使用事件
    public UnityEvent<Transform> OnTakeDamage;
    //public UnityEvent OnDie;


    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnearable)
        {
            invulnearableCounter -= Time.deltaTime;
            if(invulnearableCounter <= 0)
            {
                invulnearable = false;
                invulnearableCounter = 0f;
            }
        }
    }

    public void TakeDamage(Attack attacker)
    {
        if (invulnearable) return;
        currentHealth -= attacker.damage;
        
        float yVal = Random.Range(-.5f, .5f), xVal = Random.Range(-0.5f, 0.6f);
        Vector3 floatPos = new Vector3(transform.position.x + xVal, transform.position.y + yVal, transform.position.z);
        GameObject gb = Instantiate(floatPoint, floatPos, Quaternion.identity);
        gb.transform.GetChild(0).GetComponent<TextMesh>().text = attacker.damage.ToString();
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;
            return;
        }

        TriggerInvulnearable();
        //执行事件,加个问号代表询问列表有没有函数
        OnTakeDamage?.Invoke(attacker.transform);

    }


    //触发无敌
    private void TriggerInvulnearable()
    {
        if (!invulnearable)
        {
            invulnearable = true;
            invulnearableCounter = invulnearableDuration;
        }
    }

}
