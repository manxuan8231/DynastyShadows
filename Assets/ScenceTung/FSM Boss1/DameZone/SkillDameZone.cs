using UnityEngine;

public class SkillDameZone : MonoBehaviour
{
    public float dame = 15f;
    public PlayerStatus status;
    private void Start()
    {
        status = GameObject.Find("Stats").GetComponent<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            status.TakeHealth(dame);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            status.TakeHealth(dame * Time.deltaTime * dame);
        }
    }
}
