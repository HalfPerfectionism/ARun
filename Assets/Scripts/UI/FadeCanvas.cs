using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;
    [Header("事件监听")]
    public FadeEventSO fadeEvent;

    private void OnEnable()
    {
        fadeEvent.OnRaisedEvent += OnFadeEvent;
    }

    private void OnDisable()
    {
        fadeEvent.OnRaisedEvent -= OnFadeEvent;
    }

    private void OnFadeEvent(Color target, float duration, bool fadeIn)
    {
        //用这个函数
        fadeImage.DOColor(target, duration);
    }
}
