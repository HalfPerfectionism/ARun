using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//��֤�ڼ���֮ǰ��ȡ����
[DefaultExecutionOrder(-100)]

public class DataManager : MonoBehaviour
{
    //����ģʽ���Կ糡�����ú���
    public static DataManager instance;

    //���б�ķ�ʽ���洢���ݵ�
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data data;

    //�־û�
    private string jsonFolder;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        data = new Data(); //dataû�м̳�mono 
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

    //�浵
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
        //������ת��json�ļ�
        var jsonData = JsonConvert.SerializeObject(data);
        //����·��
        if(!File.Exists(resultPath))
            Directory.CreateDirectory(jsonFolder); //·��
        File.WriteAllText(resultPath, jsonData);

    }

    //����
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
            //��jsonת��Class.Data
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);
            data = jsonData;
        }
    }

}
