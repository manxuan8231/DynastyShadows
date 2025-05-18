using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public GameObject inventoryLogo;
    private bool isInventoryOpen = false;



    //gọi hàm
    public ItemSlot[] itemSlot; // Array of item slots
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory") && isInventoryOpen)
        {
            Time.timeScale = 1f; // Resume the game
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
            inventoryMenu.SetActive(false);
            inventoryLogo.SetActive(false);
            isInventoryOpen = false;
        }
        else if (Input.GetButtonDown("Inventory") && !isInventoryOpen)
        {
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
            inventoryMenu.SetActive(true);
            inventoryLogo.SetActive(true);
            isInventoryOpen = true;
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        // Implement your logic to add the item to the inventory
        Debug.Log("Item added: " + itemName + ", Quantity: " + quantity + "Sprite" + itemSprite);
        for (int i = 0; i < itemSlot.Length; i++)
        {
            //if (!itemSlot[i].isFull && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
             if (itemSlot[i].isFull == false && (itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0))
            {
              int leftOverItems =  itemSlot[i].AddItem(itemName, quantity, itemSprite,itemDescription);
                if(leftOverItems > 0) 
                leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                return leftOverItems;
               
            }
        }
        return quantity;

    }

    public void DeselectedAllSLot()
    {
        for (int i = 0; i < itemSlot.Length;i++)
        {
           
            itemSlot[i].selectedItem.SetActive(false);
            itemSlot[i].isSelected = false;
        }
    }
}
