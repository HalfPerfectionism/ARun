using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    //����UI
    public void OpenDeathUI()
    {
        gameObject.SetActive(true);
    }
    //�ر�UI
    public void CloseDeathUI()
    {
        gameObject.SetActive(false);
    }
    
}
