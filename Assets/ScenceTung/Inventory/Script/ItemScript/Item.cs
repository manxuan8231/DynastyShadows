using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    private string itemName;
    [SerializeField]
    private int quantity;
    [SerializeField]
    private Sprite itemSprite;

    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;
    public ItemType itemType;
        


    void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int leftOverItems = inventoryManager.AddItem(itemName , quantity,itemSprite,itemDescription,itemType);

            if (leftOverItems <= 0)    
                Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }





}
