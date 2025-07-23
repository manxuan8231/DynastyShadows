[System.Serializable]
public class SkillTreeData
{
    //skill core----------
    public float turnInSkill1, turnInSkill2, turnInSkill3, turnInSkill4;

    public bool isUnlockSkill1, isUnlockSkill2, isUnlockSkill3, isUnlockSkill4;

    public bool isDamagedSkill1;
    public bool isDamagedSkill3;
    public bool isExplosionSkill2;
    public bool isLv6Skill3;
    public bool isReflectDamageSkill4;
    public bool isUpSpeedSkill4;
    public bool isStunSkill4;
    public bool isImmotalSkill4;

    public float cooldownSkill1 = 50, cooldownSkill2 = 50, cooldownSkill3 = 100, cooldownSkill4 = 100;
    public float timeSkill1 = 5, timeSkill2 = 10, timeSkill3 = 30, timeSkill4 = 25;

    public float playerCountSkill3 = 4;


    public bool[] unlockedSkillIcons = new bool[21]; // 21 kỹ năng để đổi màu white icon


    //skill expilision------------                
    public bool isDongCung1Unlocked = false;
    public bool isDongCung2Unlocked = false;
    public bool isDongCung3Unlocked = false;
    public bool isDongCung4Unlocked = false;
    public bool isDongCung5Unlocked = false;

}
