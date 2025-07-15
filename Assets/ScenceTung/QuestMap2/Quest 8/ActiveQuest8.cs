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

    //time line 2
    public GameObject timeLine2;
    public GameObject playerTimeLine2;
    public PlayableDirector playableDirector2;
    public GameObject boss;
    public int activeTimeLine = 0; // Biến để xác định timeline nào đang hoạt động

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
        yield return new WaitForSeconds(8f);
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
        timeLine.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }
    private void OnTimelineFinished2(PlayableDirector director)
    {
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine2.transform.position;
        playerInGame.transform.rotation = playerTimeLine2.transform.rotation;
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine2.SetActive(false);
        boss.SetActive(true); // Kích hoạt boss sau khi timeline kết thúc
        timeLine2.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }

    public void StartQuest2()
    {
        activeTimeLine++;
        if (activeTimeLine == 2)
        {
            playerInGame.SetActive(false); // Ẩn player thật
            timeLine2.SetActive(true); // Bật timeline
            playableDirector2.Play(); // Chạy timeline
            playableDirector2.stopped += OnTimelineFinished2; // Đăng ký sự kiện khi timeline kết thúc
        }
    }
}
