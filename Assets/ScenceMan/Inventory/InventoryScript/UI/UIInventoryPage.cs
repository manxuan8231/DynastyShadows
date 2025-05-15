using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField]
    private UIInventoryItem itemPrebab;

    [SerializeField]
    private RectTransform contentPanel;
    [SerializeField]
    private UIInventoryDescription itemDescription;

    List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();

   [SerializeField]
    private OpenInventory openInventory;


    public Sprite image;
    public int quantity;
    public string title, description;

   
    private void Awake()
    {
        Hide();
        itemDescription.ResetDescription();
       
    }
    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem UIItem = Instantiate(itemPrebab, Vector3.zero,Quaternion.identity);
            UIItem.transform.SetParent(contentPanel);
            listOfUIItems.Add(UIItem);

            UIItem.OnItemClicked += HandleItemSelection;
            UIItem.OnItemBeginDrag += HandleBeginDrag;
            UIItem.OnItemDroppedOn += HandleSwap;
            UIItem.OnItemEndDrag += HandleEndDrag;
            UIItem.OnRightMouseBtnClick += HandleShowItemActions;
        }
       
        if (listOfUIItems.Count > 0)
        {
            listOfUIItems[0].SetData(image, quantity);
        }


    }

    private void HandleShowItemActions(UIInventoryItem item)
    {
        
    }

    private void HandleEndDrag(UIInventoryItem item)
    {
    
    }

    private void HandleSwap(UIInventoryItem item)
    {
        
    }

    private void HandleBeginDrag(UIInventoryItem item)
    {
       
    }

    private void HandleItemSelection(UIInventoryItem item)
    {
        itemDescription.SetDescription(image, title, description);
    }

    public void Show()
    {

        gameObject.SetActive(true);
        itemDescription.ResetDescription();
        openInventory.panelInven.SetActive(true);
        openInventory. panelStatus.SetActive(false);
        openInventory.panelSkill.SetActive(false);
        openInventory.audioSource.PlayOneShot(openInventory.audioClipClick);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
