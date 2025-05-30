using UnityEngine;

public class DameZoneSkill2 : MonoBehaviour
{
    public float dame = 100f;
    public PlayerStatus status;
    private void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            status.TakeHealth(dame);
        }
    }
}
