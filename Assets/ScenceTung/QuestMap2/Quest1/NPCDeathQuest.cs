using TMPro;
using UnityEngine;

public class NPCDeathQuest : MonoBehaviour
{
    public GameObject diaryModel;
    public GameObject diaryContent;
    public GameObject btnPressF;
    public TMP_Text content;

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
    }

}
