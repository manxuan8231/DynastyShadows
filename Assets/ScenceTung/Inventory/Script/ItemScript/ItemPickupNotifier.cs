using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ItemPickupNotifier : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;
    public TextMeshProUGUI normalPickupText;

    private Dictionary<string, int> pickupDict = new Dictionary<string, int>();
    private int pickupCount = 0;
    private int pickupThreshold = 5;
    private float pickupInterval = 3f;

    private Coroutine resetCoroutine;
    private Coroutine quickTextCoroutine;

    void Start()
    {
        popupPanel.SetActive(false);
        normalPickupText.text = "";
    }

    public void ShowPickup(string itemName, int quantity)
    {
        pickupCount++;

        // Gom item lại theo tên
        if (pickupDict.ContainsKey(itemName))
            pickupDict[itemName] += quantity;
        else
            pickupDict[itemName] = quantity;

        string message = $"+{quantity} {itemName}";

        if (pickupCount > pickupThreshold)
        {
            ShowPopup();
        }
        else
        {
            if (quickTextCoroutine != null) StopCoroutine(quickTextCoroutine);
            quickTextCoroutine = StartCoroutine(ShowQuickText(message));
        }

        if (resetCoroutine != null)
            StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ResetPickupCountAfterDelay());
    }

    IEnumerator ShowQuickText(string message)
    {
        normalPickupText.text = message;
        yield return new WaitForSeconds(1.5f);
        normalPickupText.text = "";
        quickTextCoroutine = null;
    }

    void ShowPopup()
    {
        popupPanel.SetActive(true);

        // Dừng và ẩn Quick Text
        if (quickTextCoroutine != null)
        {
            StopCoroutine(quickTextCoroutine);
            quickTextCoroutine = null;
        }
        normalPickupText.text = ""; // <- Tắt dòng vàng ở giữa

        string allItems = "";

        foreach (var kvp in pickupDict)
        {
            allItems += $"+{kvp.Value} {kvp.Key}\n";
        }

        popupText.text = allItems;
    }

    IEnumerator ResetPickupCountAfterDelay()
    {
        yield return new WaitForSeconds(pickupInterval);
        pickupCount = 0;
        pickupDict.Clear();
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
        if (quickTextCoroutine != null)
        {
            StopCoroutine(quickTextCoroutine);
            quickTextCoroutine = null;
        }
        popupPanel.SetActive(false);
        normalPickupText.text = "";
        popupText.text = "";
        pickupDict.Clear();
        pickupCount = 0;
    }
}
