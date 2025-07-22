using UnityEngine;

public static class CheckpointHandler
{
    // Lưu vị trí checkpoint
    public static void SaveCheckpoint(Vector3 position)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        data.checkpointData = new CheckpointData(position);
        SaveManagerMan.SaveGame(data);
        Debug.Log("Checkpoint saved: " + position);
    }

    // Tải lại checkpoint và đặt vị trí player
    public static void LoadCheckpoint(Transform playerTransform)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        if (data.checkpointData != null)
        {
            Vector3 pos = data.checkpointData.ToVector3();
            playerTransform.position = pos;
            Debug.Log("Checkpoint loaded: " + pos);
        }
        else
        {
            Debug.Log("No checkpoint found.");
        }
    }


}

//class cac stats
public static class PlayerStatsHandler
{
    public static void SaveStats(int score, int currentLevel, int gold)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        data.score = score;
        data.currentLevel = currentLevel;
        data.gold = gold;
        SaveManagerMan.SaveGame(data);

        Debug.Log($"Đã lưu: Score={score}, ScorePerLevel={currentLevel}, Gold={gold}");
    }

    public static void LoadStats(out int score, out int scorePerLevel, out int gold)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        score = data.score;
        scorePerLevel = data.currentLevel;
        gold = data.gold;

        Debug.Log($"Đã load: Score={score}, ScorePerLevel={scorePerLevel}, Gold={gold}");
    }
}

//class skilltree
public class SkillTreeHandler
{
    public static void SaveSkillTree(SkillTreeData skillTreeData)
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        data.skillTreeData = skillTreeData;
        SaveManagerMan.SaveGame(data);
        Debug.Log("Skill tree saved.");
    }
    public static SkillTreeData LoadSkillTree()
    {
        GameSaveData data = SaveManagerMan.LoadGame();
        if (data.skillTreeData != null)
        {
            Debug.Log("Skill tree loaded.");
            return data.skillTreeData;
        }
        else
        {
            Debug.Log("No skill tree found, returning default.");
            return new SkillTreeData(); // Trả về dữ liệu mặc định nếu không có
        }
    }
}
