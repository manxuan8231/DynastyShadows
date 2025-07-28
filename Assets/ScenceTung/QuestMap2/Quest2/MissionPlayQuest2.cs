using UnityEngine;

public class MissionPlayQuest2 : MonoBehaviour
{
    public GameObject[] statue;
    public NPCQuest npcQuest;
    SuccesQuest2 succesQuest2;
    void Start()
    {
        npcQuest = FindAnyObjectByType<NPCQuest>();
        succesQuest2 = FindFirstObjectByType<SuccesQuest2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (succesQuest2.isQuest2Complete) return;

        MissionPlay();
        

    }

    void MissionPlay()
    {
        if (npcQuest != null)
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
        else
            return;
      
    }
}
