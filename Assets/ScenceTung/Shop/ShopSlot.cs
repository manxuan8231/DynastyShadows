using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
   public ItermShopData itermShopData;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public Image itemIcon;
    private int price;

    public void Initallize(ItermShopData newSO,int price)
    {
        itermShopData = newSO;
        itemNameText.text = itermShopData.itemName;
        itemIcon.sprite = itermShopData.itemIcon;
        this.price = price; 
        itemPriceText.text = price.ToString();


    }


}
