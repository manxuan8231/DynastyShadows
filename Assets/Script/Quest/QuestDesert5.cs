using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class QuestDesert5 : MonoBehaviour
{
    public GameObject questPanel; // Panel hiển thị thông tin nhiệm vụ
    public TextMeshProUGUI questNameText; // Tên nhiệm vụ
    public Light directionalLight; // Đèn chiếu sáng trong scene

    private bool isScene2 = false;// Biến kiểm tra xem đã vào scene 2 hay chưa
    public bool isPlayer = true; // Biến kiểm tra xem người chơi có đang ở trong khu vực này hay không

    public GameObject enemy; // Đối tượng kẻ thù sẽ xuất hiện trong scene 2
    public float enemyCount;
    public GameObject boss;
    public float bossCount;

    public GameObject niceQuestDesert5;
    //goi ham
    TimeLineDesert timeLineDesert; // Lấy đối tượng TimeLineDesert
   public TurnInQuest5 turnInQuest5; // Lấy đối tượng TurnInQuest5

    // Biến để lưu trữ các điểm spawn của kẻ thù
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số lượng kẻ thù muốn spawn
    public string enemyTag; // Tag của kẻ thù dùng để gọi từ pool
    void Start()
    {
        niceQuestDesert5.SetActive(false); // Ẩn nhiệm vụ sa mạc ban đầu
        boss.SetActive(false);
        timeLineDesert = FindAnyObjectByType<TimeLineDesert>(); // Lấy đối tượng TimeLineDesert
        turnInQuest5 = FindAnyObjectByType<TurnInQuest5>(); // Lấy đối tượng TurnInQuest5
        questPanel.SetActive(false); // Ẩn panel nhiệm vụ ban đầu
        questNameText.text = ""; // Xóa tên nhiệm vụ ban đầu
        enemy.SetActive(false); // Ẩn kẻ thù ban đầu
        turnInQuest5.enabled = false; // Vô hiệu hóa TurnInQuest5 ban đầu
    }


    void Update()
    {
        if (isScene2 == true) // Kiểm tra nếu chưa vào scene 2
        {
            isPlayer = false; // Đặt cờ để không cho người chơi vào scene 2
            isScene2 = false;
            timeLineDesert.isScene2Active = true;
        }
        else
        if (enemyCount >= 10)
        {
            enemyCount = 0; // Reset số lượng kẻ thù đã tiêu diệt
            //boss.SetActive(true); // Hiện boss khi số lượng kẻ thù >= 10
            SpawnEnemies(); // Gọi hàm SpawnEnemies để spawn kẻ thù
            Debug.Log("Boss đã xuất hiện");
        }
        else if (bossCount >= 1)
        {
            bossCount = 0; // Reset số lượng kẻ thù đã tiêu diệt
           
            turnInQuest5.enabled = true; // Kích hoạt TurnInQuest5       
            turnInQuest5.isContent = true; // Đặt trạng thái nhiệm vụ là true
            turnInQuest5.StartTurnInQuest5();
            RenderSettings.fogDensity = 0; //giam độ mờ của sương mù
            directionalLight.color = Color.white; // Đặt màu sắc của ánh sáng
             StartCoroutine(WaitNiceQuest());
            Debug.Log("Hoàn thành nv");
        } 
        
       
    }
    // Bắt đầu quest
    public void StartQuestDesert5()
    {
        questPanel.SetActive(true); // Hiện panel nhiệm vụ
        questNameText.text = $"Đến khu vực sa mạc điều tra nguồn khí tức"; // Hiển thị tên nhiệm vụ
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isPlayer == true)
        {

            isScene2 = true; // Đặt cờ để vào scene 2
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isScene2 = false; // Đặt cờ để không vào scene 2
        }
    }

    public void UpdateKillEnemy(float amount)
    {
        enemyCount += amount; // Cập nhật số lượng kẻ thù đã tiêu diệt
    }
    public void UpdateKillBoss(float amount)
    {
        bossCount += amount; // Cập nhật số lượng kẻ thù đã tiêu diệt
    }

    public IEnumerator WaitNiceQuest()
    {
        niceQuestDesert5.SetActive(true); // Hiện nhiệm vụ sa mạc
        yield return new WaitForSeconds(5f); // Chờ 2 giây
        niceQuestDesert5.SetActive(false); // Ẩn nhiệm vụ sa mạc
       // questPanel.SetActive(false); // Ẩn panel nhiệm vụ
        questNameText.text = $"Trả nhiệm vụ cho Lính Canh B"; // Hiển thị tên nhiệm vụ
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
