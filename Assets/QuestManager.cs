using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private NPCDeathQuest npcQuest1;
    [SerializeField] private SuccesQuest2 npcQuest2;
    [SerializeField] private TimeLineQuest3 npcQuest3; // Assuming you have a TimeLineQuest3 script for quest 3
    [SerializeField] private TimeLineBossDragonDead npcQuest4;
    [SerializeField] private NPCDialogueController npc;
    private void Start()
    {
        npcQuest1 = FindAnyObjectByType<NPCDeathQuest>(); // Find the NPCDeathQuest in the scene
        npcQuest2 = FindAnyObjectByType<SuccesQuest2>(); // Find the SuccesQuest2 in the scene
        DataQuest dataQuestManager = SaveManagerMan.LoadGame().dataQuest; // Load the saved game data
        npcQuest1.isQuest1 = dataQuestManager.isQuest1Map2; // Set the quest status from the saved data
        npcQuest2.isQuest2Complete = dataQuestManager.isQuest2Map2; // Set the quest status from the saved data
        npcQuest3.isQuest3Complete = dataQuestManager.isQuest3Map2; // Set the quest status for quest 3
        npcQuest4.isCompleteQuest4 = dataQuestManager.isQuest4Map2; // Set the quest status for quest 4
        Debug.Log($"Nhiệm vụ {npcQuest4.isCompleteQuest4}");


    }
  
}

