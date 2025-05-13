using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


public class Data
{
    //用字典的方式记录物体位置
    public Dictionary<string, SerlializeVector3> characterPosDict = new Dictionary<string, SerlializeVector3>();
    //用这一个字典来存储所有的浮点数据
    public Dictionary<string, float> floatSaveData = new Dictionary<string, float>();

    public string sceneToSave;
    //工厂模式
    public void SaveGameScene(GameSceneSO gameSceneSO)
    {
        sceneToSave = JsonUtility.ToJson(gameSceneSO);
        Debug.Log(sceneToSave);
    }

    public GameSceneSO GetSaveScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }

}

public class SerlializeVector3
{
    public float x, y, z;

    public SerlializeVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);   
    }
}