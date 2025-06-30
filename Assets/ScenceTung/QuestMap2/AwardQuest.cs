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
    void Start()
    {
     playerStatus = FindAnyObjectByType<PlayerStatus>();
     InventoryManager = GameObject.Find("CanvasInventory").GetComponent<InventoryManager>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            AwardQuest2();
        }
    }
    public void AwardQuest2()
    {
        StartCoroutine(CanvasQuest());
        playerStatus.currentExp += 1000;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 100;
        playerStatus.goldQuantityTxt.text = playerStatus.gold.ToString();
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
}
[System.Serializable]
public class ItemQuest
{
  public  Item itemPrefabsQuest;

}