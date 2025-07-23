using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Database/EquipmentDatabase")]
public class EquipmentDatabase : ScriptableObject
{
    public List<EquipmentSO> equipmentList;
    public List<ItemSO> itemList;

    public EquipmentSO GetEquipmentByName(string name)
    {
        foreach (var equipment in equipmentList)
        {
            if (equipment.itemName == name) // ho?c dùng ToLowerInvariant() ?? an toàn h?n
                return equipment;
        }
        return null;
    }
    public ItemSO GetItemByName(string name)
    {
        foreach (var item in itemList)
        {
            if (item.itemName == name) // ho?c dùng ToLowerInvariant() ?? an toàn h?n
                return item;
        }
        return null;
    }
}
