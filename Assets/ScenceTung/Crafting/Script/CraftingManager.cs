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
            if (!inventory.HasItem(input.itemOther, input.count))
                return false;
        }
        return true;
        
    }

    public void Craft(ItemRecipeSO recipe, InventoryManager inventory)
    {
        if (!CanCraft(recipe, inventory)) return;

        // Remove required items
        foreach (var input in recipe.input)
        {
            inventory.RemoveItem(input.itemOther, input.count);
        }

        // Add crafted result
        foreach (var output in recipe.output)
        {
            inventory.AddItem(output.itemOther, output.count);
        }

        Debug.Log($"Crafted {recipe.recipeName} successfully!");
    }
}
