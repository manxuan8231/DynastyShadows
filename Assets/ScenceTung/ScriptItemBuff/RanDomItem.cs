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

    [Header("key")]
    public KeyCase key;
    public bool isStayInRadius = false;
    public GameObject _BtnF;
    [Header("Animation")]
    public Animator animator;
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        key = GameObject.FindWithTag("Player").GetComponent<KeyCase>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(!isStayInRadius)
        {
            return; // Không làm gì nếu không ở trong vùng
        }
        if (isStayInRadius)
        {
            if (Input.GetKeyDown(KeyCode.F) && key.hasKey && key.keyCount > 0 && !key.isKeyOpen && isStayInRadius)
            {
                key.keyCount--;
                key.isKeyOpen = true;
                StartOpenChest();
            }
            else if (Input.GetKeyDown(KeyCode.F) && key.keyCount <= 0)
            {
                key.hasKey = false;
                Debug.Log("Bạn cần có chìa khóa để mở rương này!");
            }
            else
            {
                Debug.Log("Cần chìa khóa để mở rương");
            }
        }
       
    }
    public void StartOpenChest()
    {
        StartCoroutine(DisplayDropSequence());
    }

    private IEnumerator DisplayDropSequence()
    {
        _BtnF.SetActive(false); // Ẩn nút F khi bắt đầu mở rương
        animator.SetTrigger("isOpen"); // Kích hoạt animation mở rương
        yield return new WaitForSeconds(1.5f); // Đợi animation mở rương hoàn thành
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
        animator.SetTrigger("isClosed"); // Kích hoạt animation đóng rương
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
        key.isKeyOpen = false; // Reset key state after opening chest
        if (key.keyCount > 0 && isStayInRadius)
        {
            _BtnF.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStayInRadius = true;
            if(key.isKeyOpen)
            {
                _BtnF.SetActive(false); // Ẩn nút F nếu rương đang mở
            }
            else
                _BtnF.SetActive(true); // Hiển thị nút F khi người chơi vào vùng

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isStayInRadius = false;
            _BtnF.SetActive(false); // Ẩn nút F khi người chơi ra khỏi vùng
        }
    }


}
[System.Serializable]
public class ItemBuffDrop
{
    public Item itemPrefabs;
    [Range(0f, 100f)]
    public float dropRate;
}
