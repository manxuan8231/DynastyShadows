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

    public int craftAmount = 1; 

    public void Craft(ItemRecipeSO recipe, InventoryManager inventory)
    {
        if (!CanCraft(recipe, inventory)) return;

        // Trừ nguyên liệu theo số lượng muốn craft
        foreach (var input in recipe.input)
        {
            if (input.item == null)
            {
                Debug.LogError($"[Craft] Input item is NULL in recipe: {recipe.recipeName}");
                continue;
            }

            int totalToRemove = input.count * craftAmount;
            inventory.RemoveItem(input.item, totalToRemove); // ✅ Đã sửa cú pháp
        }

        // Thêm item output vào inventory
        foreach (var output in recipe.output)
        {
            if (output.item != null)
            {
                inventory.AddItem(output.item, output.count * craftAmount, output.type); // ✅ Nhân số lượng output theo craftAmount
            }
            else
            {
                Debug.LogError($"[Craft] Output item is NULL in recipe: {recipe.recipeName}");
            }
        }
    }

}
