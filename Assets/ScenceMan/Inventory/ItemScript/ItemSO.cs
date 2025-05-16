using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
    public ItemType itemType;
    public float value; // Lượng máu hoặc mana hồi
}
public enum ItemType
{
    Heal,
    Mana
}
