using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RanDomItem : MonoBehaviour
{
    [Header("Item Drop Settings")]
    public List<ItemBuffDrop> itemDrops = new List<ItemBuffDrop>();

    [Header("Display Settings")]
    public Transform showPoint;
    public float totalShowTime = 10f;
    public float timePerItem = 1f;

    private InventoryManager inventoryManager;
    private GameObject lastShownItem;
    [Header("UI")]
    public GameObject resultPanel;
    public TMP_Text resultText;
    public Image resultImage; // optional
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Press 'O' to open the chest
        {
            StartOpenChest();
        }
    }
    public void StartOpenChest()
    {
        StartCoroutine(DisplayDropSequence());
    }

    private IEnumerator DisplayDropSequence()
    {
        List<Item> itemPool = GenerateItemPool();
        Shuffle(itemPool);

        float elapsed = 0f;
        int index = 0;

        while (elapsed < totalShowTime && index < itemPool.Count)
        {
            ShowItem(itemPool[index]);
            index++;
            elapsed += timePerItem;

            yield return new WaitForSeconds(timePerItem);
        }

        // Đợi 1 chút và nhận thưởng
        yield return new WaitForSeconds(0.5f);

        if (lastShownItem != null)
        {
            Item finalItem = lastShownItem.GetComponent<Item>();
            inventoryManager.AddItem(
                finalItem.itemName,
                finalItem.quantity,
                finalItem.itemSprite,
                finalItem.itemDescription,
                finalItem.itemType
            );
            ShowItemResult(finalItem);
            Destroy(lastShownItem);
            Debug.Log($"🎉 You received: {finalItem.itemName}");
        }
    }

    private List<Item> GenerateItemPool()
    {
        List<Item> pool = new List<Item>();

        foreach (var drop in itemDrops)
        {
            int count = Mathf.RoundToInt(drop.dropRate / 10f);
            for (int i = 0; i < count; i++)
            {
                pool.Add(drop.itemPrefabs);
            }
        }

        return pool;
    }

    private void ShowItem(Item itemData)
    {
        if (lastShownItem != null)
            Destroy(lastShownItem);

        lastShownItem = Instantiate(itemData.gameObject, showPoint.position, Quaternion.identity);
    }

    private void Shuffle(List<Item> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
    private void ShowItemResult(Item item)
    {
        resultPanel.SetActive(true);
        resultText.text = $" Bạn đã nhận được: <b>{item.itemName}</b>";

        if (resultImage != null)
        {
            resultImage.sprite = item.itemSprite;
            resultImage.enabled = true;
        }

        // Tự tắt sau vài giây
        StartCoroutine(HideResultPanelAfterDelay(3f));
    }

    private IEnumerator HideResultPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultPanel.SetActive(false);
    }
}
[System.Serializable]
public class ItemBuffDrop
{
    public Item itemPrefabs;
    [Range(0f, 100f)]
    public float dropRate;
}
