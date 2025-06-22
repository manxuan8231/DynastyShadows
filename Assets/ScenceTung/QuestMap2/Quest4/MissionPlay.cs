using System.Collections;
using TMPro;
using UnityEngine;

public class MissionPlay : MonoBehaviour
{
    [Header("Bool")]
    //bool 
    public bool isTimerActive = true; // Biến để kiểm tra xem đếm ngược thời gian có đang hoạt động hay không
    public bool isPlayText = false;

    [Header("Tham chiếu------------")]
    public TeleToMarket teleToMarket; // Tham chiếu đến đối tượng TeleToMarket
    public ChangedWedather changedWedather; // Tham chiếu đến đối tượng ChangedWedather

    [Header("CanvasTextHN")]
    public GameObject canvasTextHN;
    public TMP_Text textHN;

    [Header("Quest")]
    public int countModel = 0; // Biến đếm số lượng mô hình đã hoàn thành
    // Thời gian thực hiện nhiệm vụ
    public GameObject canvasTimerCount;
    public float missionDuration = 10f; // Thời gian thực hiện nhiệm vụ (giây)
    public TMP_Text timerText;
    

    void Start()
    {
        canvasTimerCount.SetActive(false); // Ẩn canvas text khi bắt đầu
        if (teleToMarket == null)
        {
            teleToMarket = FindAnyObjectByType<TeleToMarket>(); // Tìm đối tượng TeleToMarket trong scene
        }
        if (changedWedather == null)
        {
            changedWedather = FindAnyObjectByType<ChangedWedather>(); // Tìm đối tượng ChangedWedather trong scene
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(teleToMarket.isTeleDone && !isPlayText)
        {
            isPlayText = true;
            StartCoroutine(PlayQuestText());
        }
        if (Input.GetKeyDown(KeyCode.V)){
            StartCoroutine(CountDownTimer());
        }
        {
            
        }

    }

    IEnumerator PlayQuestText()
    {
        canvasTextHN.SetActive(true);
        textHN.text = "Nghe nói khu chợ thường rất nhộn nhịp.";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Nhưng nơi này thì có vẻ không giống như thế cho lắm.";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Chắc là do cơ quan của phong ấn đã kích hoạt nên bầu không khí đen tối, ngột ngạt thế này.";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Nó làm mình có cảm giác khó chịu và bất an.";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Mình cần phải tìm cách giải quyết vấn đề này.";
        yield return new WaitForSeconds(1f);
        canvasTextHN.SetActive(false); // Ẩn canvas text sau khi hoàn thành
        isPlayText = true; // Đặt cờ để biết nhiệm vụ đã hoàn thành
        yield return new WaitForSeconds(1f);
        StartCoroutine(CountDownTimer()); // Bắt đầu đếm ngược thời gian

    }
    public void UpCount()
    {
        countModel++;
        if (countModel >= 3)
        {
            SuccessQuest();
        }
        
    }

    public void SuccessQuest()
    {
        isTimerActive = false; // Dừng đếm ngược thời gian khi nhiệm vụ thành công
        StopCoroutine(CountDownTimer()); // Dừng đếm ngược thời gian nếu nhiệm vụ thành công
        canvasTimerCount.SetActive(false); // Ẩn canvas đếm ngược
        changedWedather.ChangedFirstWeather();
    }


    public IEnumerator CountDownTimer()
    {
        
        canvasTimerCount.SetActive(true); // Hiển thị canvas đếm ngược
        float timeRemaining = missionDuration;
        while (timeRemaining > 0 && isTimerActive)
        {
            timerText.text = "Thời gian còn lại: " + Mathf.Ceil(timeRemaining) + " giây";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
        timerText.text = "Thời gian đã hết!";
        // Thực hiện hành động khi thời gian kết thúc
        canvasTimerCount.SetActive(false); // Ẩn canvas đếm ngược
        if(timeRemaining == 0)
        {
           PlayerStatus playerStatus = FindAnyObjectByType<PlayerStatus>();
           playerStatus.currentHp = 0; // Giảm máu của người chơi về 0
            playerStatus.sliderHp.value = playerStatus.currentHp;
            PlayerControllerState playerController = FindAnyObjectByType<PlayerControllerState>();
           if(playerStatus.currentHp == 0) 
            {
                playerController.ChangeState(new PlayerDieState(playerController)); // Chuyển sang trạng thái chết
            }
        }
    }
}
