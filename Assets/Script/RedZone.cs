using UnityEngine;

public class RedZone : MonoBehaviour
{
    public GameObject telePre;
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
            other.transform.position = telePre.transform.position;
            playerControllerState.controller.enabled = true;
            Debug.Log("Player teleported to red zone position.");
        }
    }
}
