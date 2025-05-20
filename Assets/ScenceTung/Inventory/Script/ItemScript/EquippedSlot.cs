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
    public void EquipGear(string itemName, Sprite itemSprite,string itemDescription)
    {
        //nếu đã trang bị item mà muốn trang bị item khác thì bỏ trang bị item cũ
        if (slotIsUse)
        {
            UnEquipGearn();
        }



        //update image
        this.itemSprite = itemSprite;   
        slotImage.sprite = itemSprite;
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
        inventoryManager.DeselectedAllSLot();
        inventoryManager.AddItem(itemName,1 ,itemSprite,itemDescription,itemType);
        this.itemSprite = emptySprite;
        slotImage.sprite = this.emptySprite;
        slotName.enabled = true;

    }

    private void OnRightClick()
    {
        UnEquipGearn();
       
    }

   
}
