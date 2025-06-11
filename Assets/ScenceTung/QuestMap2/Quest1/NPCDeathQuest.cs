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
                content.text = "Vào chi?u hôm qua chúng tôi ?ã thay ca canh gác cho ??i tr??c\r" +
                    "\n??t nhiên nghe th?y m?t âm thanh k? l? và r?i cánh c?a b? phá m?t l? l?n\r" +
                    "\nb?n quái v?t ? ?âu xông vào r?t ?ông và các ??i g?n ?ó ?ã chi vi?n ?? ch?ng l?i.\r" +
                    "\ntrong ?ám ?ó ??t nhiên có m?t con r?t khác l? và nó ?ã h? h?t các ??i c?a chúng tôi\r" +
                    "\nkhông còn nhi?u th?i gian, tôi ngh? mình không c?m c? ???c n?a, nó …..\r\n";
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
        diaryContent.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
        diaryModel.SetActive(false);
    }

}
