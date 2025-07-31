
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class OpenMapTuto : MonoBehaviour
{
    public GameObject panelTuto;
    public TMP_Text textContenTuto;
    public ScrollRect scrollRect;

    public RawImage iconTuto;
    public Texture iconPlayer;
    public Texture iconQuest;
    public Texture iconShowQuest;
    public Texture iconCloseQuest;

    public int index = 0;
    //tham chieu
    OpenMap openMap;

    void Start()
    {
        
        //   Load chỉ số đã lưu
        index = MapTutoProgressHandler.LoadIndex();

        // Nếu đã index == 5 thì tắt UI ngay
        if (index >= 5)
        {
            panelTuto.SetActive(false);
            iconTuto.enabled = false;
            scrollRect.enabled = true;
            return;                   
        }

        // Khởi tạo UI như cũ
        panelTuto.SetActive(false);
        iconTuto.enabled = false;
        scrollRect.enabled = false;
        openMap = FindAnyObjectByType<OpenMap>();
    }

    void Update()
    {
       
       if(index == 0)
       {
            openMap. isTurnOffMapTuto = true;
            panelTuto.SetActive(true);
            iconTuto.enabled = true;
            textContenTuto.text = $"Đây là bản đồ. Nó sẽ giúp bạn xem vị trí hiện tại và các khu vực cần khám phá."; 
       }
        if (index == 1)
        {
            iconTuto.texture = iconPlayer;  
            textContenTuto.text = $"Dấu chấm sáng hiển thị vị trí hiện tại của bạn trên bản đồ.";
        }
        if (index == 2)
        {
            iconTuto.texture = iconQuest;
            textContenTuto.text = $"Biểu tượng này đánh dấu nhiệm vụ hiện tại của bạn.";
        }
        if (index == 3)
        {
            iconTuto.texture = iconShowQuest;
            textContenTuto.text = $"Đây là nội dung Nhiệm Vụ.";
        }
        if (index == 4)
        {
            iconTuto.texture = iconCloseQuest;
            textContenTuto.text = $"Nhấn nút [X] góc phải để tắt nội dung và bấm [M] quay lại trò chơi.";
        }
        if (index == 5)
        {
            panelTuto.SetActive(false);
            iconTuto.enabled = false;
            scrollRect.enabled = true;
            if(openMap != null)
            {
                openMap.isTurnOffMapTuto = false;
            }
            textContenTuto.text = $"";

            MapTutoProgressHandler.SaveIndex(5);
        }
       
    }

    public void ButtonSkipRight()
    {
        index++;
        if (index > 5)
        {
            index = 5;
          
        }
    }
    public void ButtonSkipLeft()
    {
        index--;
        if (index < 0)
        {
            index = 0;
        } 
    }
}
