using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



//�����ScriptableObject����һ���¼���ScriptableObject���͵��¼�
[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    //�¼���Ҫ���ķ���
    //��ʱ����������������¼�
    public UnityAction<Character> OnEventRaised;

    //˭Ҫ�����¼����ͰѲ�������ȥ
    public void RaiseEvent(Character charater)
    {
        OnEventRaised?.Invoke(charater);
    }
}
