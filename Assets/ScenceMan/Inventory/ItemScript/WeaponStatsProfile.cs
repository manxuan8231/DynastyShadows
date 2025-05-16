using UnityEngine;

[CreateAssetMenu(menuName = "Item/Weapon Stats Profile")]
public class WeaponStatsProfile : ScriptableObject
{
    public Rarity rarity;
    public int minSharpness, maxSharpness;
    public float minCritRate, maxCritRate;
    public float minCritDamage, maxCritDamage;
}
