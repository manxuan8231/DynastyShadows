using System;
using TMPro;
using UnityEngine;
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
  
    // Item Slot
    [SerializeField]
    private TMP_Text quantityText;

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
        
    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite,string itemDescription)
    {

        if (isFull )
            return quantity;
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
        inventoryManager.DeselectedAllSLot();   
        selectedItem.SetActive(true);
        isSelected = true;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
        itemDescriptionImage.sprite = itemSprite;
        if(itemDescriptionImage.sprite == null)
        {
            itemDescriptionImage.sprite = emptySprite;
        }
    }
    private void OnRightClick()
    {
        
    }
    

   
  
}
