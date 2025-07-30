using UnityEngine;
using UnityEngine.Playables;

public class TimeLineQuest3 : MonoBehaviour
{
    [Header("Player References")]
    public GameObject playerInGame;      // Player gameplay
    public GameObject playerTimeLine;    // Player trong cutscene
    public GameObject trigger;
    public GameObject destroy;
    [Header("Timeline")]
    public PlayableDirector playableDirector;
    public bool isQuest3Complete = false; // Biến này có thể dùng để kiểm tra trạng thái quest

    private bool hasPlayed = false; // Ngăn cho trigger chạy nhiều lần
  

    private void Start()
    {
        if (isQuest3Complete) return;
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý

        if (playableDirector == null)
            playableDirector = GetComponent<PlayableDirector>();

        playerTimeLine.SetActive(false);
        playableDirector.stopped += OnTimelineFinished;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Cursor.lockState = CursorLockMode.None; // Mở khóa con trỏ chuột
            Cursor.visible = true; // Hiển thị con trỏ chuột
        }
    }
    public void Skip()
    {
        playableDirector.Stop(); // Dừng timeline nếu đang chạy
        OnTimelineFinished(playableDirector); // Gọi hàm kết thúc timeline để cập nhật trạng thái
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isQuest3Complete) return;
        if (hasPlayed) return;

        if (other.CompareTag("Player"))
        {
            hasPlayed = true;

            // Bắt đầu cutscene
            trigger.SetActive(true);           // Tắt trigger để không chạy lại
            playerInGame.SetActive(false);      // Ẩn player thật
            playerTimeLine.SetActive(true);     // Bật player giả

            playableDirector.Play();            // Play Timeline
            
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
    {
        // Cập nhật vị trí player thật từ player timeline
        playerInGame.transform.position = playerTimeLine.transform.position;
        playerInGame.transform.rotation = playerTimeLine.transform.rotation;
        // Đặt lại trạng thái con trỏ chuột
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Ẩn con trỏ chuột khi trở lại gameplay
        // Kết thúc cutscene, chơi tiếp
        playerInGame.SetActive(true);
        playerTimeLine.SetActive(false);
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc
        isQuest3Complete = true; // Đánh dấu quest hoàn thành      
        GameSaveData data = SaveManagerMan.LoadGame(); // Lấy dữ liệu game đã lưu
        data.dataQuest.isQuest3Map2 = isQuest3Complete; // Cập nhật trạng thái quest
        SaveManagerMan.SaveGame(data); // Lưu lại dữ liệu game
        Destroy(destroy); // Hủy đối tượng destroy nếu cần thiết
    }

    private void OnDestroy()
    {
        if (playableDirector != null)
            playableDirector.stopped -= OnTimelineFinished;
        // Đảm bảo hủy sự kiện khi đối tượng bị hủy
    }
}
