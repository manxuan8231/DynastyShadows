using System.Collections;
using TMPro;
using UnityEngine;

public class Quest3 : MonoBehaviour
{
    public GameObject questPanel;// Panel hiển thị thông tin nhiệm vụ 
    public TextMeshProUGUI questNameText;// Tên nhiệm vụ
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerLinhCanh; //mui ten chi duong den linh canh
    public GameObject niceQuest; // UI nhiệm vụ đẹp    
    // Quest linh canh
    private bool questVillageStarted = false;

   // public GameObject enemy;//để suất hiện
   //public GameObject bossOrk;//để suất h
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    //boss
    public Transform[] spawnPointsBoss; // Vị trí spawn sẵn
    public int bossSpawnCount; // Số enemy muốn spawn
    public string bossTag; // Tag của enemy dùng để gọi từ pool

    [Header("Tham so cua boss va enemy")] // Tiêu đề cho các tham số bên dưới trong Inspector
    //tham so 
    public int killEnemy;
    public int killOrk;

    //tham chieu
    TurnInQuest3 turnInQuest3;

    void Start()
    {
        iconQuest.SetActive(false);    
        questPanel.SetActive(false);
        pointerLinhCanh.SetActive(false);
        turnInQuest3 = FindAnyObjectByType<TurnInQuest3>();
        niceQuest.SetActive(false); // Ẩn UI nhiệm vụ đẹp khi bắt đầu
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdateKillOrk(1); // Giả lập việc tiêu diệt Ork khi nhấn T
        }
    }

    // Bắt đầu quest
    public void StartQuestLinhCanh()
    {
        if (questVillageStarted) return;// Nếu quest đã bắt đầu thì không làm gì cả     
        questVillageStarted = true;
        iconQuest.SetActive(true);
        questPanel.SetActive(true);
        
        SpawnEnemies();// Gọi hàm spawn enemy
       
        questNameText.text = $"Tiêu diệt yêu tinh {killEnemy}/{8}";
    }
    public void UpdateKillEnemy(int amount)
    {
        killEnemy += amount;
        questNameText.text = $"Tiêu diệt yêu tinh {killEnemy}/{8}";
        if (killEnemy >= 8)
        {
            SpawnBossOrk();
            questNameText.text = $"Tiêu diệt yêu tinh Ork {killOrk}/{1}";
        }
    }
    public void UpdateKillOrk(int amount)
    {
        killOrk += amount;
        if (killOrk >= 1)
        {
            iconQuest.SetActive(false) ;
            pointerLinhCanh.SetActive(true);
            // Cho phép NPC nhận nhiệm vụ
            turnInQuest3.isContent = true;
            turnInQuest3.isButtonF = true;
            StartCoroutine(WaitNiceQuest());// Hiện UI nhiệm vụ đẹp
            questNameText.text = $"Tới chổ Lính Canh trả nhiệm vụ";
        }       
    }
    public IEnumerator WaitNiceQuest() {
        niceQuest.SetActive(true); // Ẩn UI nhiệm vụ đẹp khi bắt đầu
        yield return new WaitForSeconds(5f); // Hiện trong 5 giây
        niceQuest.SetActive(false); // Ẩn UI nhiệm vụ đẹp sau 5 giây
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
    void SpawnBossOrk()
    {
        int count = Mathf.Min(bossSpawnCount, spawnPointsBoss.Length);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = spawnPointsBoss[i].position;

            GameObject enemy = ObjPoolingManager.Instance.GetEnemyFromPool(bossTag, spawnPos);

            if (enemy == null)
            {
                Debug.LogWarning("Enemy không đủ trong pool!");
            }
        }
    }
}
