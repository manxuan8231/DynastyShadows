using UnityEngine;


public class ButtonSave : MonoBehaviour
{
    //tham chieu
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    //lưu
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
    }
  
    public void SaveGame()
    {
        PlayerStatsHandler.SaveStats(playerStatus.score, playerStatus.currentLevel, playerStatus.gold);//luu stats
        CheckpointHandler.SaveCheckpoint(playerControllerState.transform.position);//luu vi tri
    }

}
