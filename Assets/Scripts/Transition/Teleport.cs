using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour, IInteractable
{
    public GameSceneSO sceneToGo; //  ȥ���ĳ���
    public Vector3 positionToGo;//ȥ������������
    public SceneLoadEventSO loadEventSO; //�¼�SO

    public void TriggerAction()
    {
        loadEventSO.RaiseLoadRequestEvent(sceneToGo, positionToGo, true); // �¼�����
    }


}
