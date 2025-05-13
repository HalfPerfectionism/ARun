using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//保证在加载之前读取进度
[DefaultExecutionOrder(-100)]

public class DataManager : MonoBehaviour
{
    //单例模式可以跨场景调用函数
    public static DataManager instance;

    //用列表的方式来存储数据点
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data data;

    //持久化
    private string jsonFolder;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        data = new Data(); //data没有继承mono 
        jsonFolder = Application.persistentDataPath + "/SAVE_DATA/";
        print(Application.persistentDataPath);

        ReadSaveData();
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

    //存档
    public void Save()
    {
        foreach (ISaveable item in saveableList)
        {
            item.SaveData(data);
        }

        //foreach (var item in data.characterPosDict)
        //{
        //    print(item.Key + " " + item.Value);
        //}
        var resultPath = jsonFolder + "data.sav";
        //将数据转成json文件
        var jsonData = JsonConvert.SerializeObject(data);
        //创建路径
        if(!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder); //路径
        File.WriteAllText(resultPath, jsonData);

    }

    //读档
    public void Load()
    {
        ReadSaveData();
        foreach (var item in saveableList)
        {
            item.LoadData(data);
        }

    }

    private void ReadSaveData()
    {
        var resultPath = jsonFolder + "data.sav";

        if (File.Exists(resultPath))
        {
            //将json转成Class.Data
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            data = jsonData;
        }
    }

}
