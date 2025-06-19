using UnityEngine;

public class Checkpoin : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerControllerState player = other.GetComponent<PlayerControllerState>();
            if (player != null)
            {
                Debug.Log("đã lưu vị trí");
                // Lưu vị trí checkpoint
                player.SetCheckpoint(transform.position);
               
            }
        }
    }
}

