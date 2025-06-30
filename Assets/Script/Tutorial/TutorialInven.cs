using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInven : MonoBehaviour
{
    public GameObject panelTutoInven;
    public TextMeshProUGUI textConten;
    public float index = 0;
    //img
    public RawImage rawImage;
    public Texture step1;
    public Texture step2;
    public Texture step3;
    public Texture step4;
    public Texture step5;
    public Texture step6;
    public Texture step7;
    public Texture step8;
    public Texture step9;
    public Texture step10;
    public Texture step11;
    public Texture step12;
    public Texture step13;
    public Texture step14;


    //tham chieu
    TutorialManager tutorialManager;
    InventoryManager inventoryManager;
    OpenSkillTree openSkillTree;
    void Start()
    {
        tutorialManager = FindAnyObjectByType<TutorialManager>();
        inventoryManager = FindAnyObjectByType<InventoryManager>();
        openSkillTree = FindAnyObjectByType<OpenSkillTree>();
        panelTutoInven.SetActive(false);
       
        textConten.text = "";
        index = 1;
    }


    void Update()
    {
        if (tutorialManager.isTutorialInvenConten)
        {
            panelTutoInven.SetActive(true);
            //cho mo inven va skill tree
            inventoryManager.isOpenInventory = true;
            openSkillTree.isOpenSkillTree = true;
        }
        //chay cac step img 
        if (panelTutoInven.activeSelf && index == 1)
        {
            rawImage.texture = step1;
            textConten.text = "Click chuột trái dô đây để mở kho chứa các vật phẩm có thể uống được.";
        }
        if (panelTutoInven.activeSelf && index == 2)
        {
            rawImage.texture = step2;
            textConten.text = "Click chuột trái 2 lần vào bình máu để sử dụng.";
        }
        if (panelTutoInven.activeSelf && index == 3)
        {
            rawImage.texture = step3;
            textConten.text = "Bên đây là thông tin của bình máu.";
        }
        if (panelTutoInven.activeSelf && index == 4)
        {
            rawImage.texture = step4;
            textConten.text = "Click vào đây để xem thông tin của nhân vật và các trang bị.";
        }
        if (panelTutoInven.activeSelf && index == 5)
        {
            rawImage.texture = step5;
            textConten.text = "Đây là các ô chứa các trang bị nhặt được.";
        }
        if (panelTutoInven.activeSelf && index == 6)
        {
            rawImage.texture = step6;
            textConten.text = "Đây là các ô chứa để trang bị vật phẩm dô nhân vật.";
        }
        if (panelTutoInven.activeSelf && index == 7)
        {
            rawImage.texture = step7;
            textConten.text = "Click vào đây để mở bảng nâng cấp và mở khóa kỹ năng.";
        }
        if (panelTutoInven.activeSelf && index == 8)
        {
            rawImage.texture = step8;
            textConten.text = "Đây là điểm moon để mở khóa kỹ năng có thể có được sau khi làm nhiệm vụ và lên level.";
        }
        if (panelTutoInven.activeSelf && index == 9)
        {
            rawImage.texture = step9;
            textConten.text = "Click vào đây để mở bảng linh hoạt.";
        }
        if (panelTutoInven.activeSelf && index == 10)
        {
            rawImage.texture = step10;
            textConten.text = "Đây là các kỹ năng của linh hoạt có thể giúp nhân vật chuyển đổi kỹ năng cho tùy tình huống.";
        }
        if (panelTutoInven.activeSelf && index == 11)
        {
            rawImage.texture = step11;
            textConten.text = "Đây là các thông tin của kỹ năng và là chỗ mở khóa kỹ năng.";
        }
        if (panelTutoInven.activeSelf && index == 12)
        {
            rawImage.texture = step12;
            textConten.text = "Click vào đây để mở bảng cốt lõi.";
        }
        if (panelTutoInven.activeSelf && index == 13)
        {
            rawImage.texture = step13;
            textConten.text = "Đây là các kỹ năng có thể mở khóa được sau khi đủ level và điểm moon có thể nâng cấp để mở ra các tính năng mới.";
        }
        if (panelTutoInven.activeSelf && index == 14)
        {
            rawImage.texture = step14;
            textConten.text = "Đây là các thông tin của kỹ năng theo từng cấp.";
        }
        if (panelTutoInven.activeSelf && index == 15)
        {
            panelTutoInven.SetActive(false);
            textConten.text = "";
          tutorialManager.isCompleteInven=true;//danh dau hoan thanh tuto
        }
    }
    public void StartSkip() //skip qua
    {
        index++;
        if (index >= 15)
        {
            index = 15;
        }
    }
    public void BackSkip()
    {
        index--;
        if (index <= 1)
        {
            index = 1;
        }
    }
}
