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

    public GameObject videoCamera;
}
