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

        if(statToChange == StatToChange.health)
        {
          
            PlayerStatus playerStatus = GameObject.Find("ItemCanUsing").GetComponent<PlayerStatus>();
            if(playerStatus.currentHp == playerStatus.maxHp)
            {
                Debug.Log("máu đã đầy");
                return false;
            }
            else
            {
                playerStatus.AddHealth(ammoutToChangeAttackState);
                Debug.Log("đã hồi máu " + amoutToChangeStat);
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
