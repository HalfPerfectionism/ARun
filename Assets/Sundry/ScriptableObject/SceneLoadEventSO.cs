using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO: ScriptableObject
{
    //�����л����¼�����
    public UnityAction<GameSceneSO, Vector3, bool> loadRequestEvent;

    //����˵����������
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