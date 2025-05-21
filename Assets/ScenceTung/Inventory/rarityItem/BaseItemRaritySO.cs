using UnityEngine;

public abstract class BaseItemRaritySO : ScriptableObject
{
    public RarityType rarity;

    [Header("Chỉ số tối thiểu - tối đa")]
    public Vector2Int healthRange;
    public Vector2Int damageRange;
    public Vector2Int critRange;
    public Vector2Int critChanceRange;
    public Vector2Int manaRange;

    public virtual int GetRandomHealth() => Random.Range(healthRange.x, healthRange.y + 1);
    public virtual int GetRandomDamage() => Random.Range(damageRange.x, damageRange.y + 1);
    public virtual int GetRandomDefense() => Random.Range(critRange.x, critRange.y + 1);
    public virtual int GetRandomCritChance() => Random.Range(critChanceRange.x, critChanceRange.y + 1);
    public virtual int GetRandomMana() => Random.Range(manaRange.x, manaRange.y + 1);
}
