using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public GameObject recipeButtonPrefab;
    public Transform recipeButtonParent;

    public Image[] inputImages;
    public TMP_Text[] inputAmountTexts;

    public Image[] outputImages;
    public TMP_Text recipeNameText;
    public TMP_Text[] outputAmountTexts;

    public Button craftButton;

    private InventoryManager inventory;
    private ItemRecipeSO selectedRecipe;
    public TMP_Text coinCostText;

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
            if (i < recipe.input.Length && recipe.input[i].item != null)
            {
                inputImages[i].sprite = recipe.input[i].item.itemIcon;
                inputImages[i].gameObject.SetActive(true);

                // Gán text số lượng
                if (inputAmountTexts != null && i < inputAmountTexts.Length)
                {
                    inputAmountTexts[i].text = $"{recipe.input[i].count}x {recipe.input[i].item.itemName}";
                    inputAmountTexts[i].gameObject.SetActive(true);
                }
            }
            else
            {
                inputImages[i].gameObject.SetActive(false);
                if (inputAmountTexts != null && i < inputAmountTexts.Length)
                    inputAmountTexts[i].gameObject.SetActive(false);
            }
        }


        for (int i = 0; i < outputImages.Length; i++)
        {
            if (i < recipe.output.Length && recipe.output[i].item != null)
            {
                outputImages[i].sprite = recipe.output[i].item.itemSprite;
                outputImages[i].gameObject.SetActive(true);

                // Gán text số lượng và tên
                if (outputAmountTexts != null && i < outputAmountTexts.Length)
                {
                    outputAmountTexts[i].text = $"{recipe.output[i].count}x {recipe.output[i].item.itemName}";
                    outputAmountTexts[i].gameObject.SetActive(true);
                }
            }
            else
            {
                outputImages[i].gameObject.SetActive(false);
                if (outputAmountTexts != null && i < outputAmountTexts.Length)
                    outputAmountTexts[i].gameObject.SetActive(false);
            }
        }


        craftButton.interactable = CraftingManager.Instance.CanCraft(recipe, inventory);



    }

    public void OnCraftButtonPressed()
    {
        if (selectedRecipe != null)
        {
            CraftingManager.Instance.Craft(selectedRecipe, inventory);
            OnRecipeSelected(selectedRecipe); // 🔄 cập nhật lại UI
        }
    }
}
