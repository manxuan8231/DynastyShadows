using UnityEngine;

public class DraKonitDameSkill2 : MonoBehaviour
{
    public float damagePerSecond = 10f;
    PlayerStatus playerStatus;
    private void Start()
    {
        GameObject gt = GameObject.Find("ItemCanUsing");
        if (gt != null)
        {
            playerStatus = gt.GetComponent<PlayerStatus>();
           
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            playerStatus.TakeHealth(damagePerSecond * Time.deltaTime); // Gây sát thương theo thời gian      

        }
    }
}
