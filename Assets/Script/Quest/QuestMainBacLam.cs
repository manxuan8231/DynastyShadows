using System.Collections;
using TMPro;
using UnityEngine;

public class QuestMainBacLam : MonoBehaviour
{
    [Header("UI & Quest Elements")]
    public GameObject iconQuestMainBacLam;
    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public GameObject iconQuest;
    public GameObject pointerLinhCanhB;
    public GameObject QuestDesert;
    public GameObject iconQuest2;
    public GameObject pointerEnemy;
    public GameObject niceQuest;
    public GameObject questPointer;//ẩn đi khi bắt đầu nhiệm vụ
    [Header("Quest Logic")]
    public float enemyCount = 0;
    private bool questStarted = false;
    private bool subQuestStarted = false;
    private bool questCompleted = false;

    [Header("References")]
    public NPCScript linhCanhB;
    public BoxCollider boxCollider;
    private PlayerStatus playerStatus;

    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();

        // Disable all UI and quest markers at the beginning
        iconQuest.SetActive(false);
        questPanel.SetActive(false);
        pointerLinhCanhB.SetActive(false);
        QuestDesert.SetActive(false);
        iconQuest2.SetActive(false);
        pointerEnemy.SetActive(false);
        niceQuest.SetActive(false);
        linhCanhB.enabled = false;
        boxCollider.enabled = false;
    }

    void Update()
    {
        // Kiểm tra hoàn thành subquest nếu đang làm và chưa hoàn thành
        if (subQuestStarted && !questCompleted && enemyCount >= 1)
        {
            CompleteQuestMainBacLam();
        }
    }

    /// <summary>
    /// Bắt đầu nhiệm vụ chính từ NPC Bác Lâm
    /// </summary>
    public void StartQuestMainBacLam()
    {
        if (questStarted) return;

        questStarted = true;

        linhCanhB.enabled = true;
        boxCollider.enabled = true;

        iconQuest.SetActive(true);
        questPanel.SetActive(true);
        pointerLinhCanhB.SetActive(true);
        iconQuestMainBacLam.SetActive(false);
        questPointer.SetActive(false);
        questNameText.text = "Đến chỗ Lính Canh B";
    }

    /// <summary>
    /// Khi nói chuyện với Lính Canh B, bắt đầu nhiệm vụ phụ
    /// </summary>
    public void StartQuestLinhCanhB()
    {
        if (subQuestStarted) return;

        subQuestStarted = true;

        iconQuest2.SetActive(true);
        iconQuest.SetActive(false);
        pointerLinhCanhB.SetActive(false);
        pointerEnemy.SetActive(true);

        questNameText.text = "Tiêu diệt sinh vật ở khu đầm lầy";
    }

    /// <summary>
    /// Gọi khi tiêu diệt kẻ địch
    /// </summary>
    public void UpdateKillEnemy(float amount)
    {
        enemyCount += amount;
    }

    /// <summary>
    /// Hoàn thành toàn bộ nhiệm vụ chính
    /// </summary>
    private void CompleteQuestMainBacLam()
    {
        questCompleted = true;
        enemyCount = 0;

        pointerEnemy.SetActive(false);
        QuestDesert.SetActive(true);
        iconQuest2.SetActive(false);
        questPanel.SetActive(false);
        questNameText.text = "";

        playerStatus.IncreasedGold(1000);
        StartCoroutine(WaitNiceQuest());
    }

    /// <summary>
    /// Hiện UI báo hoàn thành nhiệm vụ
    /// </summary>
    private IEnumerator WaitNiceQuest()
    {
        niceQuest.SetActive(true);
        yield return new WaitForSeconds(5f);
        niceQuest.SetActive(false);
    }
}
