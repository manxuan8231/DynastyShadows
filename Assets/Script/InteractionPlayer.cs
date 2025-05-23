using UnityEngine;

public class InteractionPlayer : MonoBehaviour
{
    //ke thua
  
    private PlayerStatus playerStatus;
    void Start()
    {
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
            int randomValueAT = Random.Range(20, 50);
            playerStatus.BuffHealth(randomHealth);
            playerStatus.AddExp(randomValueAT);
            //destroy vat pham
            Destroy(other.gameObject);
        }
    }
}
