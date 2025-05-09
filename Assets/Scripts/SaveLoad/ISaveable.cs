using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this);
    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this);
    void SaveData(Data data);
    void LoadData(Data data);

}
