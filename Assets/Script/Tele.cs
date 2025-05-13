using UnityEngine;

public class Tele : MonoBehaviour
{
    public Transform teleportLocation;

    public void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && teleportLocation != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;
                player.transform.position = teleportLocation.position;
                cc.enabled = true;
            }
        }
    }
}
