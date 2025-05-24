using UnityEngine;

public enum SkillType
{
    None,
    RostbindSoul,
    Fireball,
   
    
}

[System.Serializable] 
public class SkillData
{
    public string skillName;
    public Sprite skillIcon;
    public bool isUnlocked = false;
    public SkillType skillType = SkillType.None;
    public GameObject videoCamera;//video hướng dẫn dùng skil

    //skill 1 dong cung enemy
    public GameObject skillPrefab; // Prefab của kỹ năng
    public Transform spawnPoint; // Vị trí spawn kỹ năng

}
