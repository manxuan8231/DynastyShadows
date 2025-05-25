using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    
    public string itemName;
    
    // Cho phép random trong khoảng
    public Vector2Int attackRange;
    public Vector2Int hpRange;
    public Vector2Int manaRange;
    public Vector2Int critDameRange;
    public Vector2Int critChanceRange;

    [SerializeField]
    private Sprite itemSprite;
    private bool hasGeneratedStats = false;


    // Biến lưu chỉ số sau khi random
    [HideInInspector] public int attack;
    [HideInInspector] public int hp;
    [HideInInspector] public int mana;
    [HideInInspector] public int critDame;
    [HideInInspector] public int critChance;

    // Gọi hàm này trước khi equip để random stats
    public void GenerateRandomStats()
    {
        attack = Random.Range(attackRange.x, attackRange.y + 1);
        hp = Random.Range(hpRange.x, hpRange.y + 1);
        mana = Random.Range(manaRange.x, manaRange.y + 1);
        critDame = Random.Range(critDameRange.x, critDameRange.y + 1);
        critChance = Random.Range(critChanceRange.x, critChanceRange.y + 1);
    }
    public void PreviewEquipment()
    {
        if (!hasGeneratedStats)
        {
            GenerateRandomStats();
            Debug.Log(attack + hp + mana + critDame + critChance);
            hasGeneratedStats = true;

        }

        GameObject.Find("Stats").GetComponent<PlayerStatus>().PreviewEquipmentItem(
            hp, mana, attack, critDame, critChance, itemSprite,itemName);
    }

    public void EquipItem()
    {
        // Equip theo chỉ số đã random
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

    public void RemovedItem()
    {
        // Optionally, reset generated stats and mark as not generated
        hasGeneratedStats = false;
        attack = 0;
        hp = 0;
        mana = 0;
        critDame = 0;
        critChance = 0;
    }
}
