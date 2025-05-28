using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;

public class RanDomItem : MonoBehaviour
{
    public List<ItemBuffDrop> itemDrops = new List<ItemBuffDrop>();
    private InventoryManager inventoryManager;
    public Animator animator;
    public GameObject destroyBox;
    public GameObject boxOrg;
    public AudioSource source;
    public BoxCollider boxCollider;
    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        animator = GetComponent<Animator>();
        destroyBox.SetActive(false);
        source = GetComponent<AudioSource>();
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
            boxOrg.SetActive(false);
            animator.SetTrigger("Destroy");
            source.Play();
            destroyBox.SetActive(true);
            DropItem();
            boxCollider.enabled = false;
            Destroy(gameObject,2f);

        }
    }

}

[System.Serializable]
public class ItemBuffDrop
{
    public Item itemPrefabs;
    public float dropRate;
}
