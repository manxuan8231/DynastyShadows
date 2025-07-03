using UnityEngine;

public class Back : MonoBehaviour
{
    NPCDialogueController npcDialogueController;
    public GameObject tele;
    private void Start()
    {
        npcDialogueController = FindAnyObjectByType<NPCDialogueController>();
        
    }
    private void Update()
    {
       if(npcDialogueController.currentStage == QuestStage.Quest5Completed)
        {
             tele.SetActive(true);
        }
    }
}
