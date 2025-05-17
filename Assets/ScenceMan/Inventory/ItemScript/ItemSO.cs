using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    public ItemType itemType;
    public float value;

    // Chỉ số vũ khí
    public int sharpness;
    public float critRate;
    public float critDamage;
    public Rarity rarity;
}

public enum ItemType
{
    Weapon,
    head,
    body,
    legs,
    feet,
    comsumable,
    crafting,
    none
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
