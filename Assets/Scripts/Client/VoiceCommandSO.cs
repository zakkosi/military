// VoiceCommandSO.cs
using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "New Voice Command", menuName = "Voice/Command")]
public class VoiceCommandSO : ScriptableObject
{
    [Tooltip("���߿� AI�� ����� Ű���� ���")]
    public List<string> keywords = new List<string>();


    public event Action onCommandRecognized;

    public void Invoke()
    {
        Debug.Log(this.name + " �̺�Ʈ�� ȣ��Ǿ����ϴ�!");
        onCommandRecognized?.Invoke();
    }
}