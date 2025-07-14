using UnityEngine;

public class DameZoneSkill2 : MonoBehaviour
{
    public float dame = 10f;
    public PlayerStatus status;
    private void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            status.TakeHealth(dame, gameObject, "HitRight", 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            status.TakeHealth(dame * Time.deltaTime , gameObject, "HitRight", 1);
        }
    }

}
