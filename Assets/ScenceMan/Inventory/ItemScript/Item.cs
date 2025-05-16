using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int quantity;
    [SerializeField]
    private Sprite itemImage;
    [TextArea]
    [SerializeField]
    private string itemDescription;
    public OpenInventory OpenInventory;
    public ItemSO itemSO;
    void Start()
    {
        OpenInventory = FindAnyObjectByType<OpenInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int leftOverItems = OpenInventory.AddItem(
                itemSO.itemName,
                quantity,
                itemSO.itemIcon,
                itemSO.itemDescription,
                itemSO
            );

            if (leftOverItems <= 0)
                Destroy(gameObject);
            else
                quantity = leftOverItems;
        }
    }
}
