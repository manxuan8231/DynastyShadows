using JetBrains.Annotations;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public bool isQuest2Map2Complete = false;
    public GameObject quest1;
    public GameObject quest2;
    public GameObject quest3;
    public GameObject quest4;
    public GameObject quest5;
    public GameObject quest6;
    public GameObject quest7;
    public GameObject quest8;
    private void Start()
    {
        DataQuest dataQuest = SaveManagerMan.LoadGame().dataQuest; // Lấy dữ liệu nhiệm vụ từ SaveManager
        isQuest2Map2Complete = dataQuest.isQuestMap2; // Cập nhật trạng thái nhiệm vụ từ dữ liệu lưu
    }
    private void Update()
    {
      if (isQuest2Map2Complete)
        {
            
            quest1.SetActive(false);
            quest2.SetActive(false);
            quest3.SetActive(false);
            quest4.SetActive(false);
            quest5.SetActive(false);
            quest6.SetActive(false);
            quest7.SetActive(false);
            quest8.SetActive(false);

        }
    }
}

