using UnityEngine;

[CreateAssetMenu(fileName = "ItermShopData", menuName = "ScriptableObjects/ItermShopData", order = 1)]
public class ItermShopData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;

}
