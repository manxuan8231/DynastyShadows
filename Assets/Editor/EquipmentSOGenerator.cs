using UnityEditor;
using UnityEngine;

public class EquipmentSOGeneratorSmart
{
    [MenuItem("Tools/Generate Equipment Items By Type")]
    public static void GenerateItems()
    {
        string[] types = { "Accessory1" ,"Accessory2", "Accessory3" };
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
            case "Accessory1":
                switch (rarity)
                {
                    case "Common":
                        so.hpRange = new Vector2Int(90,110);
                        so.manaRange = new Vector2Int(50,65);
                        so.attackRange = new Vector2Int(50, 65);
                        so.critDameRange = new Vector2Int(15, 20);
                        so.critChanceRange = new Vector2Int(1, 3);
                        break;
                    case "Rare":
                        so.hpRange = new Vector2Int(110,125);
                        so.manaRange = new Vector2Int(65,80);
                        so.attackRange = new Vector2Int(65, 75);
                        so.critDameRange = new Vector2Int(20, 25);
                        so.critChanceRange = new Vector2Int(3, 5);
                        break;
                    case "Epic":
                        so.hpRange = new Vector2Int(125,140);
                        so.manaRange = new Vector2Int(80,95);
                        so.attackRange = new Vector2Int(75, 80);
                        so.critDameRange = new Vector2Int(25, 28);
                        so.critChanceRange = new Vector2Int(5, 7);
                        break;
                    case "Legendary":
                        so.hpRange = new Vector2Int(140,150);
                        so.manaRange = new Vector2Int(95,105);
                        so.attackRange = new Vector2Int(80, 95);
                        so.critDameRange = new Vector2Int(28, 30);
                        so.critChanceRange = new Vector2Int(7, 8);
                        break;
                    case "Special":
                        so.hpRange = new Vector2Int(150,165);
                        so.manaRange = new Vector2Int(105,120);
                        so.attackRange = new Vector2Int(95, 110);
                        so.critDameRange = new Vector2Int(30,40);
                        so.critChanceRange = new Vector2Int(8,10);
                        break;
                }
                break;

            case "Accessory2":
                switch (rarity)
                {
                    case "Common":
                        so.hpRange = new Vector2Int(90, 110);
                        so.manaRange = new Vector2Int(50, 65);
                        so.attackRange = new Vector2Int(50, 65);
                        so.critDameRange = new Vector2Int(15, 20);
                        so.critChanceRange = new Vector2Int(1, 3);
                        break;
                    case "Rare":
                        so.hpRange = new Vector2Int(110, 125);
                        so.manaRange = new Vector2Int(65, 80);
                        so.attackRange = new Vector2Int(65, 75);
                        so.critDameRange = new Vector2Int(20, 25);
                        so.critChanceRange = new Vector2Int(3, 5);
                        break;
                    case "Epic":
                        so.hpRange = new Vector2Int(125, 140);
                        so.manaRange = new Vector2Int(80, 95);
                        so.attackRange = new Vector2Int(75, 80);
                        so.critDameRange = new Vector2Int(25, 28);
                        so.critChanceRange = new Vector2Int(5, 7);
                        break;
                    case "Legendary":
                        so.hpRange = new Vector2Int(140, 150);
                        so.manaRange = new Vector2Int(95, 105);
                        so.attackRange = new Vector2Int(80, 95);
                        so.critDameRange = new Vector2Int(28, 30);
                        so.critChanceRange = new Vector2Int(7, 8);
                        break;
                    case "Special":
                        so.hpRange = new Vector2Int(150, 165);
                        so.manaRange = new Vector2Int(105, 120);
                        so.attackRange = new Vector2Int(95, 110);
                        so.critDameRange = new Vector2Int(30, 40);
                        so.critChanceRange = new Vector2Int(8, 10);
                        break;
                }
                break;
            case "Accessory3":
                switch (rarity)
                {
                    case "Common":
                        so.hpRange = new Vector2Int(90, 110);
                        so.manaRange = new Vector2Int(50, 65);
                        so.attackRange = new Vector2Int(50, 65);
                        so.critDameRange = new Vector2Int(15, 20);
                        so.critChanceRange = new Vector2Int(1, 3);
                        break;
                    case "Rare":
                        so.hpRange = new Vector2Int(110, 125);
                        so.manaRange = new Vector2Int(65, 80);
                        so.attackRange = new Vector2Int(65, 75);
                        so.critDameRange = new Vector2Int(20, 25);
                        so.critChanceRange = new Vector2Int(3, 5);
                        break;
                    case "Epic":
                        so.hpRange = new Vector2Int(125, 140);
                        so.manaRange = new Vector2Int(80, 95);
                        so.attackRange = new Vector2Int(75, 80);
                        so.critDameRange = new Vector2Int(25, 28);
                        so.critChanceRange = new Vector2Int(5, 7);
                        break;
                    case "Legendary":
                        so.hpRange = new Vector2Int(140, 150);
                        so.manaRange = new Vector2Int(95, 105);
                        so.attackRange = new Vector2Int(80, 95);
                        so.critDameRange = new Vector2Int(28, 30);
                        so.critChanceRange = new Vector2Int(7, 8);
                        break;
                    case "Special":
                        so.hpRange = new Vector2Int(150, 165);
                        so.manaRange = new Vector2Int(105, 120);
                        so.attackRange = new Vector2Int(95, 110);
                        so.critDameRange = new Vector2Int(30, 40);
                        so.critChanceRange = new Vector2Int(8, 10);
                        break;
                }
                break;

        }
    }
}
