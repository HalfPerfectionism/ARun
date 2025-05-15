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

    //�����ⲿ���ؽ���
    public VoidEventSO LoadGameEvent;

    //��ȡ��ǰ������Ϣ����������������
    public SceneLoad sceneLoad;
    public GameSceneSO sceneToGo;

    private void OnEnable()
    {
        LoadGameEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        LoadGameEvent.OnEventRaised -= Load;
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);


        data = new Data(); //dataû�м̳�mono 
        jsonFolder = Application.persistentDataPath + "/SAVE_DATA/";

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

        data.SaveGameScene(sceneLoad.currentLoadScene);

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
        sceneToGo = data.GetSaveScene();

        StartCoroutine(LoadDataAndScene());


    }

    IEnumerator LoadDataAndScene()
    {
        bool isLoaded = false;

        // ʹ�� AddListener �����¼�
        sceneLoad.OnSceneLoaded.AddListener(() =>
        {
            isLoaded = true;
        });

        // ������������
        sceneLoad.OnLoadRequestEvent(sceneToGo, transform.position, true);

        // �ȴ�ֱ�������������
        yield return new WaitWhile(() => !isLoaded);

        // �������������Ҫ�������ظ����ĵ��µ��ڴ�й©��
        sceneLoad.OnSceneLoaded.RemoveListener(() =>
        {
            isLoaded = true;
        });

        // ��������
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
