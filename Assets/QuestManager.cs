using JetBrains.Annotations;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

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
        DataQuestSingleTon.isQuestMap2 = dataQuest.isQuestMap2; // Cập nhật trạng thái nhiệm vụ trong singleton
    }
    private void Update()
    {
        if(DataQuestSingleTon.isQuestMap2)
        {
            quest1.SetActive(false);
            quest2.SetActive(false);
            quest3.SetActive(false);
            quest4.SetActive(false);
            quest5.SetActive(false);
            quest6.SetActive(false);
            quest7.SetActive(false);
            quest8.SetActive(false); // Kích hoạt nhiệm vụ 8 khi nhiệm vụ 2 hoàn thành
        }
      }
}
