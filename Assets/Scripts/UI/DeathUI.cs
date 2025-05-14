using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    //¿ªÆôUI
    public void OpenDeathUI()
    {
        gameObject.SetActive(true);
    }
    //¹Ø±ÕUI
    public void CloseDeathUI()
    {
        gameObject.SetActive(false);
    }
    
}
