using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questPanel; // Panel hiển thị thông tin quest
    public TextMeshProUGUI questName; // Tên quest
    public GameObject icon; // Icon trên bản đồ cua bac lam
    //quest 1 bac lam
    private int quest1BacLamCount = 0; // Biến đếm số lượng quái đã tiêu diệt
    //icon tren map 
    public GameObject iconQuest; // Icon trên bản đồ chổ cần tiêu diet quai

    //tham chieu
    NPCScript npcScript; // Tham chiếu đến NPCScript
    TurnInQuest turnInQuest; // Tham chiếu đến TurnInQuest
    void Start()
    {
        iconQuest.SetActive(false); // Ẩn icon quest khi bắt đầu
        questPanel.SetActive(false); // Ẩn panel quest khi bắt đầu
        npcScript = FindAnyObjectByType<NPCScript>(); // Lấy tham chiếu đến NPCScript
        turnInQuest = FindAnyObjectByType<TurnInQuest>(); // Lấy tham chiếu đến TurnInQuest
    }

   
    void Update()
    {
        
    }

    public void StartQuestBacLam()
    {
        // Code để bắt đầu quest
        Debug.Log("Quest 1");
        iconQuest.SetActive(true); // Hiện icon quest trên bản đồ
        questPanel.SetActive(true); // Hiện panel quest
        questName.text = $"Hạ hết quái ở bãi gỗ của Bác Lâm {quest1BacLamCount}/5"; // Hiển thị tên quest

    }
    public void UpdateQuestBacLam(int amount)
    {
        quest1BacLamCount += amount; // Tăng số lượng quái đã tiêu diệt
        questName.text = $"Hạ hết quái ở bãi gỗ của Bác Lâm {quest1BacLamCount}/5"; // Cập nhật tên quest
        quest1BacLamCount = Mathf.Clamp(quest1BacLamCount, 0, 5); // Giới hạn số lượng quái đã tiêu diệt từ 0 đến 5
        if (quest1BacLamCount >= 5)
        {
            // CompleteQuestBacLam(); // Hoàn thành quest
            questName.text = $"Tới chổ Bác Lâm trả nhiệm vụ"; // Cập nhật tên quest
            turnInQuest.isContent = true; // Đặt trạng thái hội thoại là true
            turnInQuest.isButtonF = true; // Đặt trạng thái nút F là true
            iconQuest.SetActive(false); // Ẩn icon quest trên bản đồ
        }
    }
}
