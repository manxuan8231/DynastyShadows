using UnityEngine;

public class NPCDetector : MonoBehaviour
{
    private NPCController npc;

    void Start()
    {
        npc = GetComponent<NPCController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            NPCController otherNPC = other.GetComponent<NPCController>();
            if (otherNPC != null)
            {
                npc.TryStartConversation(otherNPC);
            }
        }
    }
}
