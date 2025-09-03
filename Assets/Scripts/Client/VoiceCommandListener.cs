// VoiceCommandListener.cs
using UnityEngine;
using UnityEngine.Events;

public class VoiceCommandListener : MonoBehaviour
{
    [Tooltip("�� �����ʰ� �� ����� ��ɾ� ����")]
    public VoiceCommandSO commandToListenFor;

    [Tooltip("��ɾ ����� ��, �� ���ӿ�����Ʈ���� ������ ��ɵ�")]
    public UnityEvent onCommandHeard;




    // ������Ʈ�� Ȱ��ȭ�� �� '���' ����
    private void OnEnable()
    {
        if (commandToListenFor != null)
        {
            commandToListenFor.onCommandRecognized += OnCommandHeard;
        }
    }

    // ������Ʈ�� ��Ȱ��ȭ�� �� '���' ����
    private void OnDisable()
    {
        if (commandToListenFor != null)
        {
            commandToListenFor.onCommandRecognized -= OnCommandHeard;
        }
    }

    // ��ȣ�� ����� �� ����� �Լ�
    private void OnCommandHeard()
    {
        // Inspector�� ����� UnityEvent�� ����
        onCommandHeard.Invoke();
    }
}