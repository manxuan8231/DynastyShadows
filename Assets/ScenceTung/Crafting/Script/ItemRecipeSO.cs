using UnityEngine;

[CreateAssetMenu(fileName = "NewItemRecipe", menuName = "Crafting/Item Recipe", order = 1)]
public class ItemRecipeSO : ScriptableObject
{
   public string recipeName;
   public RecipeType recipeType;

    public ItemTypeAndCount[] input;
    public ItemTypeAndCount[] output;
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
    public ItemSO itemOther;
    public int count;
    public ItemTypeAndCount(ItemSO itemOthers, int count)
    {
        this.itemOther = itemOthers;
        this.count = count;
    }
}
