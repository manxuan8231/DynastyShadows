using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AwardQuest : MonoBehaviour
{
    public PlayerStatus playerStatus;
    public AudioCanvasState audioCanvasState;
    public TMP_Text award;
    public TMP_Text stateQuestion;
    public Image iconImage;
    public GameObject canvasAward;


    //award string quest
    public string awardQuest2_1 = "500 EXP";
    public string awardQuest2_2 = "100 Vàng";
    public Sprite iconSprite1;

    void Start()
    {
     playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    public void AwardQuest2()
    {
        StartCoroutine(CanvasQuest());
        playerStatus.currentExp += 500;
        playerStatus.expSlider.value = playerStatus.currentExp;
        playerStatus.gold += 100;
        playerStatus.goldQuantityTxt.text = playerStatus.gold.ToString();
    }    
    IEnumerator CanvasQuest()
    {
        canvasAward.SetActive(true);
        audioCanvasState.PlayAward();
        stateQuestion.text = "Thưởng nhiệm vụ";
        award.text = $"Phần thưởng của bạn là: {awardQuest2_1} + {awardQuest2_2} ";
        iconImage.sprite = iconSprite1;
        yield return new WaitForSeconds(3f);
        canvasAward.SetActive(false);
    }
}
