using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AwardQuest : MonoBehaviour
{
    public List<ItemQuest> itemQuests = new List<ItemQuest>();
    public PlayerStatus playerStatus;
    public AudioCanvasState audioCanvasState;
    public TMP_Text award;
    public TMP_Text stateQuestion;
    public Image iconImage;
    public GameObject canvasAward;
    public InventoryManager InventoryManager;

    //award string quest
    public string awardQuest2_1 = "1000 EXP";
    public string awardQuest2_2 = "100 Vàng";
    public string awardQuest2_3 = "Mảnh kĩ năng \" Khiên chắn \"";
    public Sprite iconSprite1;


    //award quest 4
    public string awardQuest4_1 = "2000 EXP";
    public string awardQuest4_2 = "200 Vàng";
    public string awardQuest4_3 = "Mảnh kĩ năng \" Truy tìm dấu l \"";
    public Sprite iconSprite2;

    //Award Quest 6
    public string awardQuest6_1 = "3000 EXP";   
    public string awardQuest6_2 = "300 Vàng";
    public string awardQuest6_3 = " Mảnh vật bị nguyền rủa";
    public Sprite iconSprite3;

    //Item Quest 7 
    public string awardQuest7_1 = "5000 EXP";
    public string awardQuest7_2 = "500 Vàng";
    public string awardQuest7_3 = "Sách \"Thánh Ngôn \"";
    public Sprite iconSprite4;
    void Start()
    {
     playerStatus = FindAnyObjectByType<PlayerStatus>();
     InventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
    }
  
    public void AwardQuest2()
    {
        StartCoroutine(CanvasQuest());
        playerStatus.currentExp += 1000;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 100;
        playerStatus.UpdateTextUIGold();
       InventoryManager.AddItem(itemQuests[0].itemPrefabsQuest.itemName,
            itemQuests[0].itemPrefabsQuest.quantity,
            itemQuests[0].itemPrefabsQuest.itemSprite,
            itemQuests[0].itemPrefabsQuest.itemDescription,
            itemQuests[0].itemPrefabsQuest.itemType
        );
        Debug.Log("Thưởng nhiệm vụ 2 thành công");  

    }    
   
    IEnumerator CanvasQuest()
    {
        canvasAward.SetActive(true);
        audioCanvasState.PlayAward();
        stateQuestion.text = "Thưởng nhiệm vụ";
        award.text = $"Phần thưởng của bạn là: {awardQuest2_1} + {awardQuest2_2} + {awardQuest2_3} ";
        iconImage.sprite = iconSprite1;
        yield return new WaitForSeconds(3f);
        canvasAward.SetActive(false);
    }
    public void AwardQuest4()
    {
        StartCoroutine(CanvasQuest4());
        playerStatus.currentExp += 2000;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 200;
        playerStatus.UpdateTextUIGold();
        InventoryManager.AddItem(itemQuests[1].itemPrefabsQuest.itemName,
             itemQuests[1].itemPrefabsQuest.quantity,
             itemQuests[1].itemPrefabsQuest.itemSprite,
             itemQuests[1].itemPrefabsQuest.itemDescription,
             itemQuests[1].itemPrefabsQuest.itemType
         );
        Debug.Log("Thưởng nhiệm vụ 4 thành công");
    }
    IEnumerator CanvasQuest4()
    {
        canvasAward.SetActive(true);
        audioCanvasState.PlayAward();
        stateQuestion.text = "Thưởng nhiệm vụ";
        award.text = $"Phần thưởng của bạn là: {awardQuest4_1} + {awardQuest4_2} + {awardQuest4_3} ";
        iconImage.sprite = iconSprite2;
        yield return new WaitForSeconds(3f);
        canvasAward.SetActive(false);
    }
    public void AwardQuest6()
    {
        StartCoroutine(CanvasQuest6());
        playerStatus.currentExp += 3000;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 300;
        playerStatus.UpdateTextUIGold();
        InventoryManager.AddItem(itemQuests[2].itemPrefabsQuest.itemName,
             itemQuests[2].itemPrefabsQuest.quantity,
             itemQuests[2].itemPrefabsQuest.itemSprite,
             itemQuests[2].itemPrefabsQuest.itemDescription,
             itemQuests[2].itemPrefabsQuest.itemType
         );
        Debug.Log("Thưởng nhiệm vụ 6 thành công");
    }
    IEnumerator CanvasQuest6()
    {
        canvasAward.SetActive(true);
        audioCanvasState.PlayAward();
        stateQuestion.text = "Thưởng nhiệm vụ";
        award.text = $"Phần thưởng của bạn là: {awardQuest6_1} + {awardQuest6_2} + {awardQuest6_3} ";
        iconImage.sprite = iconSprite3;
        yield return new WaitForSeconds(3f);
        canvasAward.SetActive(false);
    }

    public void AwardQuest7()
    {
        StartCoroutine(CanvasQuest7());
        playerStatus.currentExp += 5000;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 10000;
        playerStatus.UpdateTextUIGold();
        InventoryManager.AddItem(itemQuests[3].itemPrefabsQuest.itemName,
             itemQuests[3].itemPrefabsQuest.quantity,
             itemQuests[3].itemPrefabsQuest.itemSprite,
             itemQuests[3].itemPrefabsQuest.itemDescription,
             itemQuests[3].itemPrefabsQuest.itemType
         );
        Debug.Log("Thưởng nhiệm vụ 7 thành công");
    }
    IEnumerator CanvasQuest7()
    {
        canvasAward.SetActive(true);
        audioCanvasState.PlayAward();
        stateQuestion.text = "Thưởng nhiệm vụ";
        award.text = $"Phần thưởng của bạn là: {awardQuest7_1} + {awardQuest7_2} + {awardQuest7_3} ";
        iconImage.sprite = iconSprite4;
        yield return new WaitForSeconds(3f);
        canvasAward.SetActive(false);
    }
}
[System.Serializable]
public class ItemQuest
{
  public  Item itemPrefabsQuest;

}