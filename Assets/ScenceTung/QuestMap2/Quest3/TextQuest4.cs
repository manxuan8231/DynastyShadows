using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextQuest4 : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasTextQuest4;
    public TMP_Text contentText;
    public GameObject canvasContentQuest;
    public TMP_Text contentTextQuest;
    [Header("Trạng thái nhiệm vụ")]
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;
    public Sprite spirteState;

    [Header("Tham chiếu")]
    public TimeLineQuest3 TimeLineQuest3;
    public AudioCanvasState audioCanvasState;

    [Header("Gameobject")]
    public GameObject destroy;
    public GameObject questpoint;
    bool isTextQuest4 = false;
        private void Start()
    {
        questpoint.SetActive(false); // Đảm bảo questpoint không hiển thị ban đầu
    }
    private void Update()
    {
        if(TimeLineQuest3.isQuest3Complete == true && !isTextQuest4)
        {
            isTextQuest4 = true; // Đánh dấu là đã hiển thị text quest 4
            StartCoroutine(TextHN());
            Destroy(destroy); // Hủy đối tượng destroy nếu cần thiết
        }
    }
  

    IEnumerator TextHN()
    {
        yield return new WaitForSeconds(1f); // Đợi một chút trước khi hiển thị canvas
        canvasTextQuest4.SetActive(true);
        contentText.text = "Tại sao mọi người lại truy đuổi mình ?";
        yield return new WaitForSeconds(2f);
        contentText.text = "Họ như bị ai đó điều khiển vậy ?";
        yield return new WaitForSeconds(2f);
        contentText.text = "Có vẻ nên tìm ông Trưởng mục Lương để tìm hiểu thêm chút thông tin.";
        yield return new WaitForSeconds(2f);
        canvasTextQuest4.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        canvasContentQuest.SetActive(true);
        contentTextQuest.text = "Đi tìm Trưởng mục Lương.";
        stateCanvas.SetActive(true);                                
        audioCanvasState.PlayNewQuest();
        stateText.text = "Bạn vừa nhận được nhiệm vụ mới";
        missionName.text = "Đi tìm Trưởng mục Lương";
        iconState.sprite = spirteState;
        yield return new WaitForSeconds(2f);
        questpoint.SetActive(true); // Kích hoạt questpoint
        stateCanvas.SetActive(false);
        TextQuest4 text = FindAnyObjectByType<TextQuest4>();
        text.enabled = false;


    }
}
