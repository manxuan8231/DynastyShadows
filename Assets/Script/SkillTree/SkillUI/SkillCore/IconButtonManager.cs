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
    public Skill4Manager skill4Manager;
    void Start()
    {
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        skill4Manager = FindAnyObjectByType<Skill4Manager>();
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
            //skill1-------------
            case "DongCung1":
                previewText.text = "Đông cứng - Cấp 1";
                contenSkill.text = "Khi dùng skill có khả năng đông cứng kẻ địch trong 5 giây không gây sát thương có thời gian hồi chiêu là 50 giây.";
                skillBGs[0].enabled = true;
                
                break;

            case "DongCung2":
                previewText.text = "Đông cứng - Cấp 2";
                contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                skillBGs[1].enabled = true;
                break;

            case "DongCung3":
                previewText.text = "Đông cứng - Cấp 3";
                contenSkill.text = "Tăng thời gian đông cứng kẻ địch trong 10 giây";
                skillBGs[2].enabled = true;
                break;

            case "DongCung4":
                previewText.text = "Đông cứng - Cấp 4";
                contenSkill.text = "Gây 50 sát thương liên tục trong 10 giây";
                skillBGs[3].enabled = true;
                break;

            //skill2--------------
            case "TheAnh1":
                previewText.text = "Thế ảnh - Cấp 1";
                contenSkill.text = "Khi dùng Thế ảnh người chơi có thể tạo ra một bản thể ảo ảnh giúp đánh lạc hướng kẻ thù và " +
                    "chuyển sang trạng thái tàng hình trong 10 giây, " +
                    "khi nhấn tấn công thì player sẽ lao tới kẻ địch gần nhất chém 1 nhát chém và hủy trạng thái tàng hình, có thời gian hồi chiêu là 50 giây.";
                skillBGs[4].enabled = true;

                break;

            case "TheAnh2":
                previewText.text = "Thế ảnh - Cấp 2";
                contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                skillBGs[5].enabled = true;
                break;

            case "TheAnh3":
                previewText.text = "Thế ảnh - Cấp 3";
                contenSkill.text = "Tăng thời gian tàng hình lên 20 giây.";
                skillBGs[6].enabled = true;
                break;

            case "TheAnh4":
                previewText.text = "Thế ảnh - Cấp 4";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[7].enabled = true;
                break;

            //skill3--------------
            case "QuanDoanBongToi1":
                previewText.text = "Quân đoàn bóng tối - Cấp 1";
                contenSkill.text = "Khi dùng Thế ảnh người chơi có thể tạo ra một bản thể ảo ảnh giúp đánh lạc hướng kẻ thù và " +
                    "chuyển sang trạng thái tàng hình trong 10 giây, " +
                    "khi nhấn tấn công thì player sẽ lao tới kẻ địch gần nhất chém 1 nhát chém và hủy trạng thái tàng hình, có thời gian hồi chiêu là 50 giây.";
                skillBGs[8].enabled = true;

                break;

            case "QuanDoanBongToi2":
                previewText.text = "Quân đoàn bóng tối - Cấp 2";
                contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                skillBGs[9].enabled = true;
                break;

            case "QuanDoanBongToi3":
                previewText.text = "Quân đoàn bóng tối - Cấp 3";
                contenSkill.text = "Tăng thời gian tàng hình lên 20 giây.";
                skillBGs[10].enabled = true;
                break;

            case "QuanDoanBongToi4":
                previewText.text = "Quân đoàn bóng tối - Cấp 4";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[11].enabled = true;
                break;

            case "QuanDoanBongToi5":
                previewText.text = "Quân đoàn bóng tối - Cấp 4";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[12].enabled = true;
                break;

            case "QuanDoanBongToi6":
                previewText.text = "Quân đoàn bóng tối - Cấp 4";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[13].enabled = true;
                break;

            //skill4--------------
            case "PhanNhan1":
                previewText.text = "Phản nhãn - Cấp 1";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[14].enabled = true;
                break;

            case "PhanNhan2":
                previewText.text = "Phản nhãn - Cấp 2";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[15].enabled = true;
                break;

            case "PhanNhan3":
                previewText.text = "Phản nhãn - Cấp 3";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[16].enabled = true;
                break;

            case "PhanNhan4":
                previewText.text = "Phản nhãn - Cấp 4";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[17].enabled = true;
                break;

            case "PhanNhan5":
                previewText.text = "Phản nhãn - Cấp 5";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[18].enabled = true;
                break;

            case "PhanNhan6":
                previewText.text = "Phản nhãn - Cấp 6";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[19].enabled = true;
                break;

            case "PhanNhan7":
                previewText.text = "Phản nhãn - Cấp 7";
                contenSkill.text = "Trong thời gian tàng hình, bản thể ảo gây 200 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[20].enabled = true;
                break;


            default:
                previewText.text = "Thông tin chưa cập nhật.";
                contenSkill.text = "...";
                break;
        }
    }

    public void ClosePreview()
    {
        previewPanel.SetActive(false);
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
                //mo khoa de su dung 
              
                break;
            case "DongCung3":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung3");
                ColorUnlockIcon();
                //mo khoa de su dung 
               
                break;
            case "DongCung4":
                Debug.Log("Đã mở khóa kỹ năng: ĐôngCung4");
                ColorUnlockIcon();
               
                break;

            //skill 2 the ảnh --------------------------------
            case "TheAnh1":
                Debug.Log("Đã mở khóa kỹ năng: TheAnh1");
                ColorUnlockIcon();
                //mo khoa de su dung 
                skill2Manager.isUnlockSkill2 = true;
                skill2Manager.iconSkill2.SetActive(true);
                break;
            case "TheAnh2":
                Debug.Log("Đã mở khóa kỹ năng: TheAnh2");
                ColorUnlockIcon();
                //mo khoa de su dung 
               
                break;
            case "TheAnh3":
                Debug.Log("Đã mở khóa kỹ năng: TheAnh3");
                ColorUnlockIcon();
                //mo khoa de su dung 
               
                break;
            case "TheAnh4":
                Debug.Log("Đã mở khóa kỹ năng: TheAnh4");
                ColorUnlockIcon();

                break;

            //skill 3 quan doan bong ma --------------------------------
            case "QuanDoanBongToi1":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa1");
                ColorUnlockIcon();
                //mo khoa de su dung 
                skill3Manager.isUnlockSkill3 = true;
                skill3Manager.iconSkill3.SetActive(true);
                break;
            case "QuanDoanBongToi2":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa2");
                ColorUnlockIcon();
              
                break;
            case "QuanDoanBongToi3":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa3");
                ColorUnlockIcon();
              
                break;
            case "QuanDoanBongToi4":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                ColorUnlockIcon();

                break;
            case "QuanDoanBongToi5":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                ColorUnlockIcon();

                break;
            case "QuanDoanBongToi6":
                Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                ColorUnlockIcon();

                break;

            //skill 4 Phản nhản --------------------------------
            case "PhanNhan1":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan1");
                ColorUnlockIcon();
                //mo khoa de su dung 
                skill4Manager.isUnlockSkill4 = true;
                skill4Manager.iconSkill4.SetActive(true);
                break;
            case "PhanNhan2":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan2");
                ColorUnlockIcon();

                break;
            case "PhanNhan3":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan3");
                ColorUnlockIcon();

                break;
            case "PhanNhan4":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan4");
                ColorUnlockIcon();

                break;
            case "PhanNhan5":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan5");
                ColorUnlockIcon();

                break;
            case "PhanNhan6":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan6");
                ColorUnlockIcon();

                break;
            case "PhanNhan7":
                Debug.Log("Đã mở khóa kỹ năng: PhanNhan7");
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
            //skill1
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
            //skill2
            case "TheAnh1":
                index = 4;
                break;
            case "TheAnh2":
                index = 5;
                break;
            case "TheAnh3":
                index = 6;
                break;
            case "TheAnh4":
                index = 7;
                break;
            //skill3
            case "QuanDoanBongToi1":
                index = 8;
                break;
            case "QuanDoanBongToi2":
                index = 9;
                break;
            case "QuanDoanBongToi3":
                index = 10;
                break;
            case "QuanDoanBongToi4":
                index = 11;
                break;
            case "QuanDoanBongToi5":
                index = 12;
                break;
            case "QuanDoanBongToi6":
                index = 13;
                break;
                //slill4
            case "PhanNhan1":
                index = 14;
                break;
            case "PhanNhan2":
                index = 15;
                break;
            case "PhanNhan3":
                index = 16;
                break;
            case "PhanNhan4":
                index = 17;
                break;
            case "PhanNhan5":
                index = 18;
                break;
            case "PhanNhan6":
                index = 19;
                break;
            case "PhanNhan7":
                index = 20;
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
