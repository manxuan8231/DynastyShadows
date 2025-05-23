using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    PlayerStatus playerStatus;
    void Start()
    {
        GameObject gameObject = GameObject.Find("Stats");
        if (gameObject != null)
        {
            playerStatus = gameObject.GetComponent<PlayerStatus>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            if (playerStatus != null)
            {
                playerStatus.AddExp(100); // Tăng exp cho người chơi
                Destroy(gameObject); // Xóa item sau khi nhặt
            }
        }
    }
}
