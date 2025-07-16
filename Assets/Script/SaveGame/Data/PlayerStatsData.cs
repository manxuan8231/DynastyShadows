[System.Serializable]
public class PlayerStatsData
{
    public int score;
    public int scorePerLevel;
    public int gold;

    public PlayerStatsData(int score, int scorePerLevel, int gold)
    {
        this.score = score;
        this.scorePerLevel = scorePerLevel;
        this.gold = gold;
    }
}
