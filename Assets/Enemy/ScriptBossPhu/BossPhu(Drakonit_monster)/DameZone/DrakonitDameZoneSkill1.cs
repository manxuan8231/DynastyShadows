using UnityEngine;

public class DrakonitDameZoneSkill1 : MonoBehaviour
{
    public float dame = 500f;
    PlayerStatus playerStatus;
    void Start()
    {
        GameObject gameObject = GameObject.Find("Stats");
        if (gameObject != null)
        {
            playerStatus = gameObject.GetComponent<PlayerStatus>();
           
        }
    }

    
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerStatus != null)
            {
                playerStatus.TakeHealthStun(dame);
            }
        }
    }
}
