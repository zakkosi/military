// �� ��ũ��Ʈ�� � ������Ʈ���� ������ �ʽ��ϴ�.
using UnityEngine.Events;
using System;

// Inspector â�� ����Ʈ ���·� �����ֱ� ���� [Serializable] �Ӽ��� �߰��մϴ�.
[Serializable]
public class CommandEventMapping
{
    public VoiceCommandSO command;
    public UnityEvent onCommandHeard;
}