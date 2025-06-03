using UnityEngine;
using UnityEngine.UI;

public enum SkillTypeUnlock
{
    None,
    DongCung,
    TheAnh,
    BongMa,
    PhanNhan,

}

[System.Serializable]
public class SkillDataUnlock
{
    public string skillName;
    public Sprite skillIcon;
    public bool isUnlocked = false;
    public SkillTypeUnlock skillType = SkillTypeUnlock.None;
    public GameObject videoCamera;//video hướng dẫn dùng skill

   
}
