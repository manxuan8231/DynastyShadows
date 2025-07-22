using UnityEngine;

public class RedZone : MonoBehaviour
{
    public GameObject telePosi;
    public PlayerControllerState playerControllerState;
    void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerControllerState.controller.enabled = false;
            other.transform.position = telePosi.transform.position;
            playerControllerState.controller.enabled = true;
            Debug.Log("Player teleported to red zone position.");
        }
    }
}
