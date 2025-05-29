using System.Collections;
using TMPro;
using UnityEngine;

public class QuestMainBacLam : MonoBehaviour
{
    public GameObject questPanel;// Panel hiển thị thông tin nhiệm vụ 
    public TextMeshProUGUI questNameText;// Tên nhiệm vụ
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerLinhCanhB;//mui ten chi duong den linh canh
    public GameObject QuestDesert; // Hiển thị nhiệm vụ bất ngờ ở sa mạc

    public GameObject iconQuest2; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerEnemy;//mui ten chi duong den linh canh
   
    public float enemyCount = 0; // Biến đếm số lượng kẻ thù đã tiêu diệt

    public GameObject niceQuest; // Hiển thị thông báo hoàn thành nhiệm vụ
    //tham chieu
    public NPCScript linhCanhB;
    PlayerStatus playerStatus;
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        iconQuest.SetActive(false);
        questPanel.SetActive(false);
        pointerLinhCanhB.SetActive(false);
        QuestDesert.SetActive(false); // Ẩn nhiệm vụ bất ngờ ở sa mạc

        iconQuest2.SetActive(false);
        linhCanhB.enabled = false; // Tắt script NPC để không tương tác được
        pointerEnemy.SetActive(false);
      

        niceQuest.SetActive(false); // Ẩn thông báo hoàn thành nhiệm vụ 
    }
    private void Update()
    {
        //hoan thanh quest
        if(enemyCount >= 1)
        {
            enemyCount = 0; // Reset số lượng kẻ thù đã tiêu diệt
            pointerEnemy.SetActive(false); // Ẩn mũi tên chỉ đường đến kẻ thù
            QuestDesert.SetActive(true); // Ẩn nhiệm vụ bất ngờ ở sa mạc
            iconQuest2.SetActive(false); // Ẩn icon nhiệm vụ
            questPanel.SetActive(false); // Ẩn panel nhiệm vụ
            questNameText.text = ""; // Xóa tên nhiệm vụ

            CompleteQuestMainBacLam();// Gọi hàm hoàn thành nhiệm vụ
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
       
        iconQuest2.SetActive(true);// Hiện icon nhiệm vụ trên bản đồ
        iconQuest.SetActive(false);
        pointerEnemy.SetActive(true);// Hiện mũi tên chỉ đường đến Lính Canh B
       
        questNameText.text = $"Nhiệm vụ Tiêu diệt sinh vật ở khu đầm lầy";
    }
    //hoan thanh quest
    public void CompleteQuestMainBacLam()
    {
        StartCoroutine(WaitNiceQuest()); // Bắt đầu coroutine để hiển thị thông báo hoàn thành nhiệm vụ
        //phần thưởng
        playerStatus.AddExp(1000); // Thêm kinh nghiệm cho người chơi
    }
    public void UpdateKillEnemy(float amount)
    {
        enemyCount += amount; // Cập nhật số lượng kẻ thù đã tiêu diệt
    }

    private IEnumerator WaitNiceQuest()
    {
        niceQuest.SetActive(true); // Hiện thông báo hoàn thành nhiệm vụ
        yield return new WaitForSeconds(5f); // Chờ 2 giây
        niceQuest.SetActive(false); // Ẩn thông báo hoàn thành nhiệm vụ
    }
}
