using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    //��һ������������Ϣ
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;

    public Vector3 firstPlayerPos;  
    public Vector3 menuPlayerPos;
    

    public SceneLoadEventSO sceneLoadEventSO;
    public FadeEventSO fadeEvent;
    public VoidEventSO newGameEvent;

    //�¸�����
    private GameSceneSO locationToGo;
    private Vector3 posToGo;
    private bool fadeScreen;
    private float fadeTime; //���뵭����ʱ��

    public Transform playerTrans; // �޸Ľ�ɫ����
    public GameObject playerUI;
    private bool isLoading; //�Ƿ����ڼ���
    public GameSceneSO currentLoadScene;

    [Header("�㲥")]
    public VoidEventSO afterLoadedScene;
    public UnityEvent OnSceneLoaded;

    private void Start()
    {

        sceneLoadEventSO.RaiseLoadRequestEvent(menuScene, menuPlayerPos, true);
        //NewGame();

    }

    private void OnEnable()
    {
        sceneLoadEventSO.loadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        //sceneLoadEventSO.RaiseLoadRequestEvent(menuScene, menuPlayerPos, true);
    }

    private void OnDisable()
    {
        sceneLoadEventSO.loadRequestEvent -= OnLoadRequestEvent; 
        newGameEvent.OnEventRaised -= NewGame;

    }

    public void OnLoadRequestEvent(GameSceneSO _locationToGo, Vector3 _posToGo, bool _fadeScreen)
    {
        if (isLoading) return;
        isLoading = true; 

        locationToGo = _locationToGo;
        posToGo = _posToGo;
        fadeScreen = _fadeScreen;

        StartCoroutine(UnloadPreviousScene());
    }

    //��һ������
    private void NewGame()
    {
        locationToGo = firstLoadScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(locationToGo, firstPlayerPos, true);
    }

    //ж�س���
    private IEnumerator UnloadPreviousScene()
    {
        playerUI.gameObject.SetActive(false);
        if (fadeScreen)
        {
            fadeEvent.FadeIn(0.5f);
            // Todd
        }

        yield return new WaitForSeconds(fadeTime);

        yield return currentLoadScene?.sceneReference.UnLoadScene();
        playerTrans.gameObject.SetActive(false); // �ر�����

        LoadNewScene();
    }

    //���س���
    private void LoadNewScene()
    {
        //ȷ�������Ѿ����غ�
        var loadingOption =  locationToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadedCompleted;
    }

    //�������Ѿ����غú�
    private void OnLoadedCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = locationToGo;
        playerTrans.position = posToGo; // �ý�ɫλ��
        playerTrans.gameObject.SetActive(true);

        if (fadeScreen)
        {
            //TODO:
            fadeEvent.FadeOut(0.5f); 
        }
        //�����������ִ�е��¼�
        afterLoadedScene.RaiseEvent();
        OnSceneLoaded?.Invoke();

        if (currentLoadScene != menuScene)
            playerUI.gameObject.SetActive(true);
         
        isLoading = false;
    }

}
