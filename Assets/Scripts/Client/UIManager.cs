using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Inspector���� InfoPopup �г��� ������ �� ����
    public GameObject infoPopup;

    public void ShowInfoPopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(true);
            Debug.Log("���: ����â ����");
        }
    }

    public void HideInfoPopup()
    {
        if (infoPopup != null)
        {
            infoPopup.SetActive(false);
            Debug.Log("���: ����â �ݱ�");
        }
    }
}