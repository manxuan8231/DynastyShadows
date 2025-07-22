using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSave : MonoBehaviour
{
    // Tham chiếu tới player
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
       
    }

    public void SaveGame()
    {
        // Tạo dữ liệu mới
        //GameSaveData data = new GameSaveData();
        GameSaveData data = SaveManagerMan.LoadGame();
     
        // Lưu chỉ số
        data.score = playerStatus.score;
        data.currentLevel = playerStatus.currentLevel;
        data.gold = playerStatus.gold;

        // Lưu vị trí player
        data.checkpointData = new CheckpointData(playerControllerState.transform.position);

        // Lưu tên scene hiện tại
        data.savedSceneName = SceneManager.GetActiveScene().name;

       

        // Gọi hàm lưu
        SaveManagerMan.SaveGame(data);

        Debug.Log("Game Saved at " + data.savedSceneName);
    }


}
