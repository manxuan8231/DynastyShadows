using UnityEditor;
using UnityEngine;

public class EquipmentSOGeneratorSmart
{
    [MenuItem("Tools/Generate Equipment Items By Type")]
    public static void GenerateItems()
    {
        string[] types = { "Head", "Body", "Legs", "Feet", "Weapon" };
        string[] rarities = { "Common", "Rare", "Epic", "Legendary", "Special" };
        string path = "Assets/ScenceTung/Inventory/Script/DataEquipItem/";

        foreach (string type in types)
        {
            foreach (string rarity in rarities)
            {
                EquipmentSO newSO = ScriptableObject.CreateInstance<EquipmentSO>();
                newSO.itemName = $"{type}{rarity}";
            

                SetStatsByTypeAndRarity(newSO, type, rarity);

                string assetPath = $"{path}{type}{rarity}.asset";
                AssetDatabase.CreateAsset(newSO, assetPath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("🎉 Đã tạo xong toàn bộ Equipment theo loại và độ hiếm!");
    }

    static void SetStatsByTypeAndRarity(EquipmentSO so, string type, string rarity)
    {
        switch (type)
        {
            case "Weapon":
                switch (rarity)
                {
                    case "Common":
                        so.attackRange = new Vector2Int(40, 60);
                        so.critDameRange = new Vector2Int(50, 60);
                        so.critChanceRange = new Vector2Int(5, 8);
                        break;
                    case "Rare":
                        so.attackRange = new Vector2Int(60, 70);
                        so.critDameRange = new Vector2Int(60, 70);
                        so.critChanceRange = new Vector2Int(8, 10);
                        break;
                    case "Epic":
                        so.attackRange = new Vector2Int(70, 85);
                        so.critDameRange = new Vector2Int(70, 85);
                        so.critChanceRange = new Vector2Int(10, 15);
                        break;
                    case "Legendary":
                        so.attackRange = new Vector2Int(100, 120);
                        so.critDameRange = new Vector2Int(90, 110);
                        so.critChanceRange = new Vector2Int(15, 25);
                        break;
                    case "Special":
                        so.attackRange = new Vector2Int(130, 160);
                        so.critDameRange = new Vector2Int(120,130);
                        so.critChanceRange = new Vector2Int(25,30);
                        break;
                }
                break;

            default: // Head, Body, Legs, Feet
                switch (rarity)
                {
                    case "Common":
                        so.hpRange = new Vector2Int(120, 130);
                        so.manaRange = new Vector2Int(150, 170);
                        break;
                    case "Rare":
                        so.hpRange = new Vector2Int(130, 170);
                        so.manaRange = new Vector2Int(170, 180);
                        break;
                    case "Epic":
                        so.hpRange = new Vector2Int(170, 200);
                        so.manaRange = new Vector2Int(180, 195);
                        break;
                    case "Legendary":
                        so.hpRange = new Vector2Int(210, 250);
                        so.manaRange = new Vector2Int(200, 220);
                        break;
                    case "Special":
                        so.hpRange = new Vector2Int(260, 350);
                        so.manaRange = new Vector2Int(230, 270);
                        break;
                }
                break;
        }
    }
}
