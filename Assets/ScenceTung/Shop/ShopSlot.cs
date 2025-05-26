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

            Debug.Log("Đã mua vật phẩm: " + itermShopData.itemName);
        }
        else
        {
            Debug.Log("Không đủ vàng để mua " + itermShopData.itemName);
        }
    }

    public void SellItem()
    {

    }


}
