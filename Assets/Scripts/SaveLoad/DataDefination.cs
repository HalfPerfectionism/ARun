using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefination : MonoBehaviour
{
    public string ID;

    //private void OnValidate()
    //{
    //    if (ID == string.Empty)
    //        ID = System.Guid.NewGuid().ToString(); //GUID唯一ID
    //}
    private void Start()
    {
        if (ID == string.Empty)
            ID = System.Guid.NewGuid().ToString(); //GUID唯一ID
    }

}