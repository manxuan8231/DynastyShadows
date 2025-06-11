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
    public AudioCanvasState audioCanvasStatequest;
   

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
    }

    IEnumerator ShowQuestCanvas()
    {
        canvasQuest1.SetActive(false); // Show the quest canvas
        yield return new WaitForSeconds(1f);
        stateCanvas.SetActive(true); // Show the state canvas
        stateQuestion.text = "Hoàn thành nhiệm vụ !";
        missionName.text = "Chúc mừng bạn đã hoàn thành nhiệm vụ!!";
        audioCanvasStatequest.PlayDoneQuest(); // Play the quest complete audio
        missionIcon.sprite = missionSprite;
        yield return new WaitForSeconds(4f);
        stateCanvas.SetActive(false); // Hide the state canvas
       
        
    }
}
