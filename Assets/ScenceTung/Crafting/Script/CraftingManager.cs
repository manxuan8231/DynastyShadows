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
                Debug.LogError($"Input item in recipe '{recipe.recipeName}' is null.");
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

        // Trừ nguyên liệu từ input
        foreach (var input in recipe.input)
        {
            if (input.item == null)
            {
                Debug.LogError($"Input item is NULL in recipe: {recipe.recipeName}");
                continue;
            }
            Debug.Log($"Trừ {input.count} x {input.item.itemName}");
            inventory.RemoveItem(input.item, input.count);
        }

        foreach (var output in recipe.output)
        {
            inventory.AddItem(output.item, output.count, output.type); // type ở đây là ItemType
        }

    }

}
