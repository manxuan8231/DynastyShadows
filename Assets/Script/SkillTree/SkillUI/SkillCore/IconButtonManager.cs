using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconButtonManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject previewPanel;// Panel hiển thị thông tin kỹ năng
    public TextMeshProUGUI previewText;   // Tên kỹ năng
    public TextMeshProUGUI contenSkill;     // Mô tả kỹ năng
    public GameObject buttonUnlock;

    [Header("Skill Icon BG")]
    public Image[] buttonIconColor;//icon sau khi mo khoa skill 
    public Image[] skillBGs; // icon nền 

    private string currentSkillID = ""; // Lưu skill đang được chọn

    public AudioSource skillAudioSource;
    public AudioClip buttonClick;

    //tham chieu
    [Header("Tham chieu")]
    public Skill1Manager skill1Manager;
    public Skill2Manager skill2Manager;
    public Skill3Manager skill3Manager;
    void Start()
    {
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        
        previewPanel.SetActive(false);
        HideAllHighlights();//effect
        skillAudioSource = GetComponent<AudioSource>();
    }

    public void ShowPreview(string iconID)
    {
        skillAudioSource.PlayOneShot(buttonClick);
        previewPanel.SetActive(true);
        HideAllHighlights(); // Tắt hết highlight trước
        currentSkillID = iconID; // Lưu ID đang chọn để dùng trong unlockSkill

        switch (iconID)
        {
            //skill1
            case "DongCung1":
                previewText.text = "Đông cứng";
                contenSkill.text = "Khi dùng skill có khả năng đông cứng kẻ địch trong 5 giây có thời gian hồi chiêu là 50 giây không gây sát thương";
                skillBGs[0].enabled = true;
                
                break;

            case "DongCung2":
                previewText.text = "Đông cứng";
                contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                skillBGs[1].enabled = true;
                break;

            case "DongCung3":
                previewText.text = "Đông cứng";
                contenSkill.text = "Tăng thời gian đông cứng kẻ địch trong 10 giây";
                skillBGs[2].enabled = true;
                break;

            case "DongCung4":
                previewText.text = "Đông cứng";
                contenSkill.text = "Gây 50 sát thương liên tục trong 10 giây";
                skillBGs[3].enabled = true;
                break;

            default:
                previewText.text = "Thông tin chưa cập nhật.";
                contenSkill.text = "";
                break;
        }
    }
    //mo khoa skill
    public void UnlockSkill1()
    {
        //kiểm tra ID và mở tính năng
        switch (currentSkillID)
        {
            case "DongCung1":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung1");
                ColorUnlockIcon();
                //mo khoa de su dung skill1
                skill1Manager.isUnlockSkill1 = true;
                skill1Manager.iconSkill1.SetActive(true);
                break;
            case "DongCung2":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung2");           
                ColorUnlockIcon();
                //mo khoa de su dung skill2
                skill2Manager.isUnlockSkill2 = true;
                skill2Manager.iconSkill2.SetActive(true);
                break;
            case "DongCung3":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung3");
                ColorUnlockIcon();
                //mo khoa de su dung skill2
                skill3Manager.isUnlockSkill3 = true;
                skill3Manager.iconSkill3.SetActive(true);
                break;
            case "DongCung4":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung4");
                ColorUnlockIcon();

                break;

              
        }
     

    }

    //doi mau sau khi mo skill
    public void ColorUnlockIcon()
    {
        int index = -1;

        switch (currentSkillID)
        {
            case "DongCung1":
                index = 0; 
                break;
            case "DongCung2": 
                index = 1; 
                break;
            case "DongCung3": 
                index = 2; 
                break;
            case "DongCung4": 
                index = 3; 
                break;
        }

        if (index >= 0 && index < buttonIconColor.Length && buttonIconColor[index] != null)
        {
            buttonIconColor[index].color = Color.white;
        }
    }

    //hieu ung khi nhan nut
    private void HideAllHighlights()
    {
        foreach (Image bg in skillBGs)
        {
            if (bg != null)
                bg.enabled = false;
        }
    }
}
