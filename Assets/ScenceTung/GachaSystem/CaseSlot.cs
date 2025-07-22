using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaseSlot : MonoBehaviour
{
    [Header("Case Slot Info")]
    private int price;
    public TMP_Text caseText;
    public TMP_Text priceText;
    public Image caseImage;
    public CaseSO caseSO;
   
    public GameObject gatchaMenu;
    public int id;
    [Header("Code tham chiếu")]
    [SerializeField]
    private PlayerStatus status;
    CaseScroll caseScroll;
    private void Start()
    {
        status = GameObject.Find("Stats").GetComponent<PlayerStatus>();
        caseScroll = GameObject.Find("CanvasGachaItem/PanelGacha/GatchaMain/Scroll").GetComponent<CaseScroll>();
    }

    public void Initallize(CaseSO newSO, int price)
    {
        caseSO = newSO;
        caseText.text = newSO.itemName;
        caseImage.sprite = newSO.caseImage;
        this.price = price;
        priceText.text = "Giá bán : " + price.ToString();
        id = newSO.id;

    }
    public void BuyItem()
    {
        if (status.gold >= price)
        {
            status.gold -= price;
            status.UpdateTextUIGold();
            caseScroll.hasKey = true;
            Debug.Log("Đã mua: " + caseSO.itemName);
            StartCoroutine(showGatchaMenu());

        }
        else
        {
            Debug.Log("Không đủ tiền để mua: " + caseSO.itemName);
        }

    }
    
    IEnumerator showGatchaMenu()
    {
        yield return new WaitForSeconds(01f);
        gatchaMenu.SetActive(true);
        yield return new WaitForSeconds(7f);
        gatchaMenu.SetActive(false);

    }


}
