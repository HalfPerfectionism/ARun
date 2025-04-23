using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    //用于存储所有订阅了该事件的方法。你可以通过 OnEventRaised += 方法名; 来订阅事件
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        //触发事件，调用所有订阅了OnEventRaised的方法。
        OnEventRaised?.Invoke();
    }
}