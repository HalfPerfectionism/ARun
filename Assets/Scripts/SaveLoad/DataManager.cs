using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //����ģʽ���Կ糡�����ú���
    public static DataManager instance;

    //���б�ķ�ʽ���洢���ݵ�
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data data;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        data = new Data(); //dataû�м̳�mono 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }


    public bool RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
            return true;
        }
        return false;
    }

    public bool UnRegisterSaveData(ISaveable saveable)
    {
        if (saveableList.Contains(saveable))
        {
            saveableList.Remove(saveable);
            return true;
        }
        return false;
    }

    //�浵
    public void Save()
    {
        foreach (ISaveable item in saveableList)
        {
            item.SaveData(data);
        }

        foreach (var item in data.characterPosDict)
        {
            print(item.Key + " " + item.Value);
        }
    }

    //����
    public void Load()
    {
        foreach(var item in saveableList)
        {
            item.LoadData(data);
        }
    }

}
