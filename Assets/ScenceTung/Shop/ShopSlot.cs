using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopSlot : MonoBehaviour
{
    public ItermShopData itermShopData;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public TMP_Text sellPriceText;
    public Image itemIcon;
    private int price;
    private int sellPrice;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip sellSound;
    [SerializeField] private AudioClip notEnoughGoldSound;

private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    public void Initallize(ItermShopData newSO, int price,int sellPrice)
    {
        itermShopData = newSO;
        itemNameText.text = itermShopData.itemName;
        itemIcon.sprite = itermShopData.itemIcon;
        this.price = price;
        itemPriceText.text = price.ToString();
        this.sellPrice = sellPrice;
        sellPriceText.text = sellPrice.ToString();
    }

    public void BuyItem()
    {
        if (playerStatus.gold >= price)
        {
            audioSource.PlayOneShot(buySound);
            playerStatus.gold -= price;
            playerStatus.UpdateTextUIGold();

            inventoryManager.AddItem(
                itermShopData.itemName,
                1,
                itermShopData.itemIcon,
                 itermShopData.itemDescription,
                itermShopData.itemType
            );
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
            Debug.Log("đã mua: " + itermShopData.itemName);
        }
        else
        {
            audioSource.PlayOneShot(notEnoughGoldSound);
            Debug.Log("mày nghèo vl" + itermShopData.itemName);
        }
    }
  
    public void SellItem()
    {
        audioSource.PlayOneShot(sellSound);
        for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
        {
            var slot = inventoryManager.itemSlot[i];

            if (slot.itemName == itermShopData.itemName && slot.quantity > 0)
            {
                playerStatus.gold += sellPrice;
                playerStatus.UpdateTextUIGold();

                slot.quantity--;

                if (slot.quantity <= 0)
                {
                    slot.EmptySlot();
                }
                else
                {
                    Debug.Log("Đã bán 1 " + itermShopData.itemName + " từ slot " + i);
                }

               
                return;
            }
        }

        Debug.Log("Không còn " + itermShopData.itemName + " trong kho để bán.");
    }

}
