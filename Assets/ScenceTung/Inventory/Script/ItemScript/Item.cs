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

    private InventoryManager inventoryManager;
    public ItemType itemType;
    private EquipmentSOLibrary equipmentSOLibrary;


    void Start()
    {
        inventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
        equipmentSOLibrary = GameObject.Find("CanvasInventory").GetComponent<EquipmentSOLibrary>();

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
