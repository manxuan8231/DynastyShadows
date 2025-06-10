using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; 
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
            playerStatus.gold -= price;
            playerStatus.goldQuantityTxt.text = playerStatus.gold.ToString();

            inventoryManager.AddItem(
                itermShopData.itemName,
                1,
                itermShopData.itemIcon,
                 itermShopData.itemDescription,
                itermShopData.itemType
            );

            Debug.Log("đã mua: " + itermShopData.itemName);
        }
        else
        {
            Debug.Log("mày nghèo vl" + itermShopData.itemName);
        }
    }
  
    public void SellItem()
    {
        for (int i = 0; i < inventoryManager.itemSlot.Length; i++)
        {
            var slot = inventoryManager.itemSlot[i];

            if (slot.itemName == itermShopData.itemName && slot.quantity > 0)
            {
                playerStatus.gold += sellPrice;
                playerStatus.goldQuantityTxt.text = playerStatus.gold.ToString();

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
