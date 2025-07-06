using UnityEngine;

public class TelePathic : MonoBehaviour
{
    public Transform handLeft;              // Điểm đến của teleport
    public GameObject player;               // Tham chiếu đến Player
    public float moveSpeed = 5f;            // Tốc độ dịch chuyển
    private bool isTeleporting = false;
    // Tham chiếu
    private PlayerControllerState playerControllerState;
    private PlayerStatus playerStatus;

   

    void Start()
    {
       
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        isTeleporting = false;
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    void Update()
    {
        if (isTeleporting && player != null)
        {
            
            player.transform.position = Vector3.MoveTowards(player.transform.position,
                handLeft.position, moveSpeed * Time.deltaTime

            );

            // Nếu đã đến nơi thì kết thúc 
            float distance = Vector3.Distance(player.transform.position, handLeft.position);
            if (distance < 3f)
            {
                isTeleporting = false;
                playerControllerState.controller.enabled = true; 
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isTeleporting)
        {
            
            playerControllerState.controller.enabled = false;
            playerStatus.TakeHealth(30,gameObject);
            playerStatus.TakeHealShield(20);
           
            isTeleporting = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTeleporting = false;
            playerControllerState.controller.enabled = true;
        }
    }
   
}
