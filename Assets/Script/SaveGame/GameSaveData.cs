

[System.Serializable]
public class GameSaveData
{
    //luu stats
    public int score;
    public int currentLevel;
    public int gold;

    //luu checkpoint
    public CheckpointData checkpointData;

    public GameSaveData()//mặc định khi chx lưu
    {
        score = 1110;
        currentLevel = 1;
        gold = 110;
        
    }
}
