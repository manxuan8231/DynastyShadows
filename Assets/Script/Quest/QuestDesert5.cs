using TMPro;
using UnityEngine;

public class QuestDesert5 : MonoBehaviour
{
    public GameObject questPanel; // Panel hiển thị thông tin nhiệm vụ
    public TextMeshProUGUI questNameText; // Tên nhiệm vụ                              
    void Start()
    {
        questPanel.SetActive(false); // Ẩn panel nhiệm vụ ban đầu
        questNameText.text = ""; // Xóa tên nhiệm vụ ban đầu
    }

    
    void Update()
    {
        
    }
    // Bắt đầu quest
    public void StartQuestDesert5()
    {
        questPanel.SetActive(true); // Hiện panel nhiệm vụ
        questNameText.text = $"Đến khu vực sa mạc điều tra nguồn khí tức"; // Hiển thị tên nhiệm vụ
    }
}
