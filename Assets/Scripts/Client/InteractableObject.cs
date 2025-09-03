using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // �ʱ� ���� ������ ���� ������
    private Vector3 initialScale;
    private Quaternion initialRotation;
    private bool isRotating = false;

    void Start()
    {
        // �� ���� �� �ʱ� ���¸� ����
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // isRotating�� true�� ���� ȸ��
        if (isRotating)
        {
            transform.Rotate(Vector3.up, 50f * Time.deltaTime);
        }
    }

    // --- ���� ��ɰ� ����� public �Լ��� ---

    public void ZoomIn()
    {
        transform.localScale *= 1.2f;
        Debug.Log("���: Ȯ��");
    }

    public void ZoomOut()
    {
        transform.localScale /= 1.2f;
        Debug.Log("���: ���");
    }

    public void StartRotation()
    {
        isRotating = true;
        Debug.Log("���: ȸ�� ����");
    }

    public void StopRotation()
    {
        isRotating = false;
        Debug.Log("���: ȸ�� ����");
    }

    public void ResetObject()
    {
        transform.localScale = initialScale;
        transform.rotation = initialRotation;
        isRotating = false;
        Debug.Log("���: �������");
    }
}