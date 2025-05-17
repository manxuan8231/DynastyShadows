using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // Item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    [SerializeField] private int MaxNumberOfItems;

    // UI hiển thị slot
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;
    [SerializeField] public GameObject selectPanel;
    public Sprite defaultImage;
    public Sprite defaultDescriptionImage;

    public bool thisItemSelected;

    // UI mô tả item (gán trên Inspector)
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;


    // liên kết
    private OpenInventory openInventory;
    public ItemSO itemSO;
    public PlayerStatus playerStatus;

    private void Start()
    {
        openInventory = FindAnyObjectByType<OpenInventory>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();

        // Ẩn mọi UI mô tả và nút ngay từ đầu
        ResetDescriptionUI();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemSO itemSO)
    {
        if (isFull)
            return quantity;

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemSO = itemSO;

        itemImage.sprite = itemSprite;
        this.quantity += quantity;

        if (this.quantity >= MaxNumberOfItems)
        {
            isFull = true;
            int extraItems = this.quantity - MaxNumberOfItems;
            this.quantity = MaxNumberOfItems;

            quantityText.text = this.quantity.ToString();
            quantityText.enabled = true;

            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (itemSO == null) return;
        openInventory.DeselectedAllSlots(); // tắt select các slot khác
        selectPanel.SetActive(true);
        thisItemSelected = true;
            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
      
        
       
      
      
    }

    public void OnRightClick()
    {
        //if (itemSO == null || playerStatus == null) return;

        ////switch (itemSO.itemType)
        ////{
        ////    case ItemType.Heal:
        ////        playerStatus.AddHealth(itemSO.value);
        ////        Debug.Log("Healing: " + itemSO.value);
        ////        break;
        ////    case ItemType.Mana:
        ////        playerStatus.AddMana(itemSO.value);
        ////        Debug.Log("Mana: " + itemSO.value);
        ////        break;
        ////}
        //quantity--;
        //if (quantity <= 0)
        //{
        //    ClearSlot();
        //}
        //else
        //{
        //    quantityText.text = quantity.ToString();
        //}
    }

    public void ClearSlot()
    {
        itemName = "";
        itemDescription = "";
        itemSO = null;
        isFull = false;
        itemImage.sprite = defaultImage;
        quantityText.enabled = false;
        selectPanel.SetActive(false);
        ResetDescriptionUI();
    }

    private void ResetDescriptionUI()
    {
        // Ẩn mọi thứ liên quan đến mô tả và nút
        
        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = defaultDescriptionImage;
    }
}
