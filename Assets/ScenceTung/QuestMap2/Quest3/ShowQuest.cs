using UnityEngine;

public class ShowQuest : MonoBehaviour
{
    public SuccesQuest2 quest2;
    public GameObject[] statue;
    bool isSpawn = false;

    void Start()
    {
        quest2 = FindAnyObjectByType<SuccesQuest2>();
    }

    // Update is called once per frame
    void Update()
    {
        MissionPlay();

    }
    void MissionPlay()
    {
        if (quest2.isQuest2Complete && !isSpawn)
        {
            isSpawn = true; // Đánh dấu là đã spawn
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
