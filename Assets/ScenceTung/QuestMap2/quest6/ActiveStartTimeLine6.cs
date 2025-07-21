using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ActiveStartTimeLine6 : MonoBehaviour
{
    public int enemyCount = 0;
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public GameObject boss;
    public PlayableDirector playableDirector;
    [Header("canvas")]
    public GameObject canvasQuest;
    public TMP_Text content;
    NPCDialogueController npcDialogueController;

    public int startTimeLine = 0;
    void Start()
    {
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
        npcDialogueController = FindAnyObjectByType<NPCDialogueController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skip() {
        playableDirector.Stop(); // Dừng timeline nếu đang chạy
        OnTimelineFinished(playableDirector); // Gọi hàm kết thúc timeline để cập nhật trạng thái
    }
    public void Count()
    {
        if(npcDialogueController.currentStage == QuestStage.Quest6InProgress)
        {
            enemyCount++;
            if (enemyCount == startTimeLine)
            {

                // Bắt đầu cutscene
                Debug.Log("Kích hoạt timeline Quest 6");
                playerInGame.SetActive(false); // Ẩn player thật
                timeLine.SetActive(true); // Bật timeline
                playableDirector.Play(); // Chạy timeline
                playableDirector.stopped += OnTimelineFinished; // Đăng ký sự kiện khi timeline kết thúc
            }
        }
        
    }
    private void OnTimelineFinished(PlayableDirector director)
    {
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine.SetActive(false);
        content.text = "Tiêu diệt \" Kẻ khô héo \" ";
        boss.SetActive(true); // Kích hoạt boss sau khi timeline kết thúc
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }
}
