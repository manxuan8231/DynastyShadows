using TMPro;
using UnityEngine;

public class QuestMainBacLam : MonoBehaviour
{
    public GameObject questPanel;// Panel hiển thị thông tin nhiệm vụ 
    public TextMeshProUGUI questNameText;// Tên nhiệm vụ
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerLinhCanhB;//mui ten chi duong den linh canh

 
    public GameObject iconQuest2; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerEnemy;//mui ten chi duong den linh canh

    //tham chieu
    TurnInQuest3 turnInQuest3;
    void Start()
    {
        iconQuest.SetActive(false);
        questPanel.SetActive(false);
        pointerLinhCanhB.SetActive(false);

        iconQuest2.SetActive(false);
       
        pointerEnemy.SetActive(false);
        turnInQuest3 = FindAnyObjectByType<TurnInQuest3>();
    }
    private void Update()
    {

    }

    // Bắt đầu quest
    public void StartQuestMainBacLam()
    {
      
        iconQuest.SetActive(true);// Hiện icon nhiệm vụ trên bản đồ
        questPanel.SetActive(true);// Hiện panel nhiệm vụ
        pointerLinhCanhB.SetActive(true);// Hiện mũi tên chỉ đường đến Lính Canh B
        questNameText.text = $"Nhiệm vụ đến chổ Lính Canh B";
    }

    public void StartQuestLinhCanhB()
    {

        iconQuest2.SetActive(true);// Hiện icon nhiệm vụ trên bản đồ
        iconQuest.SetActive(false);
        pointerEnemy.SetActive(true);// Hiện mũi tên chỉ đường đến Lính Canh B
        questNameText.text = $"Nhiệm vụ Tiêu diệt sinh vật ở khu đầm lầy";
    }
}
