using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    // Item Data
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public GameObject selectedItem;
    public GameObject removeItemButton; // button xóa item
    public bool isSelected;
    public Sprite emptySprite;
    public ItemType itemType;
    public bool isMarkedForDelete; 


    //equip slot
    [SerializeField]
    private Image itemImage;

    //euipped slot
    [SerializeField]
    private EquippedSlot headSlot, bodySlot, legsSlot, feetSlot, weaponSlot, accessorySlot1, accessorySlot2, accessorySlot3;



    //gọi hàm
    private InventoryManager inventoryManager;
    private EquipmentSOLibrary equipmentSOLibrary;

    private void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("CanvasInventory").GetComponent<EquipmentSOLibrary>();

    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {

        if (isFull)
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
        this.quantity = 1;
        isFull = true;
        return 0;


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        

    }



    private void OnLeftClick()
    {
      
        if (isFull)
        {
            if (isSelected)
            {
                EquipGear();
            }
            else
            {
                inventoryManager.DeselectedAllSLot();
                selectedItem.SetActive(true);
                removeItemButton.SetActive(true);
                isSelected = true;
                for (int i = 0; i < equipmentSOLibrary.EquipmentSOs.Length; i++)
                {
                    if (equipmentSOLibrary.EquipmentSOs[i].itemName == this.itemName)
                        equipmentSOLibrary.EquipmentSOs[i].PreviewEquipment();
                }
            }
        }
        else
        {
            GameObject.Find("Stats").GetComponent<PlayerStatus>().TurnOffPreviewStats();
            
        }
    }
   
    //khi removeitembutton được hiển thị thì thì nếu như click vào button thì sẽ remove item
    public void RemoveItem()
    {
        inventoryManager.RemoveItemFromInventory(itemName);
        EmptySlot();
    }

    private void EquipGear()
    {
       if(itemType == ItemType.head)
        {
            headSlot.EquipGear(itemSprite, itemName, itemDescription);
        }
        if(itemType == ItemType.body)
        {
            bodySlot.EquipGear( itemSprite, itemName, itemDescription);
        }
        if(itemType == ItemType.legs)
        {
            legsSlot.EquipGear(itemSprite, itemName, itemDescription);
        }
        if (itemType == ItemType.feet)
        {
            feetSlot.EquipGear(itemSprite, itemName, itemDescription);
        }
        if (itemType == ItemType.weapon)
        {
            weaponSlot.EquipGear(itemSprite, itemName, itemDescription);
        }
        if (itemType == ItemType.Accessory1)
        {
            accessorySlot1.EquipGear(itemSprite, itemName, itemDescription);
        }
        if (itemType == ItemType.Accessory2)
        {
            accessorySlot2.EquipGear(itemSprite, itemName, itemDescription);
        }
        if (itemType == ItemType.Accessory3)
        {
            accessorySlot3.EquipGear(itemSprite, itemName, itemDescription);
        }
        EmptySlot();
    }

    public void EmptySlot()
    {

        itemImage.sprite = emptySprite;
        itemName = "";
        itemDescription = "";
        quantity = 0;
        isFull = false;
        isSelected = false;
        selectedItem.SetActive(false);
        removeItemButton.SetActive(false);
    }
}
