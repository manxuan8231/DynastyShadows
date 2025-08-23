using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ShopTransactionNotifier : MonoBehaviour
{
    [Header("UI References")]
    public GameObject popupPanel;         // Panel hiển thị popup mua/bán
    public TextMeshProUGUI popupText;     // Text hiển thị chi tiết
    public Image popupIcon;               // Ảnh minh họa item

    [Header("Settings")]
    public float resetInterval = 5f;      // Thời gian reset về 0

    private string currentItemName = "";
    private int currentQuantity = 0;
    private Sprite currentIcon;
    private bool currentModeIsBuying = true;

    private Coroutine resetCoroutine;

    void Start()
    {
        popupPanel.SetActive(false);
    }

    /// Hiện thông báo khi mua hoặc bán item
    public void ShowTransaction(string itemName, int quantity, Sprite icon, bool isBuying)
    {
        // Nếu item khác hoặc mode khác -> reset lại
        if (currentItemName != itemName || currentModeIsBuying != isBuying)
        {
            ResetNow();
            currentItemName = itemName;
            currentQuantity = 0;
            currentIcon = icon;
            currentModeIsBuying = isBuying;
        }

        // Cộng dồn số lượng
        currentQuantity += quantity;

        ShowPopup();

        // Reset sau 1 khoảng thời gian
        if (resetCoroutine != null) StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ResetAfterDelay());
    }


    void ShowPopup()
    {
        popupPanel.SetActive(true);

        string mode = currentModeIsBuying ? "Mua" : "Bán";
        popupText.text = $"{mode} {currentQuantity} {currentItemName}";

        if (popupIcon != null && currentIcon != null)
            popupIcon.sprite = currentIcon;
    }

   
    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSecondsRealtime(resetInterval); 
        ResetNow();
    }

    private void ResetNow()
    {
        currentItemName = "";
        currentQuantity = 0;
        currentIcon = null;

        popupPanel.SetActive(false);
        popupText.text = "";
    }

    private void OnDisable()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
        ResetNow();
    }
}
