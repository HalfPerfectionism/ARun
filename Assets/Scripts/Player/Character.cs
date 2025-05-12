using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
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
    public UnityEvent<Character> OnHealthChange;

    //存档
    public DataDefination dataDefination;
    public PhysicsCheck check;


    private void OnEnable()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
        //dataDefination = GetComponent<DataDefination>();
        check = GetComponent<PhysicsCheck>();
        ISaveable saveable = this;
        saveable.RegisterSaveData(); 
    }

    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
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
        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    print(dataDefination);
        //}
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
        OnHealthChange?.Invoke(this);

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

    public DataDefination GetDataID()
    {
        return GetComponent<DataDefination>(); 
   
    }

    public void SaveData(Data data)
    {
        print(GetDataID());
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
            data.characterPosDict[GetDataID().ID] = transform.position;
        else
        {
            data.characterPosDict.Add(GetDataID().ID, transform.position);
        }

    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID]; 
        }
    }
}
