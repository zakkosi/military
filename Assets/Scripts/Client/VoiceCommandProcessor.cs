// VoiceCommandProcessor.cs
using System.Collections.Generic;
using System.Linq;
using Unity.InferenceEngine;
using UnityEngine;

public class VoiceCommandProcessor : MonoBehaviour
{
    // [����] �׽�Ʈ�� ��ɾ� ���µ��� Inspector â���� �������ִ� �κ��Դϴ�.
    public VoiceCommandSO zoomIn;
    public VoiceCommandSO zoomOut;
    public VoiceCommandSO startRotation;
    public VoiceCommandSO stopRotation;
    public VoiceCommandSO showPopup;
    public VoiceCommandSO HidePopupCommand;
    public VoiceCommandSO reset;

    [Header("AI ����")]
    [Range(0, 1)]
    [Tooltip("�� �� �̻��� ���絵�� ������ ������� �����մϴ�.")]
    public float similarityThreshold = 0.8f;
    public VoiceUIController uiController;

    // [�߰�] ��� ��ɾ� ������ �����ϱ� ���ϰ� ����Ʈ�� �����ϴ�.
    private List<VoiceCommandSO> allCommands;

    // [�߰�] �̸� ���� Ű���� �Ӻ����� ������ ��ųʸ�
    // Key: ��ɾ� SO, Value: �ش� ��ɾ��� Ű���� �Ӻ��� ����Ʈ
    private Dictionary<VoiceCommandSO, List<float[]>> commandEmbeddings;

    void Start()
    {
        // [�߰�] �� ���� ��, ��� ��ɾ �м��Ͽ� �Ӻ����� �̸� ����� �Ӵϴ�.
        InitializeCommands();
    }

    /*
    void Update()
    {
        // ���� 1 Ű�� ������ Ȯ�� ���(zoomIn)�� Invoke() �Լ��� ȣ���մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Ű���� [1] �Է� -> 'Ȯ��' ��� �׽�Ʈ");
            zoomIn?.Invoke(); // null�� �ƴ� ���� ����
        }

        // ���� 2 Ű�� ������ ��� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Ű���� [2] �Է� -> '���' ��� �׽�Ʈ");
            zoomOut?.Invoke();
        }

        // ���� 3 Ű�� ������ ȸ�� ���� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Ű���� [3] �Է� -> 'ȸ�� ����' ��� �׽�Ʈ");
            startRotation?.Invoke();
        }

        // ���� 4 Ű�� ������ ȸ�� ���� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Ű���� [4] �Է� -> 'ȸ�� ����' ��� �׽�Ʈ");
            stopRotation?.Invoke();
        }

        // ���� 5 Ű�� ������ �˾� ���� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Ű���� [5] �Է� -> '�˾� ����' ��� �׽�Ʈ");
            showPopup?.Invoke();
        }

        // ���� 6 Ű�� ������ �˾� ���� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Ű���� [6] �Է� -> '�˾� ����' ��� �׽�Ʈ");
            HidePopupCommand?.Invoke();
        }

        // ���� 0 Ű�� ������ ���� ����� �׽�Ʈ�մϴ�.
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Ű���� [0] �Է� -> '�������' ��� �׽�Ʈ");
            reset?.Invoke();
        }
    }
    */
    public void ProcessTextCommand(string recognizedText)
    {
        Debug.Log("���� �ν� ��� ����: " + recognizedText);
        uiController?.AddToHistory(recognizedText);

        // 1. E5 �𵨷� ���� ���� �ؽ�Ʈ�� �Ӻ����� �����ɴϴ�.
        float[] recognizedTextEmbedding = E5Model.Instance.GetEmbedding(recognizedText);
        if (recognizedTextEmbedding == null) return;

        // 2. ���� ������ ��ɾ ã�� ���� ���� �ʱ�ȭ
        VoiceCommandSO bestMatchCommand = null;
        float highestSimilarity = 0f;

        // 3. �̸� ����� �� ��� ��ɾ��� �Ӻ����� �ϳ��� ��
        foreach (var commandEntry in commandEmbeddings)
        {
            VoiceCommandSO command = commandEntry.Key;
            List<float[]> keywordEmbeddings = commandEntry.Value;

            foreach (var keywordEmbedding in keywordEmbeddings)
            {
                // �ڻ��� ���絵(����) ���
                float similarity = CalculateSimilarity(recognizedTextEmbedding, keywordEmbedding);

                // ��������� �ְ� �������� ������, �� ����� �ְ� �ĺ��� ����
                if (similarity > highestSimilarity)
                {
                    highestSimilarity = similarity;
                    bestMatchCommand = command;
                }
            }
        }

        // 4. ã�� �ְ� ������ ������ �Ӱ谪���� ������ Ȯ��
        if (highestSimilarity >= similarityThreshold)
        {
            Debug.Log($"<color=green>��� ����: [{bestMatchCommand.name}] (���絵: {highestSimilarity:F2})</color>");
            bestMatchCommand.Invoke(); // ���� ����� ��ɾ� ����!
        }
        else
        {
            Debug.Log($"<color=yellow>������ ����� ã�� ���߽��ϴ�. (�ְ� ���絵: {highestSimilarity:F2})</color>");
        }
    }

    /// <summary>
    /// �� ���� �� �� ���� ȣ��Ǿ� ��� Ű���带 �̸� ���ͷ� ��ȯ�� �Ӵϴ�.
    /// </summary>
    private void InitializeCommands()
    {
        allCommands = new List<VoiceCommandSO>
        {
            zoomIn, zoomOut, startRotation, stopRotation, showPopup, HidePopupCommand, reset
        };

        commandEmbeddings = new Dictionary<VoiceCommandSO, List<float[]>>();
        Debug.Log("��ɾ� �Ӻ��� �ʱ�ȭ�� �����մϴ�...");

        foreach (var command in allCommands)
        {
            if (command == null) continue;

            var keywordEmbeddings = new List<float[]>();
            foreach (var keyword in command.keywords)
            {
                // E5Model�� ����� �� Ű������ �Ӻ����� ���
                var embedding = E5Model.Instance.GetEmbedding(keyword);
                if (embedding != null)
                {
                    keywordEmbeddings.Add(embedding);
                }
            }
            commandEmbeddings[command] = keywordEmbeddings;
        }
        Debug.Log("��ɾ� �Ӻ��� �Ϸ�!");
    }

    /// <summary>
    /// �� ������ �ڻ��� ���絵(����)�� ����մϴ�.
    /// </summary>
    private float CalculateSimilarity(float[] vecA, float[] vecB)
    {
        if (vecA == null || vecB == null || vecA.Length != vecB.Length)
        {
            return 0;
        }

        // E5 �Ӻ����� �̹� ����ȭ(Normalized)�Ǿ� �����Ƿ�, ������ �� �ڻ��� ���絵�Դϴ�.
        return Enumerable.Range(0, vecA.Length).Sum(i => vecA[i] * vecB[i]);
    }
}