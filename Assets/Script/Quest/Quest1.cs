using TMPro;
using UnityEngine;

public class Quest1 : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ

    //// Các điểm xuất hiện kẻ thù
    public Transform[] spawnPoints;
    public int enemyCountToSpawn = 10;
    public bool hasSpawned = false;

    // Quest Bác Lâm
    private int questBacLamKillCount = 0;
    private bool questBacLamStarted = false;
    private bool questBacLamCompleted = false;


    // Tham chiếu
    TurnInQuest turnInQuest;

    void Start()
    {
        iconQuest.SetActive(false);
     
        questPanel.SetActive(false);
        turnInQuest = FindAnyObjectByType<TurnInQuest>();
    }

    // Bắt đầu quest Bác Lâm
    public void StartQuestBacLam()
    {
        if (questBacLamStarted) return;
        SpawnEnemiesInstantly(); // Gọi hàm để spawn kẻ thù ngay lập tức
        questBacLamStarted = true;
        questBacLamKillCount = 0;
        questBacLamCompleted = false;

        iconQuest.SetActive(true);
        questPanel.SetActive(true);
        questNameText.text = $"Tiêu diệt hết quái ở bãi gỗ của Bác Lâm {questBacLamKillCount}/5";
    }

    // Gọi khi tiêu diệt quái
    public void UpdateQuestBacLam(int amount)
    {
        if (!questBacLamStarted || questBacLamCompleted) return;

        questBacLamKillCount += amount;
        questBacLamKillCount = Mathf.Clamp(questBacLamKillCount, 0, 5);
        questNameText.text = $"Tiêu diệt hết quái ở bãi gỗ của Bác Lâm {questBacLamKillCount}/5";

        if (questBacLamKillCount >= 5)
        {
            questBacLamCompleted = true;
            questNameText.text = "Tới chỗ Bác Lâm để trả nhiệm vụ!";
            iconQuest.SetActive(false); // Không còn mục tiêu trên bản đồ
         

            // Cho phép NPC nhận nhiệm vụ
            turnInQuest.isContent = true;
            turnInQuest.isButtonF = true;
        }
    }

    public void SpawnEnemiesInstantly()
    {
        for (int i = 0; i < enemyCountToSpawn; i++)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            EnemyPoolManager.Instance.GetEnemyFromPool(randomPoint.position);
        }


    }
}
