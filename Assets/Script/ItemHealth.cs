using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class ItemHealth : MonoBehaviour
{
    PlayerStatus playerStatus;
    public Transform player;
   public AIPath aiPath;
    void Start()
    {
        GameObject gameObject = GameObject.Find("Stats");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (gameObject != null)
        {
            playerStatus = gameObject.GetComponent<PlayerStatus>();
        }
     
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        aiPath = GetComponent<AIPath>();

    }

    // Update is called once per frame
    void Update()
    {

        aiPath.canSearch = true; // Bật tìm kiếm để AIPath có thể tìm đường đến player
        aiPath.canMove = true; // Bật di chuyển để AIPath có thể di chuyển đến player
        aiPath.destination = player.position; // Cập nhật vị trí đến player
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (playerStatus != null)
            {
                playerStatus.AddExp(50); // Tăng exp cho người chơi
                Destroy(gameObject); // Xóa item sau khi nhặt
            }
        }
    }
}
