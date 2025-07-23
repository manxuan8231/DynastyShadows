using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSave : MonoBehaviour
{
    // Tham chiếu tới player
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    public InventoryManager inventoryManager;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void SaveGame()
    {
        GameSaveData data = SaveManagerMan.LoadGame();

        data.score = playerStatus.score;
        data.currentLevel = playerStatus.currentLevel;
        data.gold = playerStatus.gold;
        data.checkpointData = new CheckpointData(playerControllerState.transform.position);
        data.savedSceneName = SceneManager.GetActiveScene().name;

        // ✅ Lưu item
        data.inventoryItems.Clear();
        data.inventoryItemSos.Clear();
        foreach (var slot in inventoryManager.itemSlot)
        {
            if (!string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
            {
                data.inventoryItemSos.Add(new SaveItemSO
                {
                    itemName = slot.itemName,
                    quantity = slot.quantity,
                    itemType = slot.itemType.ToString(),
                    itemSprite = slot.itemSprite,
                    itemDescription = slot.itemDescription

                });
            }
        }

        foreach (var slot in inventoryManager.equipmentSlot)
        {
            if (!string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
            {
                data.inventoryItems.Add(new SavedItemData
                {
                    itemName = slot.itemName,
                    quantity = slot.quantity,
                    itemType = slot.itemType.ToString()
                });
            }
        }

        // ✅ Cuối cùng, lưu lại
        SaveManagerMan.SaveGame(data);
        Debug.Log("Saved with " + data.inventoryItems.Count + " items");
        Debug.Log("Saved with " + data.inventoryItemSos.Count + " itemSOs");
    }


}
