using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject canvasPauser;
    public EquipmentDatabase equipmentDatabase;
    public GameObject equipmentMenu;
    public bool isOpenInventory = true;
    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    public EquippedSlot[] equippedSlot;
    public ItemSO[] itemSOs;
    public AudioSource audioSource;
    public AudioClip selectedClip;
    private PauseManager pausedManager;
    public ButtonSave buttonSave;
    public PlayerControllerState pl;
    public OpenMap openMap;
    void Update()
    {
        if (pausedManager == null) pausedManager = FindAnyObjectByType<PauseManager>();
        audioSource = GameObject.Find("Inventory").GetComponent<AudioSource>();

        if (Input.GetButtonDown("Inventory") && isOpenInventory && pl.animator.enabled && !openMap.isTurnOffMap && !TurnOffOnUI.openShop && !TurnOffOnUI.isTutorialInven)
            Inventory();
        if (Input.GetButtonDown("EquipmentMenu") && isOpenInventory && pl.animator.enabled && !openMap.isTurnOffMap && !TurnOffOnUI.openShop && !TurnOffOnUI.isTutorialInven)
            EquipmentMenu();
    }
    void Start()
    {
        // Load game data khi bắt đầu
        GameSaveData data = SaveManagerMan.LoadGame();
        LoadInventoryFromSave(data);
        LoadItemSOFromSave(data);
        pl = FindAnyObjectByType<PlayerControllerState>();
        openMap = FindAnyObjectByType<OpenMap>();
    }

    public void Inventory()
    {
        if (inventoryMenu.activeSelf)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inventoryMenu.SetActive(false);
            TurnOffOnUI.pause = false;
            equipmentMenu.SetActive(false);
            pausedManager.canvasPause.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryMenu.SetActive(true);
            TurnOffOnUI.pause = true;
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
            TurnOffOnUI.pause = false;
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
            TurnOffOnUI.pause = true;
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.ItemQuest || itemType == ItemType.Pet)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if ((!itemSlot[i].isFull &&(string.IsNullOrEmpty(itemSlot[i].itemName) ||
                itemSlot[i].itemName == itemName ||
                itemSlot[i].quantity == 0)))
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
public void LoadInventoryFromSave(GameSaveData data)
{
    if (data == null || data.inventoryItems == null)
    {
        Debug.LogWarning("Không có dữ liệu inventory để load.");
        return;
    }
    foreach (var slot in equipmentSlot)
    {
        slot?.EmptySlot();
    }

    foreach (var savedItem in data.inventoryItems)
    {
        if (System.Enum.TryParse(savedItem.itemType, out ItemType type))
        {
            EquipmentSO equipment = null;
            if (equipmentDatabase != null)
            {
                equipment = equipmentDatabase.GetEquipmentByName(savedItem.itemName);
            }

            if (equipment != null)
            {
                AddItem(savedItem.itemName, savedItem.quantity, equipment.itemSprite, equipment.itemDescription, type);
            }
            else
            {
                Debug.LogWarning($"Không tìm thấy equipment '{savedItem.itemName}' hoặc database chưa gán.");
                // Có thể gọi AddItem với sprite và mô tả null nếu muốn
                AddItem(savedItem.itemName, savedItem.quantity, null, "", type);
            }
        }
        else
        {
            Debug.LogWarning($"Không parse được ItemType: {savedItem.itemType}");
        }
    }
}

    public void LoadItemSOFromSave(GameSaveData data)
    {
        if (data == null || data.inventoryItemSos == null)
        {
            Debug.LogWarning("Không có dữ liệu inventory để load.");
            return;
        }
        foreach (var slot in itemSlot)
        {
            slot?.EmptySlot();
        }

        foreach (var savedItem in data.inventoryItemSos)
        {
            if (System.Enum.TryParse(savedItem.itemType, out ItemType type))
            {
                ItemSO equipment = null;
                if (equipmentDatabase != null)
                {
                    equipment = equipmentDatabase.GetItemByName(savedItem.itemName);
                }

                if (equipment != null)
                {
                    AddItem(savedItem.itemName, savedItem.quantity, savedItem.itemSprite, savedItem.itemDescription, type);
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy equipment '{savedItem.itemName}' hoặc database chưa gán.");
                    // Có thể gọi AddItem với sprite và mô tả null nếu muốn
                    AddItem(savedItem.itemName, savedItem.quantity, null, "", type);
                }
            }
            else
            {
                Debug.LogWarning($"Không parse được ItemType: {savedItem.itemType}");
            }
        }
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
    Pet,
    none
}
