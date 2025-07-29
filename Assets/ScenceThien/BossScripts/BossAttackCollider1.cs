using UnityEngine;

public class BossAttackCollider1 : MonoBehaviour
{
    public int damage = 50;
   public  PlayerStatus playerStatus;
    private void Start()
    {
        playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerStatus.TakeHealth(damage,gameObject,"Hit",0.5f);
        }
    }
}
