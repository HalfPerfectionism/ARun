using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner;
    public CinemachineImpulseSource impulseSource;
    public VoidEventSO cameraShakeEvent;

    [Header("�����¼�")]
    public VoidEventSO afterLoadedScene;

    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    //private void Start()
    //{
    //    GetCameraBounds();
    //}

    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCameraShakeEvent;
        afterLoadedScene.OnEventRaised += OnAfterLoadedScene;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCameraShakeEvent;
        afterLoadedScene.OnEventRaised -= OnAfterLoadedScene;
    }

    private void OnAfterLoadedScene()
    {
        GetCameraBounds();
    }

    private void OnCameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    //�������Զ���ȡ����
    private void GetCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj != null)
        {
            confiner.m_BoundingShape2D = obj.GetComponent<Collider2D>();

            confiner.InvalidateCache(); //�建��
        }
    }
}
