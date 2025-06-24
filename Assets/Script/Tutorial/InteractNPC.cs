using UnityEngine;

public class InteractNPC : MonoBehaviour
{
    public bool isInteract = false;
    void Start()
    {
        isInteract = false ;
    }

   
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isInteract = true;
            Destroy(gameObject,0.5f);
        }
    }
}
