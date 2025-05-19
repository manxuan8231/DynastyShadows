using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [System.Serializable]
    public class Quest
    {
        public string questName;
        public Transform npcLocation; // Vị trí NPC liên quan
        public bool isMainQuest;
        public bool isAccepted;
    }

    public Quest[] allQuests;

    public GameObject questPointer; // Mũi tên chỉ đường (hoặc UI waypoint)
    public Transform player;

    // Các nút UI (gắn từ Inspector)
    public Button quest1Button;
    public Button quest2Button;
    public Button quest3Button;
    public Button mainQuestButton;

    private Quest currentSelectedQuest;

    void Start()
    {
        // Gắn sự kiện click cho các nút
        quest1Button.onClick.AddListener(() => SelectQuest(0));
        quest2Button.onClick.AddListener(() => SelectQuest(1));
        quest3Button.onClick.AddListener(() => SelectQuest(2));
        mainQuestButton.onClick.AddListener(() => SelectQuest(3));

        questPointer.SetActive(false);
    }

    void Update()
    {
        if (currentSelectedQuest != null && !currentSelectedQuest.isAccepted)
        {
            Vector3 dir = currentSelectedQuest.npcLocation.position - player.position;
            dir.y = 0;

            questPointer.transform.position = player.position + dir.normalized * 2f + Vector3.up * 2f;
            questPointer.transform.LookAt(currentSelectedQuest.npcLocation.position);
        }
    }

    void SelectQuest(int index)
    {
        currentSelectedQuest = allQuests[index];
        questPointer.SetActive(true);
    }

    // Gọi hàm này khi player tới gần NPC và nhấn "Nhận nhiệm vụ"
    public void AcceptQuest()
    {
        if (currentSelectedQuest != null)
        {
            currentSelectedQuest.isAccepted = true;
            questPointer.SetActive(false);
            Debug.Log("Đã nhận nhiệm vụ: " + currentSelectedQuest.questName);
        }
    }
}
