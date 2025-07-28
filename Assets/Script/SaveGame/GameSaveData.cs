
using System.Collections.Generic;
using UnityEngine;

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
    public List<SavedItemData> inventoryItems = new List<SavedItemData>();
    public List<SaveItemSO> inventoryItemSos = new List<SaveItemSO>();
    // lưu hoàn thành hướng dẫn
    public bool isTutorialDone;
    public int mapTutoIndex;

    // lưu skill tree
    public SkillTreeData skillTreeData;
    // lưu quest data
    public DataQuest dataQuest;

    public GameSaveData()//mặc định khi chx lưu
    {
        score = 10;
        currentLevel = 1;
        gold = 10;
        isTutorialDone = false;
        mapTutoIndex = 0;
        
    }
}
[System.Serializable]
public class SavedItemData
{
    public string itemName;
    public int quantity;
    public string itemType;
}
[System.Serializable]
public class SaveItemSO
{
    public string itemName;
    public string itemType;
    public Sprite itemSprite;
    public string itemDescription;
    public int quantity;
   
   
}
