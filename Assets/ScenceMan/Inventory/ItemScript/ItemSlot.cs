using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerClickHandler
{
    //item data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    [SerializeField]
    private int MaxNumberOfItems;

    //item slot
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;
    [SerializeField]
    public  GameObject selectPanel;
    public bool thisItemSelected;

    //item description
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;


    //gọi hàm
    private OpenInventory openInventory;
    public ItemSO itemSO; // <-- thêm dòng này
    public PlayerStatus playerStatus; // gán qua Inspector hoặc Find


    private void Start()
    {
        openInventory = FindAnyObjectByType<OpenInventory>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        itemSO = FindAnyObjectByType<ItemSO>();
    }
    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemSO itemSO)
    {
        if (isFull)
            return quantity;

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        this.itemSO = itemSO; // <-- gán vào đây
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
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

  
        public void OnRightClick()
    {
        if (itemSO == null || playerStatus == null) return;

        switch (itemSO.itemType)
        {
            case ItemType.Heal:
                playerStatus.AddHealth(itemSO.value);
                Debug.Log("Healing: " + itemSO.value);
                break;
            case ItemType.Mana:
                playerStatus.AddMana(itemSO.value);
                Debug.Log("Mana: " + itemSO.value);
                break;
            default:
                Debug.Log("Unknown item type");
                break;
        }

        quantity--;

        if (quantity <= 0)
        {
            itemName = "";
            itemSprite = null;
            itemDescription = "";
            itemSO = null;
            isFull = false;
            itemImage.sprite = null;
            quantityText.enabled = false;
            selectPanel.SetActive(false);
        }
        else
        {
            quantityText.text = quantity.ToString();
        }
    
}

    public void OnLeftClick()
    {
      
        openInventory.DeselectedAllSlots();
        selectPanel.SetActive(true);
        thisItemSelected = true;
        itemDescriptionText.text = itemDescription;
        itemDescriptionNameText.text = itemName;
        itemDescriptionImage.sprite = itemSprite;
    }
}
