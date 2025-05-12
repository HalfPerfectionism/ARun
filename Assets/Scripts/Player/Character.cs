using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour, ISaveable
{
    [Header("��ֵ")]
    public int currentHealth;
    public int maxHealth = 3000;
    public float invulnearableDuration; //�޵�
    public float invulnearableCounter;
    public bool invulnearable;

    [Header("״̬")]
    public bool isDead = false;

    //����
    public GameObject floatPoint;
    //ʹ���¼�
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent<Character> OnHealthChange;

    //�浵
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
        //ִ���¼�,�Ӹ��ʺŴ���ѯ���б���û�к���
        OnTakeDamage?.Invoke(attacker.transform);
        OnHealthChange?.Invoke(this);

    }


    //�����޵�
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
