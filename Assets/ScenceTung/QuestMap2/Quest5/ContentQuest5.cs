using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentQuest5 : MonoBehaviour
{
    [Header("CanvasTextHN")]
    public GameObject canvasTextHN5; // Canvas hiển thị thông báo
    public TMP_Text textHN; // Text hiển thị thông báo


    [Header("CanvasQuest")]
    public GameObject canvasQuest; // Canvas hiển thị thông báo nhiệm vụ
    public TMP_Text textQuest; // Text hiển thị thông báo nhiệm vụ
    public Image iconQuest; // Hình ảnh biểu tượng nhiệm vụ


    [Header("CanvasSuscessQuest")]
    public GameObject canvasSuscessQuest; // Canvas hiển thị thông báo hoàn thành nhiệm vụ
    public TMP_Text stateText; // Text hiển thị thông báo hoàn thành nhiệm vụ
    public TMP_Text missionText;
    public Image iconSuscessQuest; // Hình ảnh biểu tượng nhiệm vụ hoàn thành
    public Sprite iconQuest4    ; // Biểu tượng nhiệm vụ 4


    [Header("Tham chiếu")]
    public MissionPlay MissionPlay;


    [Header("Bool")]
    public bool isQuest5TextOpen = false;


    void Start()
    {
        MissionPlay = FindAnyObjectByType<MissionPlay>(); // Tìm đối tượng MissionPlay trong scene
    }

    // Update is called once per frame
    void Update()
    {
        if (MissionPlay.isQuest4Done == false) return;
        if(MissionPlay.isQuest4Done && !isQuest5TextOpen)
        {
            StartCoroutine(ContentTextQuest5()); // Bắt đầu coroutine để hiển thị nội dung nhiệm vụ 5
        }
    }

    IEnumerator ContentTextQuest5()
    {
        isQuest5TextOpen = true;
        yield return new WaitForSeconds(10f);
        while (DialogueControl.isCanvasBusy) // Đợi nhiệm vụ 4 nói xong
            yield return null;
        DialogueControl.isCanvasBusy = true; // Đặt cờ để biết canvas đang bận
        yield return new WaitForSeconds(3f);
        canvasTextHN5.SetActive(true);
        textHN.text = "Tinh linh... vật tế cổ... Tất cả manh mối đều quy về quá vãng xa xưa.";
        yield return new WaitForSeconds(3f);
        textHN.text = "Muốn thấu tỏ chân tướng, chỉ e phải dò hỏi nơi tàng thư. Chốn ấy cất giữ bao cổ văn, ắt có điều chưa từng hé lộ.";
        yield return new WaitForSeconds(3f);
        canvasTextHN5.SetActive(false);
        DialogueControl.isCanvasBusy = false; // Đặt cờ để biết canvas không còn bận nữa
        yield return new WaitForSeconds(1f);
        canvasQuest.SetActive(false);
        yield return new WaitForSeconds(2f);
        canvasSuscessQuest.SetActive(true); // Hiển thị canvas thông báo hoàn thành nhiệm vụ
        stateText.text = "Nhiệm vụ hoàn thành !!!";
        missionText.text = "Nhiệm vụ: Tìm kiếm tàng thư!?.";
        iconSuscessQuest.sprite = iconQuest4; // Cập nhật biểu tượng nhiệm vụ hoàn thành
        canvasQuest.SetActive(true);
        textQuest.text = "Nhiệm vụ: Tìm đến tàng thư các trong trấn, tra xét về thứ gọi là \"Vật tế cổ\".";
        iconQuest.sprite = iconQuest4;
        yield return new WaitForSeconds(3f);
        canvasSuscessQuest.SetActive(false); // Ẩn canvas thông báo hoàn thành nhiệm vụ


    }
}
