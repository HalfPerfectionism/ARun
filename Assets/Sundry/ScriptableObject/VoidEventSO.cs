using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEventSO")]
public class VoidEventSO : ScriptableObject
{
    //���ڴ洢���ж����˸��¼��ķ����������ͨ�� OnEventRaised += ������; �������¼�
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        //�����¼����������ж�����OnEventRaised�ķ�����
        OnEventRaised?.Invoke();
    }
}