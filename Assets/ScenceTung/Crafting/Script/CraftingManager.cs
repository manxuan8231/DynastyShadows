using System.Collections.Generic;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;
    public List<ItemRecipeSO> allRecipes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanCraft(ItemRecipeSO recipe, InventoryManager inventory)
    {
        foreach (var input in recipe.input)
        {
            if (input.item == null)
            {
                Debug.LogError($"[CanCraft] Input item in recipe '{recipe.recipeName}' is null.");
                return false;
            }

            if (!inventory.HasItem(input.item, input.count))
                return false;
        }

        return true;
    }

    public void Craft(ItemRecipeSO recipe, InventoryManager inventory)
    {
        if (!CanCraft(recipe, inventory)) return;

        foreach (var input in recipe.input)
        {
            if (input.item == null)
            {
                Debug.LogError($"[Craft] Input item is NULL in recipe: {recipe.recipeName}");
                continue;
            }

            inventory.RemoveItem(input.item, input.count);
        }

        foreach (var output in recipe.output)
        {
            if (output.item != null)
            {
                inventory.AddItem(output.item, output.count, output.type);
            }
            else
            {
                Debug.LogError($"[Craft] Output item is NULL in recipe: {recipe.recipeName}");
            }
        }
    }
}
