using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnRaisedEvent;

    //±ä°×
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
    }

    //Í¸Ã÷
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
        //Debug.Log(123);
    }

    public void RaiseEvent(Color target, float duration, bool fadeIn)
    {
        OnRaisedEvent?.Invoke(target, duration, fadeIn);
    }
}
