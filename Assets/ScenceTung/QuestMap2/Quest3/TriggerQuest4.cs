using UnityEngine;

public class TriggerQuest4 : MonoBehaviour
{
    public TimeLineQuest3 timeLineQuest3; // Reference to the TimeLineQuest3 script
    public GameObject[] objects;
    bool isSpawnQuest4 = false;
    void Start()
    {
        timeLineQuest3 = FindAnyObjectByType<TimeLineQuest3>();
    }

    // Update is called once per frame
    void Update()
    {
        MissionPlay();

    }
    void MissionPlay()
    {
        if (timeLineQuest3.isQuest3Complete && !isSpawnQuest4)
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
