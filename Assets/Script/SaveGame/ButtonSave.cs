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
        data.savedSceneName = SceneManager.GetActiveScene().name;

       

        // ✅ Cuối cùng, lưu lại
        SaveManagerMan.SaveGame(data);
      
    }


}
