    using UnityEngine;
using UnityEngine.Playables;

public class TimeLineBoss : MonoBehaviour
{
    public GameObject playerInGame;      // Player gameplay
    public GameObject playerTimeLine;    // Player chạy trong cutscene
    public PlayableDirector playableDirector;

    private void Start()
    {
        // Disable player thật để chỉ thấy player trong timeline
        playerInGame.SetActive(false);
        playerTimeLine.SetActive(true);

        // Bắt sự kiện khi timeline kết thúc
        playableDirector.stopped += OnTimelineFinished;

        // Bắt đầu chạy timeline (nếu chưa chạy auto)
        playableDirector.Play();
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        // Set lại vị trí và hướng của player thật bằng player trong timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;

        // Bật lại player thật để chơi tiếp
        playerInGame.SetActive(true);

        // Ẩn player timeline nếu không cần dùng nữa
        playerTimeLine.SetActive(false);
    }

    private void OnDestroy()
    {
        playableDirector.stopped -= OnTimelineFinished;
    }
}

