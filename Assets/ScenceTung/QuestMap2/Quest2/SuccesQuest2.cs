using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuccesQuest2 : MonoBehaviour
{
    public GameObject trigger;
    public bool isQuest2Complete = false;
    public NPCQuest nPCQuest;

    public AudioCanvasState audioCanvasState;
    [Header("Trạng thái nhiệm vụ")]

    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;

    public GameObject canvasQuest;

    [Header("Tham chiếu ")]
    public AwardQuest awardQuest;
    private void Start()
    {
        nPCQuest = FindFirstObjectByType<NPCQuest>();
        awardQuest = FindFirstObjectByType<AwardQuest>();
    }
       
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isQuest2Complete)
        {
            isQuest2Complete = true; // Đánh dấu nhiệm vụ đã hoàn thành
            nPCQuest.MoveToDoneQuest(); // Gọi hàm để NPC di chuyển đến vị trí hoàn thành nhiệm vụ
            awardQuest.AwardQuest2(); // Gọi hàm để thưởng nhiệm vụ
            canvasQuest.SetActive(false); // Hiển thị canvas nhiệm vụ
        }
    }
  
}
