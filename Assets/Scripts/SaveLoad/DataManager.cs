using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    //单例模式可以跨场景调用函数
    public static DataManager instance;

    //用列表的方式来存储数据点
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data data;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        data = new Data(); //data没有继承mono 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Save();
        }
    }


    public void RegisterSaveData(ISaveable saveable)
    {
        if(!saveableList.Contains(saveable))
            saveableList.Add(saveable);
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    //存档
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

    //读档
    public void Load()
    {

    }

}
