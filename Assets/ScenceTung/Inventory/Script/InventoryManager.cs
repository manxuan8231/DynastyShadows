using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject inventoryLogo;


    public GameObject equipmentMenu;
    public bool isOpenInventory = true;
    //gọi hàm
    public ItemSlot[] itemSlot; // Array of item slots
    public EquipmentSlot[] equipmentSlot; // Array of equipment slots
    public EquippedSlot[] equippedSlot; // Array of equipped slots
    public ItemSO[] itemSOs;
    public AudioSource audioSource;
    public AudioClip selectedClip;

  
    void Update()
    {
        audioSource = GameObject.Find("Inventory").GetComponent<AudioSource>();
        if (Input.GetButtonDown("Inventory") && isOpenInventory)
            Inventory();
        if (Input.GetButtonDown("EquipmentMenu") && isOpenInventory)
            EquipmentMenu();

    }
    void Inventory()
    {
        if (inventoryMenu.activeSelf)
        {
            Time.timeScale = 1f; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
            inventoryMenu.SetActive(false);
            inventoryLogo.SetActive(false);
            equipmentMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
            inventoryMenu.SetActive(true);
            inventoryLogo.SetActive(true);
            equipmentMenu.SetActive(false);
        }
    }

    void EquipmentMenu()
    {
        if (equipmentMenu.activeSelf)
        {
            Time.timeScale = 1f; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
            inventoryMenu.SetActive(false);
            inventoryLogo.SetActive(false);
            equipmentMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
            inventoryMenu.SetActive(false);
            inventoryLogo.SetActive(true);
            equipmentMenu.SetActive(true);
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.ItemQuest)
        {
            // Implement your logic to add the item to the inventory
            Debug.Log("Item added: " + itemName + ", Quantity: " + quantity + "Sprite" + itemSprite);
            for (int i = 0; i < itemSlot.Length; i++)
            {

                if (itemSlot[i].isFull == false && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
                {
                    int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription,itemType);
                    if (leftOverItems > 0)
                        leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
            return quantity;
        }
        else
        {
            // Implement your logic to add the item to the inventory
            Debug.Log("Item added equipmentMenu: " + itemName + ", Quantity: " + quantity + "Sprite" + itemSprite);
            for (int i = 0; i < equipmentSlot.Length; i++)
            {

                if (equipmentSlot[i].isFull == false && (equipmentSlot[i].itemName == itemName || equipmentSlot[i].quantity == 0))
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

    public bool HasItem(EquipmentSO item, int amount)
    {
        int total = 0;


        foreach (var slot in itemSlot)
        {
            if (slot.itemName == item.itemName)
            {
                total += slot.quantity;
                if (total >= amount) return true;
            }
        }

        foreach (var slot in equipmentSlot)
        {
            if (slot.itemName == item.itemName)
            {
                total += slot.quantity;
                if (total >= amount) return true;
            }
        }

        return false;
    }


    public void DeselectedAllSLot()
    {
        for (int i = 0; i < itemSlot.Length;i++)
        {
           
            itemSlot[i].selectedItem.SetActive(false);
            itemSlot[i].isSelected = false;
        }
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].selectedItem.SetActive(false);
            equipmentSlot[i].isSelected = false;
            equipmentSlot[i].removeItemButton.SetActive(false);
        }
        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].selectedItem.SetActive(false);
            equippedSlot[i].isSelected = false;
            
        }

    }
    public void RemoveItemFromInventory(string itemName)
    {
        // Tìm item trong Equipment Slot
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            if (equipmentSlot[i].itemName == itemName)
            {
                equipmentSlot[i].EmptySlot(); // Xóa item khỏi slot
                return; // Item đã được xóa, không cần tiếp tục vòng lặp
            }
        }

        // Nếu item không tìm thấy trong Equipment Slot, tìm và xóa trong Item Slot
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].itemName == itemName)
            {
                itemSlot[i].EmptySlot(); // Xóa item khỏi slot
                return;
            }
        }
    }
    public void RemoveItem(EquipmentSO itemSO, int amount)
    {
        int remaining = amount;

        // Trừ trong itemSlot
        foreach (var slot in itemSlot)
        {
            if (slot.itemName == itemSO.itemName && remaining > 0)
            {
                int taken = Mathf.Min(remaining, slot.quantity);
                slot.quantity -= taken;
                if (slot.quantity <= 0)
                    slot.EmptySlot();
                remaining -= taken;
            }
        }

        // Trừ trong equipmentSlot nếu chưa đủ
        foreach (var slot in equipmentSlot)
        {
            if (slot.itemName == itemSO.itemName && remaining > 0)
            {
                int taken = Mathf.Min(remaining, slot.quantity);
                slot.quantity -= taken;
                if (slot.quantity <= 0)
                    slot.EmptySlot();
                remaining -= taken;
            }
        }

        Debug.Log($"Removed {amount} x {itemSO.itemName}");
    }
    public int AddItem(EquipmentSO itemSO, int quantity)
    {
        // Tự xác định itemType dựa vào tên hoặc nơi gọi (nếu cần). Nếu không, gán mặc định
        ItemType itemType = ItemType.crafting; // Hoặc định nghĩa logic riêng nếu cần

        return AddItem(itemSO.itemName, quantity, null, "", itemType); // null và "" là placeholder vì AddItem vẫn yêu cầu
    }

    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSOs.Length; i++)
        {
            if (itemSOs[i].itemName == itemName)
            {
                Debug.Log("Check đúng tên item");
                bool usable = itemSOs[i].UseItem(); // UseItem của ItemSO
                return usable;
            }
        }
        return false;
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
    //trang sức
    Accessory1,
    Accessory2,
    Accessory3,
    ItemQuest, // Thêm loại item cho nhiệm vụ
    none

}
