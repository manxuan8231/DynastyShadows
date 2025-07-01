using UnityEngine;
using UnityEngine.Playables;

public class TimeLineBossDragonDead : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public GameObject boss;
    public PlayableDirector playableDirector;
    void Start()
    {
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
    }

    public void Run()
    {
        
            // Bắt đầu cutscene
            Debug.Log("Kích hoạt timeline boss dead");
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
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc
        boss.SetActive(false); // Kích hoạt boss sau khi timeline kết thúc
    }
}
