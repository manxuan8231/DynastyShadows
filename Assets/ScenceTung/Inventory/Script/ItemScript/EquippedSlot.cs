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
    private EquipmentSOLibrary equipmentSOLibrary;
    private void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("CanvasInventory").GetComponent<EquipmentSOLibrary>();
       
    }
    public void EquipGear(Sprite newItemSprite, string newItemName, string newItemDescription)
    {
        // Nếu đã có trang bị thì bỏ trang bị cũ trước (và nhớ giữ lại data cũ)
        if (slotIsUse)
        {
            string oldItemName = itemName;
            string oldItemDescription = itemDescription;
            Sprite oldItemSprite = itemSprite;

            // Bỏ trang bị cũ
            for (int i = 0; i < equipmentSOLibrary.EquipmentSOs.Length; i++)
            {
                if (equipmentSOLibrary.EquipmentSOs[i].itemName == oldItemName)
                {
                    equipmentSOLibrary.EquipmentSOs[i].UnEquipItem();
                    break;
                }
            }

            inventoryManager.AddItem(oldItemName, 1, oldItemSprite, oldItemDescription, itemType);
        }

        if (newItemSprite == emptySprite || string.IsNullOrEmpty(newItemName))
            return; // Không trang bị nếu là rỗng

        // update image & data
        this.itemSprite = newItemSprite;
        slotImage.sprite = this.itemSprite;
        slotName.enabled = false;
        this.itemName = newItemName;
        this.itemDescription = newItemDescription;

        for (int i = 0; i < equipmentSOLibrary.EquipmentSOs.Length; i++)
        {
            if (equipmentSOLibrary.EquipmentSOs[i].itemName == newItemName)
            {
                equipmentSOLibrary.EquipmentSOs[i].EquipItem();
                break;
            }
        }

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
            for (int i = 0; i < equipmentSOLibrary.EquipmentSOs.Length; i++)
            {
                if (equipmentSOLibrary.EquipmentSOs[i].itemName == this.itemName)
                    equipmentSOLibrary.EquipmentSOs[i].PreviewEquipment();
            }

        }
    }

    public void UnEquipGearn()
    {
        if (!slotIsUse || !isSelected) return; // tránh gọi nhầm
        for (int i = 0; i < equipmentSOLibrary.EquipmentSOs.Length; i++)
        {
            if (equipmentSOLibrary.EquipmentSOs[i].itemName == itemName)
            {
                equipmentSOLibrary.EquipmentSOs[i].UnEquipItem();
                Debug.Log("UnEquip item: " + itemName);
                break;
            }
        }
        GameObject.Find("Stats").GetComponent<PlayerStatus>().TurnOffPreviewStats();
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
