using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    
    [Header("事件监听")]
    public CharacterEventSO healthEvent; //通过这个事件，成功的给UIManager传递了信息

    //注册事件，给事件添加函数
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    //注销事件
    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
    }

    private void OnHealthEvent(Character character)
    {
         var persentage =  (float)character.currentHealth / character.maxHealth;
        playerStatBar.OnHealthChange(persentage);
    }

   


}
