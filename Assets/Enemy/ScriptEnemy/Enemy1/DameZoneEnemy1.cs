using UnityEngine;

public class DameZoneEnemy1 : MonoBehaviour
{
  PlayerStatus playerStatus;

    public GameObject enemy;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStatus.TakeHealth(50, enemy);
            Debug.Log("Player hit by enemy");
        }
    }

     

}
