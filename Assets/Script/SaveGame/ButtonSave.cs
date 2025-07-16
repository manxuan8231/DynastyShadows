using UnityEngine;


public class ButtonSave : MonoBehaviour
{
    //tham chieu
    public PlayerStatus playerStatus;
    //lưu
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    public void SaveStats()
    {
        PlayerStatsHandler.SaveStats(playerStatus.score, playerStatus.currentLevel, playerStatus.gold);
    }
}
