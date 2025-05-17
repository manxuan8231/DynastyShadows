using UnityEngine;

public class DraKonitDameSkill2 : MonoBehaviour
{
    public float damagePerSecond = 10f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStatus status = other.GetComponent<PlayerStatus>();
            if (status != null)
            {
                // Gây sát thương theo thời gian
                status.TakeHealth(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
