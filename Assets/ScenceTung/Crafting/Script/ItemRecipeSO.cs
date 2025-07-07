using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemRecipe", menuName = "Crafting/Item Recipe", order = 1)]
public class ItemRecipeSO : ScriptableObject
{
    public string recipeName;
    public RecipeType recipeType;

    public InputItemData[] input;
    public OutputItemData[] output;

    public int coinCost;
}

public enum RecipeType
{
    Weapon,
    Armor,
    MiscellaneousItem
}
[System.Serializable]
public class InputItemData
{
    public ItermShopData item;
    public int count;
}

[System.Serializable]
public class OutputItemData
{
    public EquipmentSO item;
    public int count;
}


