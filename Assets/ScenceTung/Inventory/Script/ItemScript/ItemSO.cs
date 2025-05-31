using UnityEngine;
using UnityEngine.Android;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{

    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amoutToChangeStat;

    public AttributeChange attackState = new AttributeChange();
    public int ammoutToChangeAttackState;


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

            itemBuff.BuffDameAndCrit(ammoutToChangeAttackState, 10, 15);
            return true;
        }
        if (statToChange == StatToChange.buffAttack)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.PotionDameRage(ammoutToChangeAttackState, 10);
            return true;
        }
        if (statToChange == StatToChange.buffCrit)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.BUffCrit(ammoutToChangeAttackState, 8);
            return true;
        }
        if (statToChange == StatToChange.BuffCritRate)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.PotionCritRate(ammoutToChangeAttackState, 8);
            return true;
        }
        if(statToChange == StatToChange.buffAllStats)
        {
            itemBuff itemBuff = GameObject.Find("Stats").GetComponent<itemBuff>();
            itemBuff.BuffAllStats(amoutToChangeStat, 85, 30, 30, 10);
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
        debuff4


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
