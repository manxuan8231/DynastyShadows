using TMPro;
using UnityEngine;

public class Quest2 : MonoBehaviour
{
    public GameObject questPanel;// Panel hiển thị thông tin nhiệm vụ 
    public TextMeshProUGUI questNameText;// Tên nhiệm vụ
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ

    // Quest Village
    private bool questVillageStarted = false;
    public GameObject enemy; // Kẻ thù trong quest

    void Start()
    {
        iconQuest.SetActive(false);
        enemy.SetActive(false); // Ẩn kẻ thù khi bắt đầu
        questPanel.SetActive(false);
       
    }
   

    // Bắt đầu quest
    public void StartQuestVillage()
    {
        if (questVillageStarted) return;// Nếu quest đã bắt đầu thì không làm gì cả
        enemy.SetActive(true); // Kẻ thù sẽ xuất hiện khi bắt đầu quest
        questVillageStarted = true;
        iconQuest.SetActive(true);
        questPanel.SetActive(true);
        questNameText.text = "Đến khu đánh cá điều tra về bọ yêu tinh quấy phá";
    }
}
