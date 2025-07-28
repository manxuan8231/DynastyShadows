using UnityEngine;

public class BossAttackCollider1 : MonoBehaviour
{
    public int damage = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<IDamageable>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
