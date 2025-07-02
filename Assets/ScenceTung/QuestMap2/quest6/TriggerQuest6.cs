using System.Collections;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class TriggerQuest6 : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasHN;
    public TMP_Text contentHN;
    public GameObject canvasQuest;
    public TMP_Text content;
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;
    [Header("Tham chiếu")]
    public ActiveQuest6 quest6;
    [Header("Boolean")]
    bool isPlayQuest6 = false; // Biến để kiểm tra xem nhiệm vụ đã được kích hoạt hay chưa

    [Header("Other")]
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    [SerializeField] 
    public string[] enemyTags; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;
    void Start()
    {
        quest6 = FindAnyObjectByType<ActiveQuest6>(); // Tìm đối tượng ActiveQuest6 trong scene
    }

    // Update is called once per frame
    void Update()
    {
        if (quest6.isActiveQuest6 && !isPlayQuest6)
        {
            isPlayQuest6 = true; // Đánh dấu là nhiệm vụ đã được kích hoạt
            StartCoroutine(TextQuest6()); // Gọi coroutine khi nhiệm vụ đang hoạt động
        }
        if (!hasSpawned)
        {
            hasSpawned = true; // Đánh dấu là đã spawn enemy
            SpawnEnemies(); // Gọi hàm spawn enemy nếu chưa spawn
        }
    }
    IEnumerator TextQuest6()
    {
        canvasHN.SetActive(true);
        contentHN.text = "Khu vực này khiến mình cảm thấy khó chịu.";
        yield return new WaitForSeconds(2.5f);
        contentHN.text = "Nơi này trong đông sinh vật biến dị hơn mình nghĩ.";
        yield return new WaitForSeconds(2.5f);
        contentHN.text = "Có vẽ âm khí ở đây khá nặng.";
        yield return new WaitForSeconds(2.5f);
        contentHN.text = "Có tên nào trông như mình đã gặp qua ?...";
        yield return new WaitForSeconds(2.5f);
        canvasHN.SetActive(false);
        canvasQuest.SetActive(true); // Hiển thị canvas nhiệm vụ
        content.text = "Nhiệm vụ : Tiêu diệt hết tất cả quái vật ở hội chợ "; // Cập nhật nội dung nhiệm vụ
        stateCanvas.SetActive(true); // Hiển thị canvas trạng thái
        stateText.text = "Bạn vừa nhận nhiệm vụ mới !"; // Cập nhật trạng thái nhiệm vụ
        missionName.text = " Tiêu diệt quái vật ở hội chợ ! "; // Cập nhật tên nhiệm vụ
        iconState.sprite = spirteState; // Cập nhật biểu tượng trạng thái nhiệm vụ
        yield return new WaitForSeconds(2.5f); // Chờ 1 giây trước khi kết thúc
        stateCanvas.SetActive(false); // Ẩn canvas trạng thái


    }

    void SpawnEnemies()
    {
        int count = Mathf.Min(enemySpawnCount, spawnPoints.Length);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = spawnPoints[i].position;
            string currentTag = enemyTags[i % enemyTags.Length];

            GameObject enemy = ObjPoolingManager.Instance.GetEnemyFromPool(currentTag, spawnPos);

            if (enemy == null)
            {
                Debug.LogWarning("Enemy không đủ trong pool!");
            }
        }

    }
}
