// VoiceUIController.cs
using UnityEngine;
using TMPro; // TextMeshPro�� ����ϱ� ���� �߰�
using System.Collections.Generic;

public class VoiceUIController : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject historyContent; // ��ũ�� ���� Content ������Ʈ
    public GameObject historyEntryPrefab; // �����丮 �׸����� ����� Text ������
    public TextMeshProUGUI statusText; // ���� ���¸� ǥ���� Text

    private List<string> commandHistory = new List<string>();

    void Start()
    {
        // ������ �� ���� �ؽ�Ʈ�� �ʱ�ȭ
        SetStatusIdle();
    }

    // "��� �ֽ��ϴ�..." ���·� ����
    public void SetStatusListening()
    {
        statusText.text = "��� �ֽ��ϴ�...";
    }

    // "���� �ν� ��..." ���·� ����
    public void SetStatusProcessing()
    {
        statusText.text = "���� �ν� ��...";
    }

    // ��� ���·� ���� (�ؽ�Ʈ �����)
    public void SetStatusIdle()
    {
        statusText.text = "";
    }

    // ���ο� �����丮 �׸� �߰�
    public void AddToHistory(string newText)
    {
        if (historyContent == null || historyEntryPrefab == null) return;

        // 1. �����丮 ��Ͽ� �ؽ�Ʈ �߰�
        commandHistory.Add(newText);

        // 2. �������� ����� ���ο� UI Text ������Ʈ ����
        GameObject newEntry = Instantiate(historyEntryPrefab, historyContent.transform);

        // 3. ������ ������Ʈ�� �ؽ�Ʈ�� ����
        var textComponent = newEntry.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = "> " + newText;
        }

        // (����) �ʹ� ���� �����丮�� ���̸� ���� ������ �ͺ��� ����� ���� �߰� ����
    }
}