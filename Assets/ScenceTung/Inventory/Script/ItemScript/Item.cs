using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField]
    public string itemName;
    [SerializeField]
    public int quantity;
    [SerializeField]
    public Sprite itemSprite;

    [TextArea]
    [SerializeField]
    public string itemDescription;

    public InventoryManager inventoryManager;
    public ItemType itemType;
    private float timeToDestroy = 10;
    void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        Destroy(gameObject, timeToDestroy); // Destroy the item after a certain time

    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int leftOverItems = inventoryManager.AddItem(itemName , quantity,itemSprite,itemDescription,itemType);
            FindAnyObjectByType<ItemPickupNotifier>().ShowPickup(itemName, quantity);

            if (leftOverItems <= 0)

            Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }

}