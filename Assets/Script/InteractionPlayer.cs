using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    //ke thua
    private LevelAvatar itemHealth;
    private PlayerStatus playerStatus;
    void Start()
    {
        itemHealth = FindAnyObjectByType<LevelAvatar>();      
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }

    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Exp"))
        {
            float randomHealth = Random.Range(500, 2000);
            int randomValueAT = Random.Range(1, 5);
            playerStatus.BuffHealth(randomHealth);
            itemHealth.AddValueAvatar(randomValueAT);
            //destroy vat pham
            Destroy(other.gameObject);
        }
    }
}
