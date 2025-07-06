using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemRecipe", menuName = "Crafting/Item Recipe", order = 1)]
public class ItemRecipeSO : ScriptableObject
{
   public string recipeName;
   public RecipeType recipeType;

    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;
    public int coinCost;
}
public enum RecipeType
{
    Weapon,
    Armor,
    MiscellaneousItem
}
[System.Serializable]
public class ItemTypeAndCount
{
    public EquipmentSO itemOther;
    public ItermShopData item;
    public int count;


}

