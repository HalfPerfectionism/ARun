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
    //第一个场景，的信息
    public GameSceneSO firstLoadScene;
    public GameSceneSO menuScene;

    public Vector3 firstPlayerPos;  
    public Vector3 menuPlayerPos;
    

    public SceneLoadEventSO sceneLoadEventSO;
    public FadeEventSO fadeEvent;
    public VoidEventSO newGameEvent;

    //下个场景
    private GameSceneSO locationToGo;
    private Vector3 posToGo;
    private bool fadeScreen;
    private float fadeTime; //淡入淡出的时间

    public Transform playerTrans; // 修改角色坐标
    public GameObject playerUI;
    private bool isLoading; //是否正在加载
    public GameSceneSO currentLoadScene;

    [Header("广播")]
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

    //第一个场景
    private void NewGame()
    {
        locationToGo = firstLoadScene;
        sceneLoadEventSO.RaiseLoadRequestEvent(locationToGo, firstPlayerPos, true);
    }

    //卸载场景
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
        playerTrans.gameObject.SetActive(false); // 关闭人物

        LoadNewScene();
    }

    //加载场景
    private void LoadNewScene()
    {
        //确保场景已经加载好
        var loadingOption =  locationToGo.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOption.Completed += OnLoadedCompleted;
    }

    //当场景已经加载好后
    private void OnLoadedCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = locationToGo;
        playerTrans.position = posToGo; // 该角色位置
        playerTrans.gameObject.SetActive(true);

        if (fadeScreen)
        {
            //TODO:
            fadeEvent.FadeOut(0.5f); 
        }
        //场景加载完后执行的事件
        afterLoadedScene.RaiseEvent();
        OnSceneLoaded?.Invoke();

        if (currentLoadScene != menuScene)
            playerUI.gameObject.SetActive(true);
         
        isLoading = false;
    }

}
