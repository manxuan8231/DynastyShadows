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
    void Start()
    {
        OpenInventory = FindAnyObjectByType<OpenInventory>();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenInventory.AddItem(itemName, quantity, itemImage, itemDescription);
            Destroy(gameObject);
        }
    }
}
