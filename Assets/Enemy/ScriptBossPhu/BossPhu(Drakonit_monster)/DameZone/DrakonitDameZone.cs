using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DrakonitDameZone : MonoBehaviour
{
    public Collider dameZoneCollider;
    public float dame = 50f;
    public string tagPlayer;
    public List<Collider> listDame = new List<Collider>();

    void Start()
    {
        dameZoneCollider.enabled = false;
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagPlayer) && !listDame.Contains(other))
        {
           listDame.Add(other);
            other.GetComponent<PlayerStatus>().TakeHealth(dame);
        }
    }
    private void OnTriggerStay(Collider other)//nếu ontrigger xử lấy ko kịp thì nó dô đây xử lý tiếp
    {
        if (other.gameObject.CompareTag(tagPlayer) && !listDame.Contains(other))
        {
            listDame.Add(other);
            other.GetComponent<PlayerStatus>().TakeHealth(dame);
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
