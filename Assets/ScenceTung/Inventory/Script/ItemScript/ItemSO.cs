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
            PlayerStatus playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
            if (playerStatus.currentHp == playerStatus.maxHp)
            {
                Debug.Log("Máu đầy");
                return false;
            }
            else
            {
                playerStatus.BuffHealth(amoutToChangeStat);
                Debug.Log("đã hồi máu " + amoutToChangeStat);
                return true;
            }
        }
        if (statToChange == StatToChange.stamina)
        {
            PlayerStatus playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
            if(playerStatus.currentHp == playerStatus.maxHp)
            {
                Debug.Log("Máu đầy");
                return false;
            }
            else
            {
                playerStatus.BuffHealth(amoutToChangeStat);
                Debug.Log("đã hồi máu " + amoutToChangeStat);
                playerStatus.BuffDamage(amoutToChangeStat,60);
                return true;
            }
        }
        if (statToChange == StatToChange.mana)
        {
            PlayerStatus playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
            if (playerStatus.currentMana == playerStatus.maxMana)
            {
                Debug.Log("mana đã đầy");
                return false;
            }
            else
            {
                playerStatus.BuffMana(amoutToChangeStat);
                Debug.Log("đã hồi mana " + amoutToChangeStat);
                return true;
            }
        }
        return false;
    }
  

      
    public enum StatToChange
    {
        none,
        health,
        mana,
        stamina,
        crit,
        critChance,

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
