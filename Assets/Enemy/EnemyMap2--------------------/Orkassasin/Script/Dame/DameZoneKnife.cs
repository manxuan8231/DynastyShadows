using System;
using UnityEngine;

public class DameZoneKnife : MonoBehaviour
{
    public float dame;
   
    public GameObject target;
    //tham chieu
    public PlayerStatus status;
    public PlayerControllerState playerControllerState;
    
    Rigidbody rb;
    private void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
       
        rb = GetComponent<Rigidbody>();
       
        target = GameObject.Find("weaponTagertHand");
    }
    private void Update()
    {
       /* if (isTarget)
        {
            transform.position = playerControllerState.transform.position;
        }*/
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            // Làm con của Player 
            transform.SetParent(playerControllerState.transform, true);

            rb.linearVelocity = Vector3.zero; // Dừng chuyển động ngay
            // Gây sát thương và trừ giáp
            status.TakeHealth(dame, gameObject, "HitBack", 0.5f);
            status.TakeHealShield(dame);
            Destroy(gameObject,1f);
        }
    }

}

