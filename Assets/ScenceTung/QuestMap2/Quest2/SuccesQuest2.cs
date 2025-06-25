using UnityEngine;

public class SuccesQuest2 : MonoBehaviour
{
    public GameObject trigger;
    public bool isQuest2Complete = false;
    public NPCQuest nPCQuest;
    private void Start()
    {
        nPCQuest = FindFirstObjectByType<NPCQuest>();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isQuest2Complete)
        {
            isQuest2Complete = true; // Đánh dấu nhiệm vụ đã hoàn thành
            nPCQuest.MoveToDoneQuest(); // Gọi hàm để NPC di chuyển đến vị trí hoàn thành nhiệm vụ
            
        }
    }
}
