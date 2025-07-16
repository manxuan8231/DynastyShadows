using TMPro;
using UnityEngine;

public class OpenShopMap2 : MonoBehaviour
{
    public GameObject canvasShop;
    public GameObject canvasBtnf;
    public GameObject btnF;
    public TMP_Text textBtnf;
    bool isCanOpen = false;

    void Update()
    {
        if (isCanOpen)
        {
            if (canvasShop.activeSelf)
            {
                btnF.SetActive(false);
                ShowCursor(true); // Hiện con trỏ khi shop mở
            }
            else
            {
                canvasBtnf.SetActive(true);
                btnF.SetActive(true);
                ShowCursor(false); // Ẩn con trỏ khi shop đóng
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                bool isShopOpen = canvasShop.activeSelf;
                canvasShop.SetActive(!isShopOpen);
                btnF.SetActive(false);
                textBtnf.text = "F:Mở Shop";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCanOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isCanOpen = false;
            canvasBtnf.SetActive(false);
            btnF.SetActive(false);
            canvasShop.SetActive(false);
            ShowCursor(false); // Ẩn chuột khi rời khỏi shop
            textBtnf.text = "F:Nói chuyện"; // Reset text khi rời khỏi vùng kích hoạt
        }
    }

    void ShowCursor(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
