using UnityEngine;

public class DrakonitDameZoneSkill1 : MonoBehaviour
{
    public float dame = 500f;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStatus>().TakeHealth(dame);
        }
    }
}
