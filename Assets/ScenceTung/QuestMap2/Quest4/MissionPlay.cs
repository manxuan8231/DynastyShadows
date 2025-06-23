using System.Collections;
using TMPro;
using UnityEngine;

public class MissionPlay : MonoBehaviour
{
    [Header("Bool")]
    //bool 
    public bool isTimerActive = true; // Biến để kiểm tra xem đếm ngược thời gian có đang hoạt động hay không
    public bool isPlayText = false;
    public bool isQuest4Done = false;

    [Header("Tham chiếu------------")]
    public TeleToMarket teleToMarket; // Tham chiếu đến đối tượng TeleToMarket
    public ChangedWedather changedWedather; // Tham chiếu đến đối tượng ChangedWedather

    [Header("CanvasTextHN")]
    public GameObject canvasTextHN4;
    public TMP_Text textHN;

 



    [Header("Quest")]
    public int countModel = 0; // Biến đếm số lượng mô hình đã hoàn thành
    // Thời gian thực hiện nhiệm vụ
    public GameObject canvasTimerCount;
    public float missionDuration = 10f; // Thời gian thực hiện nhiệm vụ (giây)
    public TMP_Text timerText;
    public GameObject teleToBack;
    

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
    }

    IEnumerator PlayQuestText()
    {
        DialogueControl.isCanvasBusy = true; // Đặt cờ để biết canvas đang bận
        canvasTextHN4.SetActive(true);
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
        canvasTextHN4.SetActive(false); // Ẩn canvas text sau khi hoàn thành
        DialogueControl.isCanvasBusy = false; // Đặt cờ để biết canvas không còn bận nữa
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
        StartCoroutine(DoneQuest()); // Bắt đầu hiển thị thông báo hoàn thành nhiệm vụ
        isQuest4Done = true; // Đặt cờ để biết nhiệm vụ đã hoàn thành
        teleToBack.SetActive(true); // Kích hoạt đối tượng teleToBack
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
    IEnumerator DoneQuest()
    {
        canvasTextHN4.SetActive(true);
        textHN.text = "Không khí ở đây trở nên thoải hơn rồi.";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Bầu trời cùng dần sáng trở lại.Thành công rồi!!";
        yield return new WaitForSeconds(1.5f);
        canvasTextHN4.SetActive(false); // Ẩn canvas text sau khi hoàn thành
        gameObject.SetActive(false); // Ẩn NPC sau khi hoàn thành nhiệm vụ
    }
   
}
public static class DialogueControl
{
    public static bool isCanvasBusy = false;
}