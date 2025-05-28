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
    public GameObject enemyPrefab; // Prefab ẩn hiện
    public float enemyCount = 0; // Biến đếm số lượng kẻ thù đã tiêu diệt
    //tham chieu
   
    public NPCScript linhCanhB;
    void Start()
    {
        iconQuest.SetActive(false);
        questPanel.SetActive(false);
        pointerLinhCanhB.SetActive(false);

        iconQuest2.SetActive(false);
        linhCanhB.enabled = false; // Tắt script NPC để không tương tác được
        pointerEnemy.SetActive(false);
        enemyPrefab.SetActive(false); // Ẩn prefab kẻ thù ban đầu
       
    }
    private void Update()
    {
        if(enemyCount >= 1)
        {
            pointerEnemy.SetActive(false); // Ẩn mũi tên chỉ đường đến kẻ thù
            enemyPrefab.SetActive(false); // Ẩn prefab kẻ thù
            iconQuest2.SetActive(false); // Ẩn icon nhiệm vụ
            questPanel.SetActive(false); // Ẩn panel nhiệm vụ
            questNameText.text = ""; // Xóa tên nhiệm vụ
           
            enemyCount = 0; // Reset số lượng kẻ thù đã tiêu diệt
        }
    }

    // Bắt đầu quest
    public void StartQuestMainBacLam()
    {
        linhCanhB.enabled = true;
        iconQuest.SetActive(true);// Hiện icon nhiệm vụ trên bản đồ
        questPanel.SetActive(true);// Hiện panel nhiệm vụ
        pointerLinhCanhB.SetActive(true);// Hiện mũi tên chỉ đường đến Lính Canh B
        questNameText.text = $"Nhiệm vụ đến chổ Lính Canh B";
    }

    public void StartQuestLinhCanhB()
    {
        enemyPrefab.SetActive(true); // Hiện prefab kẻ thù
        iconQuest2.SetActive(true);// Hiện icon nhiệm vụ trên bản đồ
        iconQuest.SetActive(false);
        pointerEnemy.SetActive(true);// Hiện mũi tên chỉ đường đến Lính Canh B
        questNameText.text = $"Nhiệm vụ Tiêu diệt sinh vật ở khu đầm lầy";
    }

    public void UpdateKillEnemy(float amount)
    {
        enemyCount += amount; // Cập nhật số lượng kẻ thù đã tiêu diệt
    }
}
