using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemSO", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{

    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public float amoutToChangeStat;

    public AttributeChange attackState = new AttributeChange();
    public float ammoutToChangeAttackState;
    [Header("Bool Quest")] 
    public bool hasItemQuest = false;
    public bool showSkill4 = false; // Biến để kiểm tra xem có hiển thị kỹ năng 4 hay không
    //unlock skill 2
    public bool hasItemQuest2 = false;
    public bool showSkill5 = false; // Biến để kiểm tra xem có hiển thị kỹ năng 5 hay không
    public bool UseItem()
    {
        if (statToChange == StatToChange.health)
        {
            
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            if (itemBuff.status.currentHp == itemBuff.status.maxHp)
            {
                Debug.Log("Máu đầy");
                return false;
            }
            else
            {
                itemBuff.BuffHP(amoutToChangeStat);     
                Debug.Log("đã hồi máu " + amoutToChangeStat);
                return true;
            }
        }

        if (statToChange == StatToChange.mana)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            if (itemBuff.status.currentMana == itemBuff.status.maxMana)
            {
                Debug.Log("Máu đầy");
                return false;
            }
            else
            {
                itemBuff.BuffMana(amoutToChangeStat);
                Debug.Log("đã hồi máu " + amoutToChangeStat);
                return true;
            }
        }
        if (statToChange == StatToChange.stamina)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();

            itemBuff.BuffDameAndCrit((int)ammoutToChangeAttackState, 10, 15);
            return true;
        }
        if (statToChange == StatToChange.buffAttack)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.PotionDameRage((int)ammoutToChangeAttackState, 10);
            return true;
        }
        if (statToChange == StatToChange.buffCrit)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.BUffCrit((int)ammoutToChangeAttackState, 8);
            return true;
        }
        if (statToChange == StatToChange.BuffCritRate)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.PotionCritRate((int)ammoutToChangeAttackState, 8);
            return true;
        }
        if(statToChange == StatToChange.buffAllStats)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.BuffAllStats((int)amoutToChangeStat, 85, 30, 30, 10);
            return true;
        }
        if(statToChange == StatToChange.debuff1)
        {
            ItemDebuff debuff = GameObject.Find("Stats").GetComponent<ItemDebuff>();
            debuff.Debuff1(3, 10);
            return true;
        }
        if(statToChange ==StatToChange.debuff2)
        {
            ItemDebuff debuff = GameObject.Find("Stats").GetComponent<ItemDebuff>();
            debuff.Debuff2(2,300,5);
            return true;
        }
        if (statToChange == StatToChange.debuff3)
        {
            ItemDebuff debuff = GameObject.Find("Stats").GetComponent<ItemDebuff>();
            debuff.DeBuff3(10, 11, 5);
            return true;
        }
        if (statToChange == StatToChange.debuff4)
        {
            ItemDebuff debuff = GameObject.Find("Stats").GetComponent<ItemDebuff>();
            debuff.Debuff4(2, 50, 10);
            return true;
        }
        if (statToChange == StatToChange.itemQuest)
        {

            ItemSO cloned = Instantiate(this);
            cloned.showSkill4 = true;
            cloned.hasItemQuest = true;

    SkillFlexibleManager manager = Resources.FindObjectsOfTypeAll<SkillFlexibleManager>()
                .FirstOrDefault(x => x.gameObject.name == " Panel(Flexible Skill)");

            if (manager != null)
            {
                manager.itemQuestUnlock = cloned;
                Debug.Log("Gán clone ItemSO vào SkillFlexibleManager OK");
            }
            else
            {
                Debug.LogError("Không tìm thấy SkillFlexibleManager");
            }

            return true;



        }
        if (statToChange == StatToChange.itemQuest2)
        {
            Debug.Log("Item Quest 2");
            ItemSO cloned2 = Instantiate(this);
            cloned2.hasItemQuest2 = true;
            cloned2.showSkill5 = true;
            SkillFlexibleManager manager = Resources.FindObjectsOfTypeAll<SkillFlexibleManager>()
                .FirstOrDefault(x => x.gameObject.name == " Panel(Flexible Skill)");
            if (manager != null)
            {
                manager.itemQuestUnlock2 = cloned2;
                Debug.Log("Gán clone ItemSO vào SkillFlexibleManager OK");
            }
            else
            {
                Debug.LogError("Không tìm thấy SkillFlexibleManager");
            }
            return true;
        }
        return false;
        
    }



    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina,
        buffAttack,
        buffCrit,
        BuffCritRate,
        buffAllStats,
        debuff1,
        debuff2,
        debuff3,
        debuff4,
        itemQuest,
        itemQuest2
    }


    public enum AttributeChange
    {
        none,
        strength,//tăng sát thương
        Agility, //tăng sát thương chí mạng
        intelligence, //tăng mana
        vitality //tăng máu

    }
}
