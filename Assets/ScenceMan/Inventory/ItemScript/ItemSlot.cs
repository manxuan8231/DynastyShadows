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
    private void Start()
    {
        openInventory = FindAnyObjectByType<OpenInventory>();
    }
    public void AddItem(string itemName, int quantity, Sprite itemSprite,string itemDescription)
    {
     this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        isFull = true;
        
        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
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
