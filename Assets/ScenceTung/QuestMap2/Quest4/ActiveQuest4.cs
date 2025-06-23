using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System.Collections;
public class ActiveQuest4 : MonoBehaviour
{
    public GameObject canvasTextHN;
    public TMP_Text textHN;
    public GameObject canvasQuest;
    public TMP_Text txtQuest;
    public Image imageQuest;
    public Sprite spriteQuest;
    public GameObject canvasNiceQuest;
    public TMP_Text txtMissionName;
    public TMP_Text txtStateText;
    public Image imageIcon;
    public Sprite iconNiceQuest;
    public TeleToMarket TeleToMarket;


    //tham chiếu đến các quest khác
    public ChangedWedather changedWedather;
    public AudioCanvasState AudioCanvasState;


    //bool
    public bool isActiveQuest4 = false;


    //Quest transform target
    public GameObject questPoint;
    void Start()
    {
        changedWedather = FindAnyObjectByType<ChangedWedather>();
        questPoint.SetActive(false);

        TeleToMarket = FindAnyObjectByType<TeleToMarket>();
    }

    // Update is called once per frame
    void Update()
    {
        if (changedWedather.isDoneQuest3 && !isActiveQuest4)
        { 
            StartCoroutine(Quest4Active());
        }
    }
    IEnumerator Quest4Active()
    {
        isActiveQuest4 = true; // Đặt cờ để biết nhiệm vụ đã được kích hoạt
        canvasTextHN.SetActive(true);
        textHN.text = "Tại sao không khí lại thay đổi như vậy???";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Có vẻ có người đã đụng chạm vào các vật phong ấn này…";
        yield return new WaitForSeconds(1.5f);
        textHN.text = "Ba vật phong ấn đó chắc phải có gì đó để nhận diện.\r\n";
        yield return new WaitForSeconds(2f);
        textHN.text = "Chuyện này ắt hẳn liên quan đến Tô Nam ?!!!";
        yield return new WaitForSeconds(2f);
        canvasTextHN.SetActive(false);
        yield return new WaitForSeconds(2f);
        canvasQuest.SetActive(true);
        txtQuest.text = "Nhiệm vụ: Tìm và phá hủy 3 Phong Ấn Cổ nằm ở khu chợ!";
        imageQuest.sprite = spriteQuest;
        yield return new WaitForSeconds(2f);
        canvasNiceQuest.SetActive(true);
        AudioCanvasState.PlayNewQuest();
        txtMissionName.text = "Nhiệm vụ: Tìm và phá hủy 3 Phong Ấn Cổ";
        txtStateText.text = "Bạn vừa nhận được nhiệm vụ mới!";
        imageIcon.sprite = iconNiceQuest;
        yield return new WaitForSeconds(5f);
        canvasNiceQuest.SetActive(false);
        questPoint.SetActive(true); // Kích hoạt điểm quest
        TeleToMarket.boxCollider.enabled = true; // Bật lại collider để có thể kích hoạt dịch chuyển

    }
}
