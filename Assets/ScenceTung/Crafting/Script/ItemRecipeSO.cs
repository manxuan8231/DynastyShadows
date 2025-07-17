using UnityEngine;

[CreateAssetMenu(fileName = "NewItemRecipe", menuName = "Crafting/Item Recipe", order = 1)]
public class ItemRecipeSO : ScriptableObject
{
    public string recipeName;
    public ItemType recipeType;

    public InputItemData[] input;
    public OutputItemData[] output;
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
    public ItemType type;
}
