using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public string ID;
    //public PersistentType persistentType;

    //private void OnValidate()
    //{
    //    if(persistentType == PersistentType.RW)
    //        if (ID == string.Empty)
    //            ID = System.Guid.NewGuid().ToString(); //GUID唯一ID
    //    else
    //        ID = string.Empty;
    //}
    private void OnValidate()
    {
        if (ID == string.Empty)
            ID = System.Guid.NewGuid().ToString(); //GUID唯一ID
    }
}