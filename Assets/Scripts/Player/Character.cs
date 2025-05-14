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

    [Header("身份")]
    public CharacterType characterType;

    //气泡
    public GameObject floatPoint;
    //使用事件
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent OnDeath;
    public VoidEventSO ReBirthEvent;


    private void OnEnable()
    {
        currentHealth = maxHealth;
        OnHealthChange?.Invoke(this);
        ISaveable saveable = this;
        saveable.RegisterSaveData();
        ReBirthEvent.OnEventRaised += ReBirth;
    }


    private void OnDisable()
    {
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
        ReBirthEvent.OnEventRaised -= ReBirth;
        OnHealthChange?.Invoke(this);
    }
    //重生
    public void ReBirth()
    {
        isDead = false;
        gameObject.SetActive(true);
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
            OnDeath?.Invoke();
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
        //print(GetDataID());
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            data.characterPosDict[GetDataID().ID] = new SerlializeVector3(transform.position);
            if (characterType == CharacterType.Enemey)
            {
                print("save: " + transform.position);
            }
            data.floatSaveData[GetDataID().ID + "health"] = this.currentHealth;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID, new SerlializeVector3(transform.position));
            data.floatSaveData.Add(GetDataID().ID + "health", this.currentHealth);
        }
        

    }

    public void LoadData(Data data)
    {
        if (data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            currentHealth = (int)data.floatSaveData[GetDataID().ID + "health"];
            transform.position = data.characterPosDict[GetDataID().ID].ToVector3();
            if (characterType == CharacterType.Enemey)
            {
                print("load: " + transform.position);
            }

            //更新UI
            OnHealthChange?.Invoke(this);
        }
    }
}
