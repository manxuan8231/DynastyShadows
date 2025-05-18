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


    public void UseItem()
    {

        if(statToChange == StatToChange.health)
        {
            Debug.Log("đã hồi máu "+ amoutToChangeStat);
            GameObject.Find("ItemCanUsing").GetComponent<PlayerStatus>().AddHealth(ammoutToChangeAttackState);
        }
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
