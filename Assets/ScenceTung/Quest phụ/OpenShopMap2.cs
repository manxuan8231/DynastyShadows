using TMPro;
using UnityEngine;

public class OpenShopMap2 : MonoBehaviour
{
    public GameObject canvasShop;
    public GameObject canvasBtnf;
    public GameObject btnF;
    public TMP_Text textBtnf;
    bool isCanOpen = false;
    public TextMeshProUGUI textGold;
    public float gold;
    private void Start()
    {
        textGold.text = $"{gold}";
    }
    void Update()
    {
        gold = TurnOffOnUI.gold;
        textGold.text = $"{gold}";
      
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
                TurnOffOnUI.openShop = !isShopOpen; // Cập nhật trạng thái cửa hàng trong TurnOffOnUI
                Time.timeScale = isShopOpen ? 1f : 0f; // Dừng thời gian khi mở shop
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
