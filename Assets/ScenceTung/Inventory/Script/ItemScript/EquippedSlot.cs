using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour
{
    //Appearance slot
    [SerializeField]
    private Image slotImage;

    [SerializeField]
    private TMP_Text slotName;


    //Slot Data
    [SerializeField]
    private ItemType itemType = new ItemType();

    private Sprite itemSprite;

    private string itemName;
    private string itemDescription;

    //other variables
    private bool slotIsUse;


    public void EquipGear(string itemName, Sprite itemSprite,string itemDescription)
    {
        //update image
        this.itemSprite = itemSprite;   
        slotImage.sprite = itemSprite;
        slotName.enabled = false;


        //update data
        this.itemName = itemName;
        this.itemDescription = itemDescription;

        slotIsUse = true;


    }


}
