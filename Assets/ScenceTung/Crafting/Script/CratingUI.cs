using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject recipeButtonPrefab;
    public Transform recipeButtonParent;

    public Image[] inputImages;
    public Image[] outputImages;
    public TMP_Text recipeNameText;
    public Button craftButton;

    private InventoryManager inventory;
    private ItemRecipeSO selectedRecipe;

    private void Start()
    {
        inventory = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        PopulateRecipes();
    }

    void PopulateRecipes()
    {
        foreach (var recipe in CraftingManager.Instance.allRecipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipeButtonParent);
            buttonObj.GetComponentInChildren<TMP_Text>().text = recipe.recipeName;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => OnRecipeSelected(recipe));
        }
    }

    public void OnRecipeSelected(ItemRecipeSO recipe)
    {
        selectedRecipe = recipe;

        if (recipeNameText != null)
            recipeNameText.text = recipe.recipeName;

        for (int i = 0; i < inputImages.Length; i++)
        {
            if (i < recipe.input.Length && recipe.input[i].itemOther != null)
            {
                inputImages[i].sprite = recipe.input[i].itemOther.itemSprite;
                inputImages[i].gameObject.SetActive(true);
            }
            else
            {
                inputImages[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < outputImages.Length; i++)
        {
            if (i < recipe.output.Length && recipe.output[i].itemOther != null)
            {
                outputImages[i].sprite = recipe.output[i].itemOther.itemSprite;
                outputImages[i].gameObject.SetActive(true);
            }
            else
            {
                outputImages[i].gameObject.SetActive(false);
            }
        }

        craftButton.interactable = CraftingManager.Instance.CanCraft(recipe, inventory);
    }

    public void OnCraftButtonPressed()
    {
        if (selectedRecipe != null)
        {
            CraftingManager.Instance.Craft(selectedRecipe, inventory);
            OnRecipeSelected(selectedRecipe); // Refresh UI
        }
    }
}
