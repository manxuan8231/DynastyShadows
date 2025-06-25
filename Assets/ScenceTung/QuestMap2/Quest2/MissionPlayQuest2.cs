using UnityEngine;

public class MissionPlayQuest2 : MonoBehaviour
{
    public GameObject[] statue;
    public NPCQuest npcQuest;

    void Start()
    {
        npcQuest = FindAnyObjectByType<NPCQuest>();
    }

    // Update is called once per frame
    void Update()
    {
        MissionPlay();
        

    }

    void MissionPlay()
    {
        if (npcQuest.hasFinishedDialogue)
        {
            for (int i = 0; i < statue.Length; i++)
            {
                statue[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < statue.Length; i++)
            {
                statue[i].SetActive(false);
            }
        }
    }
}
