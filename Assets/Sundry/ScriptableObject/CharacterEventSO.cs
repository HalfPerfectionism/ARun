using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



//把这个ScriptableObject理解成一个事件，ScriptableObject类型的事件
[CreateAssetMenu(menuName = "Event/CharacterEventSO")]
public class CharacterEventSO : ScriptableObject
{
    //事件需要订阅方法
    //暂时把这个看成真正的事件
    public UnityAction<Character> OnEventRaised;

    //谁要启动事件，就把参数传进去
    public void RaiseEvent(Character charater)
    {
        OnEventRaised?.Invoke(charater);
    }
}
