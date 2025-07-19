using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject canvasPauser;

    public GameObject equipmentMenu;
    public bool isOpenInventory = true;
    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    public EquippedSlot[] equippedSlot;
    public ItemSO[] itemSOs;
    public AudioSource audioSource;
    public AudioClip selectedClip;
    private PauseManager pausedManager;

    void Update()
    {
        if (pausedManager == null) pausedManager = FindAnyObjectByType<PauseManager>();
        audioSource = GameObject.Find("Inventory").GetComponent<AudioSource>();
        if (Input.GetButtonDown("Inventory") && isOpenInventory )
            Inventory();
        if (Input.GetButtonDown("EquipmentMenu") && isOpenInventory )
            EquipmentMenu();
    }

    public  void Inventory()
    {
        if (inventoryMenu.activeSelf)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryMenu.SetActive(false);
            
            equipmentMenu.SetActive(false);
            pausedManager.canvasPause.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryMenu.SetActive(true);
           
            equipmentMenu.SetActive(false);
            pausedManager.ButtonInven();
            pausedManager.canvasPause.SetActive(true);
        }
    }

    public void EquipmentMenu()
    {
        if (equipmentMenu.activeSelf)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryMenu.SetActive(false);
            pausedManager.canvasPause.SetActive(false);
            equipmentMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryMenu.SetActive(false);
           
            equipmentMenu.SetActive(true);
            pausedManager.canvasPause.SetActive(true);
            pausedManager.ButtonEquipment();
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.ItemQuest)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if (!itemSlot[i].isFull && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
                {
                    int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems > 0)
                        leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
            return quantity;
        }
        else
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
            {
                if (!equipmentSlot[i].isFull && (equipmentSlot[i].itemName == itemName || equipmentSlot[i].quantity == 0))
                {
                    int leftOverItems = equipmentSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    if (leftOverItems > 0)
                        leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
            return quantity;
        }
    }

    public bool HasItem(ItermShopData item, int amount)
    {
        if (item == null || string.IsNullOrEmpty(item.itemName))
        {
            Debug.LogWarning("HasItem được gọi với item null hoặc itemName trống.");
            return false;
        }

        int total = 0;

        if (itemSlot != null)
        {
            foreach (var slot in itemSlot)
            {
                if (slot != null && slot.itemName == item.itemName)
                {
                    total += slot.quantity;
                    if (total >= amount) return true;
                }
            }
        }

        if (equipmentSlot != null)
        {
            foreach (var slot in equipmentSlot)
            {
                if (slot != null && slot.itemName == item.itemName)
                {
                    total += slot.quantity;
                    if (total >= amount) return true;
                }
            }
        }

        return false;
    }


    public void DeselectedAllSLot()
    {
        if (itemSlot != null)
        {
            foreach (var slot in itemSlot)
            {
                if (slot != null)
                {
                    if (slot.selectedItem != null)
                        slot.selectedItem.SetActive(false);

                    slot.isSelected = false;
                }
            }
        }

        if (equipmentSlot != null)
        {
            foreach (var slot in equipmentSlot)
            {
                if (slot != null)
                {
                    if (slot.selectedItem != null)
                        slot.selectedItem.SetActive(false);

                    slot.isSelected = false;

                    if (slot.removeItemButton != null)
                        slot.removeItemButton.SetActive(false);
                }
            }
        }

        if (equippedSlot != null)
        {
            foreach (var slot in equippedSlot)
            {
                if (slot != null)
                {
                    if (slot.selectedItem != null)
                        slot.selectedItem.SetActive(false);

                    slot.isSelected = false;
                }
            }
        }
    }


    public void RemoveItemFromInventory(string itemName)
    {
        foreach (var slot in equipmentSlot)
        {
            if (slot.itemName == itemName)
            {
                slot.EmptySlot();
                return;
            }
        }
        foreach (var slot in itemSlot)
        {
            if (slot.itemName == itemName)
            {
                slot.EmptySlot();
                return;
            }
        }
    }

    public void RemoveItem(ItermShopData item, int amount)
    {
        int remaining = amount;
        foreach (var slot in itemSlot)
        {
            if (slot.itemName == item.itemName && remaining > 0)
            {
                int taken = Mathf.Min(remaining, slot.quantity);
                slot.quantity -= taken;
                if (slot.quantity <= 0)
                    slot.EmptySlot();
                remaining -= taken;
            }
        }
        foreach (var slot in equipmentSlot)
        {
            if (slot.itemName == item.itemName && remaining > 0)
            {
                int taken = Mathf.Min(remaining, slot.quantity);
                slot.quantity -= taken;
                if (slot.quantity <= 0)
                    slot.EmptySlot();
                remaining -= taken;
            }
        }
        Debug.Log($"Removed {amount} x {item.itemName}");
    }

    public int AddItem(EquipmentSO itemSO, int quantity, ItemType type)
    {
        return AddItem(itemSO.itemName, quantity, itemSO.itemSprite, "Equipment", type);
    }

    public bool UseItem(string itemName)
    {
        foreach (var itemSO in itemSOs)
        {
            if (itemSO.itemName == itemName)
            {
                Debug.Log("Check đúng tên item");
                return itemSO.UseItem();
            }
        }
        return false;
    }

    public int GetItemCount(string itemName)
    {
        int total = 0;

        if (string.IsNullOrEmpty(itemName)) return 0;

        foreach (var slot in itemSlot)
        {
            if (slot != null && slot.itemName == itemName)
            {
                total += slot.quantity;
            }
        }

        foreach (var slot in equipmentSlot)
        {
            if (slot != null && slot.itemName == itemName)
            {
                total += slot.quantity;
            }
        }

        return total;
    }

}

public enum ItemType
{
    consumable,
    crafting,
    head,
    body,
    legs,
    feet,
    weapon,
    Accessory1,
    Accessory2,
    Accessory3,
    ItemQuest,
    none
}
