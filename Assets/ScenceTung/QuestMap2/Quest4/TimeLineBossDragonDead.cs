using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.UI;

public class TimeLineBossDragonDead : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject playerInGame; // Player gameplay
    public GameObject playerTimeLine; // Player trong cutscene
    public GameObject boss;
    public PlayableDirector playableDirector;
    public AwardQuest awardQuest; // Tham chiếu đến script AwardQuest
    public GameObject objDestroy;
    public Light _light;
    [Header("canvas")]
    public GameObject canvasQuest;
    public TMP_Text content;
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;
    void Start()
    {
        if (playerInGame == null)
            playerInGame = GameObject.FindWithTag("Player"); // tìm theo tag cho dễ quản lý
        awardQuest = FindAnyObjectByType<AwardQuest>(); // Tìm kiếm đối tượng AwardQuest trong scene
        _light = GameObject.Find("Directional Light(None)").GetComponent<Light>(); // Tìm kiếm ánh sáng trong scene
    }

    public void Run()
    {
             boss.SetActive(false); // Kích hoạt boss sau khi timeline kết thúc
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
        Destroy(objDestroy); // Xóa đối tượng không cần thiết
        StartCoroutine(ChangedWeather()); // Thay đổi thời tiết sau khi timeline kết thúc
    }
    IEnumerator ChangedWeather()
    {
        _light.color = Color.white;
        yield return new WaitForSeconds(0.5f); // Thay đổi màu sắc ánh sáng sau 1 giây
        _light.intensity = 0.5f; // Giảm độ sáng của ánh sáng
        yield return new WaitForSeconds(0.5f); // Chờ thêm 1 giây
        canvasQuest.SetActive(true); // Hiển thị canvas nhiệm vụ
        content.text = "Trở về khu an toàn !"; // Cập nhật nội dung nhiệm vụ
        stateCanvas.SetActive(true); // Hiển thị canvas trạng thái
        stateText.text = "Bạn đã hoàn thành nhiệm vụ !"; // Cập nhật trạng thái nhiệm vụ
        missionName.text = " Tiêu diệt \" Rồng đỏ \" ! " ; // Cập nhật tên nhiệm vụ
        iconState.sprite = spirteState; // Cập nhật biểu tượng trạng thái nhiệm vụ
        yield return new WaitForSeconds(2.5f); // Chờ 1 giây trước khi kết thúc
        stateCanvas.SetActive(false); // Ẩn canvas trạng thái
        awardQuest.AwardQuest2(); // Gọi hàm để thưởng nhiệm vụ 4
        yield return new WaitForSeconds(1f); // Chờ 1 giây trước khi ẩn canvas nhiệm vụ
        gameObject.SetActive(false); // Ẩn đối tượng sau khi timeline kết thúc

    }
}
