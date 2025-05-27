using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class RanDomItem : MonoBehaviour
{
    public List<ItemBuffDrop> itemDrops = new List<ItemBuffDrop>();
    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }
    public void DropItem()
    {
        float rand = Random.Range(0f, 100f); // số ngẫu nhiên từ 0 đến 100
        float cumulative = 0f;

        foreach (ItemBuffDrop item in itemDrops)
        {
            
            cumulative += item.dropRate;
            if (rand <= cumulative)
            {
              
               inventoryManager.AddItem(
                    item.itemPrefabs.itemName,
                    item.itemPrefabs.quantity,
                    item.itemPrefabs.itemSprite, // Corrected argument
                    item.itemPrefabs.itemDescription,
                    item.itemPrefabs.itemType
                );
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DropItem();
            Destroy(gameObject);

        }
    }

}

[System.Serializable]
public class ItemBuffDrop
{
    public Item itemPrefabs;
    public float dropRate;
}
