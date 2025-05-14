using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO: ScriptableObject
{
    //场景切换的事件主体
    public UnityAction<GameSceneSO, Vector3, bool> loadRequestEvent;

    //进入菜单界面的数据
    public GameSceneSO menuScene;
    public Vector3 posToGo;


    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGo, bool fadeScreen)
    {
        loadRequestEvent?.Invoke(locationToLoad, posToGo, fadeScreen);  
    }
    public void LoadMenu()
    {
        loadRequestEvent?.Invoke(menuScene, posToGo, true);
    }
}