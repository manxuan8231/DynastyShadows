using System.Collections.Generic;
using UnityEngine;

public class DameZoneWeapon : MonoBehaviour
{
    public Collider dameZoneCollider;
    public float dame = 50f;
    public string tagPlayer;
    public List<Collider> listDame = new List<Collider>();
    PlayerStatus playerStatus;
    public GameObject enemy;
    void Start()
    {
        dameZoneCollider.enabled = false;

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
            playerStatus.TakeHealth(dame, enemy, "HitLeft");
            playerStatus.TakeHealShield(dame);
        }
    }
    private void OnTriggerStay(Collider other)//nếu ontrigger xử lấy ko kịp thì nó dô đây xử lý tiếp
    {
        if (other.gameObject.CompareTag(tagPlayer) && !listDame.Contains(other))
        {
            listDame.Add(other);
            playerStatus.TakeHealth(dame, enemy, "HitLeft");
            playerStatus.TakeHealShield(dame);
        }
    }
    public void beginDame()
    {
        listDame.Clear();
        dameZoneCollider.enabled = true;
    }
    public void endDame()
    {
        listDame.Clear();//xóa
        dameZoneCollider.enabled = false;
    }
}
