using UnityEngine;
using UnityEngine.AI;

public class ItemHealth : MonoBehaviour
{
    PlayerStatus playerStatus;
    public Transform player;
    public NavMeshAgent agent;
    void Start()
    {
        GameObject gameObject = GameObject.Find("Stats");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        if (gameObject != null)
        {
            playerStatus = gameObject.GetComponent<PlayerStatus>();
        }
     
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
          
            agent.SetDestination(player.transform.position);
        
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
