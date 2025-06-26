using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingUI : MonoBehaviour
{
    public Transform recipeListParent;
    public GameObject recipeButtonPrefab;
    public InventoryManager inventory;

    void Start()
    {
        PopulateRecipes();
    }

    void PopulateRecipes()
    {
        foreach (var recipe in CraftingManager.Instance.allRecipes)
        {
            GameObject buttonGO = Instantiate(recipeButtonPrefab, recipeListParent);
            buttonGO.GetComponentInChildren<TMP_Text>().text = recipe.recipeName;

            Button btn = buttonGO.GetComponent<Button>();
            btn.onClick.AddListener(() => OnRecipeSelected(recipe));
        }
    }

    void OnRecipeSelected(ItemRecipeSO recipe)
    {
        if (CraftingManager.Instance.CanCraft(recipe, inventory))
        {
            CraftingManager.Instance.Craft(recipe, inventory);
            Debug.Log($"Crafted {recipe.recipeName}");
        }
        else
        {
            Debug.Log("Not enough materials!");
        }
    }
}
