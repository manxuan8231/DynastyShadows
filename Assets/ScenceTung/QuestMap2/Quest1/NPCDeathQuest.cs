using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDeathQuest : MonoBehaviour
{
    public GameObject diaryModel;
    public GameObject diaryContent;
    public GameObject btnPressF;
    public TMP_Text content;
    public GameObject canvasQuest1;
    public GameObject stateCanvas;
    public TMP_Text stateQuestion;
    public TMP_Text missionName;
    public Image missionIcon;
    public Sprite missionSprite;
    public Sprite newQuest;
    public AudioCanvasState audioCanvasStatequest;
    public TMP_Text contentQuest;
    public bool isQuest1 = false;
    public GameObject canvasText;
    public TMP_Text contentText;
    public GameObject quest2;
    public GameObject quest1;
    public GameObject transformQuest;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && diaryModel.activeSelf )
        {
            btnPressF.SetActive(true);
           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            btnPressF.SetActive(false);
            
        }
    }
    private void Update()
    {
        if (isQuest1)
        {
            quest1.SetActive(false); // Hide the quest 1
            quest2.SetActive(true); // Show the quest 2
        }
        else
        {
            quest2.SetActive(false); // Hide the quest 2
        }
        if (Input.GetKeyDown(KeyCode.F) && btnPressF.activeSelf)
        {
            btnPressF.SetActive(false);
            diaryContent.SetActive(true);
            if (diaryContent.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Show the cursor
                Time.timeScale = 0f; // Pause the game time

                content.text = "Vào chiều hôm qua chúng tôi đã thay ca canh gác cho đội trước\r" +
                    "\nđột nhiên nghe thấy một âm thanh kì lạ và rồi cánh cửa bị phá một lổ lớn\r" +
                    "\nbọn quái vật từ đâu xông vào rất đông và các đội gần đó đã chi viện để chống lại.\r" +
                    "\ntrong đám đó đột nhiên có một con rất khác lạ và nó đã hạ hết các đội của chúng tôi\r" +
                    "\nkhông còn nhiều thời gian, tôi nghĩ mình không cầm cự được nữa, nó …..\r\n";
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
                Cursor.visible = false; // Hide the cursor
            }
          
        }
    }
    public void closeDiary()
    {
        Time.timeScale = 1f; // Resume the game time
        diaryContent.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        diaryModel.SetActive(false);
        StartCoroutine(ShowQuestCanvas()); // Start the coroutine to show the quest canvas
       transformQuest.SetActive(false); // Deactivate the transform quest object
    }

    IEnumerator ShowQuestCanvas()
    {
        canvasText.SetActive(true); // Show the canvas text
        contentText.text = "Chuyện này thật tệ, mình cần xem xung quanh khu này còn ai không!?";
        yield return new WaitForSeconds(2f);
        contentText.text = "Có vẽ ở đây có chuyện chẳng lành rồi...";
        yield return new WaitForSeconds(2f);
        contentText.text = "Mình cần phải tìm hiểu xem chuyện gì đã xảy ra ở đây!";
        yield return new WaitForSeconds(1.5f); // Wait for 3 seconds
        canvasText.SetActive(false); // Hide the canvas text
        stateCanvas.SetActive(true); // Show the state canvas
        stateQuestion.text = "Hoàn thành nhiệm vụ !";
        missionName.text = "Chúc mừng bạn đã hoàn thành nhiệm vụ!!";
        audioCanvasStatequest.PlayDoneQuest(); // Play the quest complete audio
        missionIcon.sprite = missionSprite;
        yield return new WaitForSeconds(2f);
        stateCanvas.SetActive(false); // Hide the state canvas
        yield return new WaitForSeconds(1.5f);
        stateCanvas.SetActive(true); // Show the state canvas
        stateQuestion.text = "Bạn vừa nhận được nhiệm vụ mới!";
        missionName.text = "Tìm kiếm sự sống xung quanh thị trấn.";
        audioCanvasStatequest.PlayNewQuest(); // Play the quest complete audio
        missionIcon.sprite = newQuest;
        yield return new WaitForSeconds(1.5f);
        stateCanvas.SetActive(false);
        canvasQuest1.SetActive(true); // Show the quest canvas
        contentQuest.text = "Nhiệm vụ: Tìm kiếm sự sống xung quanh thị trấn.";
        isQuest1 = true; // Set the quest flag to true
        transformQuest.SetActive(true); // Activate the transform quest object 
    }
}
