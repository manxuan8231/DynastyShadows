using UnityEngine;

public class OpenCrafting : MonoBehaviour
{
    public GameObject craftingMenu;
    public InventoryManager inventoryManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKeyDown(KeyCode.C))
                CraftingMenu();
        }
    }
    void CraftingMenu()
    {
        if (craftingMenu.activeSelf)
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            craftingMenu.SetActive(false);
            inventoryManager.inventoryMenu.SetActive(false);
            inventoryManager.equipmentMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            craftingMenu.SetActive(true);
            inventoryManager.inventoryMenu.SetActive(false);
            inventoryManager.equipmentMenu.SetActive(false);
        }
    }


}
