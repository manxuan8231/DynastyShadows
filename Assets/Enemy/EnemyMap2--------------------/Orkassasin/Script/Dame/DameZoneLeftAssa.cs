using System.Collections.Generic;
using UnityEngine;

public class DameZoneLeftAssa : MonoBehaviour
{
    public PlayerStatus statusPl;
    public Collider boxDame;

    public float dame = 50f;
    public string tagPlayer;
    public List<Collider> listDame = new List<Collider>();
    PlayerStatus playerStatus;
    public GameObject enemy;
    public string hitAnimationTrigger = "HitLeft"; 
    void Start()
    {
        boxDame.enabled = false;

        GameObject playerObject = GameObject.Find("Stats");
        if (playerObject != null)
        {
            playerStatus = playerObject.GetComponent<PlayerStatus>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagPlayer) && !listDame.Contains(other))
        {
            listDame.Add(other);
            playerStatus.TakeHealth(dame, enemy, hitAnimationTrigger, 0.2f);
            playerStatus.TakeHealShield(dame);
        }
    }
    private void OnTriggerStay(Collider other)//nếu ontrigger xử lấy ko kịp thì nó dô đây xử lý tiếp
    {
        if (other.gameObject.CompareTag(tagPlayer) && !listDame.Contains(other))
        {
            listDame.Add(other);
            playerStatus.TakeHealth(dame, enemy, hitAnimationTrigger, 0.2f);
            playerStatus.TakeHealShield(dame);
        }
    }

    public void BeginDame()
    {
        listDame.Clear();
        boxDame.enabled = true;
    }
    public void EndDame()
    {
        boxDame.enabled = false;
    }
}
