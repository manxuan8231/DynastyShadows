using TMPro;
using UnityEngine;

public class Quest3 : MonoBehaviour
{
    public GameObject questPanel;// Panel hiển thị thông tin nhiệm vụ 
    public TextMeshProUGUI questNameText;// Tên nhiệm vụ
    public GameObject iconQuest; // Icon hiển thị nơi làm nhiệm vụ
    public GameObject pointerLinhCanh; //mui ten chi duong den linh canh
        
    // Quest linh canh
    private bool questVillageStarted = false;

    public GameObject enemy;//để suất hiện
    public GameObject bossOrk;//để suất h

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
        enemy.SetActive(true);
        bossOrk.SetActive(false);
        questNameText.text = $"Tiêu diệt yêu tinh {killEnemy}/{8}";
    }
    public void UpdateKillEnemy(int amount)
    {
        killEnemy += amount;
        questNameText.text = $"Tiêu diệt yêu tinh {killEnemy}/{8}";
        if (killEnemy >= 8)
        {
            bossOrk.SetActive(true);
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
            questNameText.text = $"Tới chổ Lính Canh trả nhiệm vụ";
        }       
    }
}
