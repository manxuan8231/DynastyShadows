using UnityEngine;

public class TelePlayerTarget : MonoBehaviour
{
    public GameObject player; // Tham chiếu đến đối tượng người chơi
    PlayerControllerState playerControllerState; // Tham chiếu đến trạng thái người chơi
    void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update()
    {
        TeleportToPlayer();
    }
    public void TeleportToPlayer()
    {
        if (player != null)
        {
            playerControllerState.controller.enabled = false; // Tắt điều khiển của người chơi để tránh va chạm không mong muốn
            player.transform.position = transform.position; // Di chuyển người chơi đến vị trí của đối tượng này
            playerControllerState.controller.enabled = true;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy đối tượng người chơi!");
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           gameObject.SetActive(false); // Tắt đối tượng này khi người chơi va chạm
        }
    }
}
