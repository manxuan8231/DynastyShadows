using UnityEngine;

public class DameZone_Horseman : MonoBehaviour
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
            playerStatus.TakeHealShield(50);
            Debug.Log("Player hit by enemy");
        }
    }



}
