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
                content.text = "V�o chi?u h�m qua ch�ng t�i ?� thay ca canh g�c cho ??i tr??c\r" +
                    "\n??t nhi�n nghe th?y m?t �m thanh k? l? v� r?i c�nh c?a b? ph� m?t l? l?n\r" +
                    "\nb?n qu�i v?t ? ?�u x�ng v�o r?t ?�ng v� c�c ??i g?n ?� ?� chi vi?n ?? ch?ng l?i.\r" +
                    "\ntrong ?�m ?� ??t nhi�n c� m?t con r?t kh�c l? v� n� ?� h? h?t c�c ??i c?a ch�ng t�i\r" +
                    "\nkh�ng c�n nhi?u th?i gian, t�i ngh? m�nh kh�ng c?m c? ???c n?a, n� �..\r\n";
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
