using UnityEngine;

public class DameZoneEnemy1 : MonoBehaviour
{
  PlayerStatus playerStatus;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStatus.TakeHealth(20);
            Debug.Log("Player hit by enemy");
        }
    }



}
