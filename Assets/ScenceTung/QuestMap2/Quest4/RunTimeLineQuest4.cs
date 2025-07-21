using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class RunTimeLineQuest4 : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public GameObject boss;
    public int activeTimeLine = 0; // Biến để xác định xem timeline có được kích hoạt hay không
    public PlayableDirector playableDirector;
    [Header("canvas")]
    public GameObject canvasQuest;
    public TMP_Text content;
    void Start()
    {
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
    }

    public void Active()
    {
        activeTimeLine++;
        if (activeTimeLine == 3)
        {
            // Bắt đầu cutscene
            Debug.Log("Kích hoạt timeline Quest 4");
            playerInGame.SetActive(false); // Ẩn player thật
            timeLine.SetActive(true); // Bật timeline
            playableDirector.Play(); // Chạy timeline
            playableDirector.stopped += OnTimelineFinished; // Đăng ký sự kiện khi timeline kết thúc

        }
    }
    public void Skip()
    {
        playableDirector.Stop(); // Dừng timeline nếu đang chạy
        OnTimelineFinished(playableDirector); // Gọi hàm kết thúc timeline để cập nhật trạng thái
    }
    private void OnTimelineFinished(PlayableDirector director)
    {
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine.SetActive(false);
        content.text = "Tiêu diệt \" Rồng đỏ \" ";
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc
        boss.SetActive(true); // Kích hoạt boss sau khi timeline kết thúc
    }
}
