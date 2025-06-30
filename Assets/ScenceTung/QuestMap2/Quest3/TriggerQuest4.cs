using UnityEngine;

public class TriggerQuest4 : MonoBehaviour
{
    public ActiveQuest4 activeQuest4;
    public GameObject[] objects;
    bool isSpawnQuest4 = false;
    void Start()
    {
        activeQuest4 = FindFirstObjectByType<ActiveQuest4>();
       
    }

    // Update is called once per frame
    void Update()
    {
        MissionPlay();

    }
    void MissionPlay()
    {
        if (activeQuest4.isActiveQuest4== true && !isSpawnQuest4)
        {
            isSpawnQuest4 = true; // Đánh dấu là đã spawn
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(false);
            }
        }
    }
}
