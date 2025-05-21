using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
  public string itemName;
    public int attack,hp,mana,critDame, critChance;

    [SerializeField]
    private Sprite itemSprite;

    public void PreviewEquipment()
    {
        GameObject.Find("Stats").GetComponent<PlayerStatus>().
            PreviewEquipmentItem(hp,mana,attack,critDame,critChance,itemSprite);
        
    }
    public void EquipItem()
    {
        PlayerStatus playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
        playerStatus.maxHp += hp;
        playerStatus.sliderHp.maxValue += hp;
        playerStatus.maxMana += mana;
        playerStatus.sliderMana.maxValue += mana;
        playerStatus.baseDamage += attack;

        playerStatus.criticalDamage += critDame;
        playerStatus.criticalChance += critChance;


    }
    public void UnEquipItem()
    {
        PlayerStatus playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
        playerStatus.maxHp -= hp;
        playerStatus.sliderHp.maxValue -= hp;
        playerStatus.maxMana -= mana;
        playerStatus.sliderMana.maxValue -= mana;
        playerStatus.baseDamage -= attack;
        playerStatus.criticalDamage -= critDame;
        playerStatus.criticalChance -= critChance;
    }
}
