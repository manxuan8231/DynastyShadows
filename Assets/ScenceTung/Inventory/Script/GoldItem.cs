using TMPro;
using UnityEngine;

public class GoldItem : MonoBehaviour
{
    public PlayerStatus status;
    
    private void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
           status. IncreasedGold(10);
            Destroy(gameObject);
           
        }
    }
   
  
}
