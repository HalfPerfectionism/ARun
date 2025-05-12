using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public interface ISaveable 
{
    DataDefination GetDataID();
    bool RegisterSaveData() { return DataManager.instance.RegisterSaveData(this); }
    bool UnRegisterSaveData() { return DataManager.instance.UnRegisterSaveData(this); } 
    void SaveData(Data data);
    void LoadData(Data data);

}
