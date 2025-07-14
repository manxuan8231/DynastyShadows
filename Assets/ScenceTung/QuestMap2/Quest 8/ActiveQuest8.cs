using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveQuest8 : MonoBehaviour
{
    public NPCDialogueController npc;
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public PlayableDirector playableDirector;
    public GameObject model;
    void Start()
    {
        npc = GameObject.Find("NPCQuest4").GetComponent<NPCDialogueController>();
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
    }

    // Update is called once per frame
    void Update()
    {
        if(npc.currentStage == QuestStage.Quest7Completed)
        {
            StartCoroutine(StartQuest());
           
        }
    }
    IEnumerator StartQuest()
    {
        yield return new WaitForSeconds(5f);
        playerInGame.SetActive(false); // Ẩn player thật
        timeLine.SetActive(true); // Bật timeline
        playableDirector.Play(); // Chạy timeline
        playableDirector.stopped += OnTimelineFinished; // Đăng ký sự kiện khi timeline kết thúc

    }
    private void OnTimelineFinished(PlayableDirector director)
    {
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine.SetActive(false);
        model.SetActive(true); // Kích hoạt mô hình sau khi timeline kết thúc
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }
}
