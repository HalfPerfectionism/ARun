using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStatBar playerStatBar;
    
    [Header("�¼�����")]
    public CharacterEventSO healthEvent; //ͨ������¼����ɹ��ĸ�UIManager��������Ϣ

    //ע���¼������¼���Ӻ���
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
    }

    //ע���¼�
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
