using UnityEngine;

public class BossAttackCollider : MonoBehaviour
{
    public BossScript boss; // tham chiếu boss
    public float damage = 200f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.TakeHealth(damage, boss.gameObject, "HitBack");
                Debug.Log($"💥 Boss {boss.name} hit player with {damage} damage!");
            }
        }
    }
}
