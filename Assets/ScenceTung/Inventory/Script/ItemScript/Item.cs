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
            // ✅ Lưu item
            GameSaveData data = SaveManagerMan.LoadGame();
            data.inventoryItems.Clear();
            data.inventoryItemSos.Clear();
            foreach (var slot in inventoryManager.itemSlot)
            {
                if (!string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
                {
                    data.inventoryItemSos.Add(new SaveItemSO
                    {
                        itemName = slot.itemName,
                        quantity = slot.quantity,
                        itemType = slot.itemType.ToString(),
                        itemSprite = slot.itemSprite,
                        itemDescription = slot.itemDescription

                    });
                }
            }

            foreach (var slot in inventoryManager.equipmentSlot)
            {
                if (!string.IsNullOrEmpty(slot.itemName) && slot.quantity > 0)
                {
                    data.inventoryItems.Add(new SavedItemData
                    {
                        itemName = slot.itemName,
                        quantity = slot.quantity,
                        itemType = slot.itemType.ToString()
                    });
                }
            }

            // ✅ Cuối cùng, lưu lại
            SaveManagerMan.SaveGame(data);
            Debug.Log("Saved with " + data.inventoryItems.Count + " items");
            Debug.Log("Saved with " + data.inventoryItemSos.Count + " itemSOs");

            if (leftOverItems <= 0)

            Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }

}