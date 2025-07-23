using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Database/EquipmentDatabase")]
public class EquipmentDatabase : ScriptableObject
{
    public List<EquipmentSO> equipmentList;

    public EquipmentSO GetEquipmentByName(string name)
    {
        foreach (var equipment in equipmentList)
        {
            if (equipment.itemName == name) // ho?c dùng ToLowerInvariant() ?? an toàn h?n
                return equipment;
        }
        return null;
    }
}
