using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractable
{
    public GameSceneSO sceneToGo; //  去往的场景
    public Vector3 positionToGo;//去往场景的坐标
    public SceneLoadEventSO loadEventSO; //事件SO

    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true); // 事件呼叫
    }


}
