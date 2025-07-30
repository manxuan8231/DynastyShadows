using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class ShopMap2 : MonoBehaviour
{
    [Header("-----------------Time Line-----------------")]
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public PlayableDirector playableDirector; 
    public GameObject ThuongNhan;
    NPCDialogueController npc;
    public bool isTimelineOpen = false;
    public GameObject quest6;
    private void Start()
    {
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
        npc = FindAnyObjectByType<NPCDialogueController>();
    }

    private void Update()
    {
        if (npc.currentStage == QuestStage.Quest5Completed && !isTimelineOpen)
        {
            isTimelineOpen = true; // Đánh dấu là đã mở timeline
            StartCoroutine(StartQuest());
        }
    }
    public IEnumerator StartQuest()
    {
        yield return new WaitForSeconds(3f);
        quest6.SetActive(false);
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
        ThuongNhan.SetActive(true); // Kích hoạt mô hình sau khi timeline kết thúc
        quest6.SetActive(true);
        timeLine.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }
}
