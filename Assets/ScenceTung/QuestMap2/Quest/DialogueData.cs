using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Quest/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string[] contentTextQuest;
    public string[] nameTextQuest;
    public string missionName;
    public Sprite iconState;
    public string stateText;
    public string contentQuestGame;
}
