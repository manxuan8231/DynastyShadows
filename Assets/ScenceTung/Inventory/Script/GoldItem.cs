using TMPro;
using UnityEngine;

public class GoldItem : MonoBehaviour
{
    public PlayerStatus status;
    private int gold=0;
    private void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            IncreasedGold(10);
            Destroy(gameObject);
           
        }
    }
   
    public void IncreasedGold(int value)
    {
        gold += value;
       status.goldQuantityTxt.text = value.ToString();

    }

}
