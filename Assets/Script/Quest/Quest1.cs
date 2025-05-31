using TMPro;
using UnityEngine;

public class Quest1 : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ

    //// Các điểm xuất hiện kẻ thù
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
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
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemies();
        }
    }
    // Bắt đầu quest Bác Lâm
    public void StartQuestBacLam()
    {
        if (questBacLamStarted) return;
        SpawnEnemies(); // Gọi hàm để spawn kẻ thù ngay lập tức
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

    void SpawnEnemies()
    {
        int count = Mathf.Min(enemySpawnCount, spawnPoints.Length);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = spawnPoints[i].position;

            GameObject enemy = ObjPoolingManager.Instance.GetEnemyFromPool(enemyTag, spawnPos);

            if (enemy == null)
            {
                Debug.LogWarning("Enemy không đủ trong pool!");
            }
        }
    }
}
