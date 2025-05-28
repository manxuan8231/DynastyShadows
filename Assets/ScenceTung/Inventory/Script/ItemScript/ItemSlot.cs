using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public GameObject selectedItem;
    public bool isSelected;
    public Sprite emptySprite;
    public ItemType itemType;
  
    // Item Slot
    [SerializeField]
    public TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;
    [SerializeField]
    private int maxNumberOfItems;
     

    // item Description

    public Image itemDescriptionImage;
    public TMP_Text  itemDescriptionText;
    public TMP_Text itemDescriptionNameText;

    //gọi hàm
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";

    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite,string itemDescription,ItemType itemType)
    {

        if (isFull )
            return quantity;
        //update item type
        this.itemType = itemType;
        //update item name
        this.itemName = itemName;
        //update item image
        this.itemSprite = itemSprite;
        itemImage.sprite = itemSprite;
        //update item description
        this.itemDescription = itemDescription;

        //update item quantity
        this.quantity += quantity;
        if (this.quantity >= maxNumberOfItems)
        {
            quantityText.text = maxNumberOfItems.ToString();
            quantityText.enabled = true;
            isFull = true;
            // trả về những món đồ còn lại
            int extraItems = this.quantity - maxNumberOfItems;
            this.quantity = maxNumberOfItems;
            return extraItems;
        }
        

        // cập nhật số lượng
        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;
        Debug.Log("ItemName " + itemName + "Itemquantity " + quantity);
        return 0;

       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }
    private void OnLeftClick()
    {
        bool isAudio = false;
        // Nếu không có item thì không làm gì cả
        if (quantity <= 0 || string.IsNullOrEmpty(itemName))
        {
           

            Debug.Log("Slot này đang trống, không làm gì cả");
            return;
        }
        if (selectedItem && !isAudio)
        {
            inventoryManager.audioSource.PlayOneShot(inventoryManager.selectedClip);
            isAudio = false;
        }
        if (isSelected)
        {
            isAudio = true;

            Debug.Log("Đã chọn trúng tên item " + itemName);
                bool usable = inventoryManager.UseItem(itemName);
                
                if (usable)
                {
                this.quantity -= 1;
                inventoryManager.audioSource.PlayOneShot(inventoryManager.usingClip);
                if (this.quantity <= 0)
                    {

                        EmptySlot();
                    }
                    else
                    {
                        quantityText.text = this.quantity.ToString();
                  
                }
                
            }
            
        }
        else
        {
            inventoryManager.DeselectedAllSLot();
            selectedItem.SetActive(true);
            isSelected = true;

            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite != null ? itemSprite : emptySprite;
        }

    }

    public void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = emptySprite;

        // Reset dữ liệu slot
        itemName = "";
        itemSprite = null;
        itemDescription = "";
        isFull = false;
        quantity = 0;
        itemType = ItemType.none;

        // Reset mô tả và trạng thái chọn
        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;

        selectedItem.SetActive(false);  // Tắt viền chọn (nếu có)
        isSelected = false;
    }

    private void OnRightClick()
    {
        
    }
    

   
  
}
