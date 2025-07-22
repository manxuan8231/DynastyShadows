
[System.Serializable]
public class GameSaveData
{
    //luu stats
    public int score;
    public int currentLevel;
    public int gold;

    //luu checkpoint
    public CheckpointData checkpointData;
    public string savedSceneName;
    // lưu hoàn thành hướng dẫn
    public bool isTutorialDone;
    public int mapTutoIndex;
    public GameSaveData()//mặc định khi chx lưu
    {
        score = 10;
        currentLevel = 1;
        gold = 10;
        isTutorialDone = false;
        mapTutoIndex = 0;
        
    }
}
