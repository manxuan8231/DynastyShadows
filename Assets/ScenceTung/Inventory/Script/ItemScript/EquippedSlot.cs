using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour,IPointerClickHandler
{
    //Appearance slot
    [SerializeField]
    private Image slotImage;

    [SerializeField]
    private TMP_Text slotName;
  

 [SerializeField]
 private ItemType itemType = new ItemType();

    private Sprite itemSprite;

    private string itemName;
    private string itemDescription;

    //other variables
    private bool slotIsUse;

    [SerializeField]
    public GameObject selectedItem;
    [SerializeField]
    public bool isSelected;
    [SerializeField]
    private Sprite emptySprite;

    private InventoryManager inventoryManager;
    private void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        
    }
    public void EquipGear(Sprite itemSprite,string itemName,string itemDescription)
    {
        //nếu đã trang bị item mà muốn trang bị item khác thì bỏ trang bị item cũ
        if (slotIsUse)
        {
            UnEquipGearn();
        }
        if (itemSprite == emptySprite || string.IsNullOrEmpty(itemName))
            return; // Không trang bị nếu là rỗng
        //update image
        this.itemSprite = itemSprite;   
        slotImage.sprite = this.itemSprite;
        slotName.enabled = false;


        //update data
        this.itemName = itemName;
        this.itemDescription = itemDescription;

        slotIsUse = true;


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
    private void OnLeftClick()
    {
       if(isSelected && slotIsUse)
        {
            UnEquipGearn();
        }
        else
        {
            inventoryManager.DeselectedAllSLot();
            selectedItem.SetActive(true);
            isSelected = true; 

        }
    }

    public void UnEquipGearn()
    {
        if (!slotIsUse || !isSelected) return; // tránh gọi nhầm

        // Reset state
        inventoryManager.AddItem(itemName, 1, itemSprite, itemDescription, itemType);

        slotImage.sprite = emptySprite;
        itemName = "";
        itemDescription = "";
        isSelected = false;
        selectedItem.SetActive(false);
        slotIsUse = false;
        slotName.enabled = true;
    }


    private void OnRightClick()
    {
        UnEquipGearn();
       
    }

   
}
