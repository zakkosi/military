// �� ��ũ��Ʈ�� TargetObject�� �� �� ���� �ٿ��ݴϴ�.
using UnityEngine;
using System.Collections.Generic;

public class ObjectCommandRouter : MonoBehaviour
{
    // ������ ���� ������ ���ڸ� ����Ʈ(���) ���·� �����ϴ�.
    public List<CommandEventMapping> commandMappings;

    private void OnEnable()
    {
        // ����Ʈ�� �ִ� ��� '���-���' ¦�� ���� �����ʸ� ����մϴ�.
        foreach (var mapping in commandMappings)
        {
            if (mapping.command != null)
            {
                // ���(SO)�� �̺�Ʈ�� �߻��ϸ�, �� mapping�� ����� UnityEvent�� �����ϵ��� ���
                mapping.command.onCommandRecognized += mapping.onCommandHeard.Invoke;
            }
        }
    }

    private void OnDisable()
    {
        // �����ʸ� �����ϰ� �����մϴ�.
        foreach (var mapping in commandMappings)
        {
            if (mapping.command != null)
            {
                mapping.command.onCommandRecognized -= mapping.onCommandHeard.Invoke;
            }
        }
    }
}