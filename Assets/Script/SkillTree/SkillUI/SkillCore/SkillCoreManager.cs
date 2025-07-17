using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SkillCoreManager : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject previewPanel;// Panel hiển thị thông tin kỹ năng
    public TextMeshProUGUI previewText;   // Tên kỹ năng
    public TextMeshProUGUI contenSkill;     // Mô tả kỹ năng
    public TextMeshProUGUI scoreUpgradeText;//so diem can de nang cap
    public GameObject buttonUnlock;//nut mo khoa skill

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
    public PlayerStatus playerStatus;

    [Header("TextureTutorial")]
    public Camera[] cameras; // Mảng chứa các camera hướng dẫn
    public RawImage textureTutorial; // Hiển thị camera hướng dẫn
    public Texture spriteDongCung;
    public Texture spriteTheAnh;
    public Texture spriteQuanDoanBongToi;
    public Texture spritePhanNhan;

    //nâng cấp theo trình tự
    public float turnInSkill1 = 0f;
    public float turnInSkill2 = 0f;
    public float turnInSkill3 = 0f;
    public float turnInSkill4 = 0f;

    public GameObject panelSkill1;
    public GameObject panelSkill2;
    public GameObject panelSkill3;
    public GameObject panelSkill4;
    void Start()
    {
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        skill4Manager = FindAnyObjectByType<Skill4Manager>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        previewPanel.SetActive(false);
        HideAllHighlights();//effect
        skillAudioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(playerStatus.currentLevel >= 5)
        {
            panelSkill1.SetActive(false);//an panel neu du cap
        }
        if (playerStatus.currentLevel >= 10)
        {
            panelSkill2.SetActive(false);//an panel neu du cap
        }
        if (playerStatus.currentLevel >= 15)
        {
            panelSkill3.SetActive(false);//an panel neu du cap
        }
        if (playerStatus.currentLevel >= 20)
        {
            panelSkill4.SetActive(false);//an panel neu du cap
        }
        SimulateUnlockAllSkills(); //giả lập mở khóa tất cả kỹ năng
    }
    private void OnDisable()
    {
        HideAllHighlights(); // Khi tắt panel thì ẩn tất cả hiệu ứng highlight
        previewPanel.SetActive(false); // Ẩn panel preview khi tắt
       
    }

    public void ShowPreview(string iconID)
    {
        skillAudioSource.PlayOneShot(buttonClick);
        previewPanel.SetActive(true);
        HideAllHighlights(); // Tắt hết highlight trước
        CameraPreview(); // Hiển thị camera hướng dẫn nếu có
        currentSkillID = iconID; // Lưu ID đang chọn để dùng trong unlockSkill
        buttonUnlock.SetActive(true);
        switch (iconID) 
        {
            //skill1-------------
            case "DongCung1":
                if(playerStatus.currentLevel >= 5)
                {
                   cameras[0].gameObject.SetActive(true); // Hiển thị camera hướng dẫn cho kỹ năng Đông Cứng
                    textureTutorial.texture = spriteDongCung;

                    panelSkill1.SetActive(false);//an panel neu du cap
                    previewText.text = "Đông cứng - Cấp 1";
                    contenSkill.text = "Khi dùng skill có khả năng đông cứng kẻ địch trong 5 giây không gây sát thương có thời gian hồi chiêu là 50 giây.";
                    skillBGs[0].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }
                else
                {
                    panelSkill1.SetActive(true);//hien panel skill1 nếu chưa đủ cấp độ
                    previewPanel.SetActive(false);
                    return; // Nếu chưa đủ cấp độ thì không hiển thị gì
                }
                if (skill1Manager.isUnlockSkill1 == true)
                {
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;

            case "DongCung2":
            if(turnInSkill1 >= 1)//phải mở khóa skill1 cấp 1 mới có thể coi cấp 2
                {
                    cameras[0].gameObject.SetActive(true); // Hiển thị camera hướng dẫn cho kỹ năng Đông Cứng
                    textureTutorial.texture = spriteDongCung;
                    previewText.text = "Đông cứng - Cấp 2";
                contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                skillBGs[1].enabled = true;
                scoreUpgradeText.text = "/2";//so diem can de nang cap
                } 
                else
                {
                    previewPanel.SetActive(false);
                }
                //bat su kien da mo khoa skill1 de an buttonUnlock
                if (skill1Manager.cooldownSkill == 35f)
                {
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;

            case "DongCung3":
                if(turnInSkill1 >= 1)
                {
                    cameras[0].gameObject.SetActive(true); // Hiển thị camera hướng dẫn cho kỹ năng Đông Cứng
                    textureTutorial.texture = spriteDongCung;
                    previewText.text = "Đông cứng - Cấp 3";
                contenSkill.text = "Tăng thời gian đông cứng kẻ địch trong 10 giây";
                skillBGs[2].enabled = true;
                 scoreUpgradeText.text = "/2";//so diem can de nang cap
                }
                else
                {
                    previewPanel.SetActive(false);
                }    
                if(skill1Manager.timeSkill1 == 10f)
                {
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;

            case "DongCung4":
                if(turnInSkill1 >= 3)
                {
                    cameras[0].gameObject.SetActive(true); // Hiển thị camera hướng dẫn cho kỹ năng Đông Cứng
                    textureTutorial.texture = spriteDongCung;
                    previewText.text = "Đông cứng - Cấp 4";
                contenSkill.text = "Gây sát thương sau khi bị đông cứng, dựa vào sát thương bạo kích của player không cần tỷ lệ bạo.";
                skillBGs[3].enabled = true;   
                scoreUpgradeText.text = "/4";//so diem can de nang cap
                } 
                else
                {
                    previewPanel.SetActive(false);
                }
                if (skill1Manager.isDamaged == true)
                {
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;

            //skill2-----------------------------------------
            case "TheAnh1":
                if(playerStatus.currentLevel >= 10)
                {
                    textureTutorial.texture = spriteTheAnh; // Hiển thị camera hướng dẫn
                    cameras[1].gameObject.SetActive(true);
                    panelSkill2.SetActive(false);
                    previewText.text = "Thế ảnh - Cấp 1";
                    contenSkill.text = "Khi dùng Thế ảnh người chơi có thể tạo ra một bản thể ảo ảnh giúp đánh lạc hướng kẻ thù" +
                        " và chuyển sang trạng thái thế ảnh trong 10 giây, " +
                        "khi nhấn tấn công thì player sẽ lao tới kẻ địch gần nhất chém 1 nhát chém và hủy trạng thái, có thời gian hồi chiêu là 50 giây.";
                    skillBGs[4].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }
                else
                {
                    previewPanel.SetActive(false);
                    panelSkill2.SetActive(true);
                }
                if (skill2Manager.isUnlockSkill2 == true)
                {
                    buttonUnlock.SetActive(false);
                }
                    break;

            case "TheAnh2":
             if(turnInSkill2 >= 1)
                {
                    textureTutorial.texture = spriteTheAnh; // Hiển thị camera hướng dẫn
                    cameras[1].gameObject.SetActive(true);
                    previewText.text = "Thế ảnh - Cấp 2";
                    contenSkill.text = "Giảm thời gian hồi chiêu còn 35 giây";
                    skillBGs[5].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }             
                else{
                previewPanel.SetActive(false);
                }
                if (skill2Manager.skillCooldown == 35)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "TheAnh3":
                if(turnInSkill2 >= 2)
                {
                    textureTutorial.texture = spriteTheAnh; // Hiển thị camera hướng dẫn
                    cameras[1].gameObject.SetActive(true);
                    previewText.text = "Thế ảnh - Cấp 3";
                    contenSkill.text = "Tăng thời gian trạng thái thế ảnh lên 15 giây";
                    skillBGs[6].enabled = true;
                    scoreUpgradeText.text = "/3";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if(skill2Manager.timeSkill2 == 15f)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "TheAnh4":
            if(turnInSkill2 >= 3)
            {
                    textureTutorial.texture = spriteTheAnh; // Hiển thị camera hướng dẫn
                    cameras[1].gameObject.SetActive(true);
                    previewText.text = "Thế ảnh - Cấp 4";
                contenSkill.text = "Khi hết thời gian trạng thái thế ảnh, bản thể ảo sẽ phát nổ gây 1000 sát thương cho kẻ địch ở gần trước khi biến mất.";
                skillBGs[7].enabled = true;
                scoreUpgradeText.text = "/4";//so diem can de nang cap
            }
                else{
                previewPanel.SetActive(false);
            }
                if (skill2Manager.isExplosionSkill2 == true)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            //skill3--------------
            case "QuanDoanBongToi1":
                if(playerStatus.currentLevel >= 15)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    panelSkill3.SetActive(false);//an panel neu  du cap
                    previewText.text = "Quân đoàn bóng tối - Cấp 1";
                    contenSkill.text = "Khi dùng kỹ năng thì nguời chơi sẽ tạo ra 4 phân thân giúp hỗ trợ, " +
                    "có sát thương theo chỉ số hiện tại của người chơi và có thời gian tồn tại là 30 giây, " +
                    "có thời gian hồi chiêu là 100 giây.";
                    skillBGs[8].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }
                else
                {
                    previewPanel.SetActive(false);
                    panelSkill3.SetActive(true);//hien panel neu chua du cap
                    return; // Nếu chưa đủ cấp độ thì không hiển thị gì
                }if (skill3Manager.isUnlockSkill3 == true)
                {
                    buttonUnlock.SetActive(false);//ẩn panel nếu đã mở khóa
                }
                    break;

            case "QuanDoanBongToi2":
                if(turnInSkill3 >= 1)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    previewText.text = "Quân đoàn bóng tối - Cấp 2";
                    contenSkill.text = "Giảm thời gian hồi chiêu còn 70 giây";
                    skillBGs[9].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                   
                }else{
                    previewPanel.SetActive(false);
                }if(skill3Manager.cooldownSkill == 70f)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "QuanDoanBongToi3":
                if(turnInSkill3 >= 1)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    previewText.text = "Quân đoàn bóng tối - Cấp 3";
                    contenSkill.text = "Tăng thời gian tồn tại phân thân lên 40 giây.";
                    skillBGs[10].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                   
                }else{
                    previewPanel.SetActive(false);
                }if(skill3Manager.timeSkill3 == 40f)
                {
                    buttonUnlock.SetActive(false);
                }

                break;

            case "QuanDoanBongToi4":
                if(turnInSkill3 >= 3)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    previewText.text = "Quân đoàn bóng tối - Cấp 4";
                    contenSkill.text = "Tăng sát thương của quân đoàn lên x2 so với player";
                    skillBGs[11].enabled = true;
                    scoreUpgradeText.text = "/4";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if(skill3Manager.isDamaged == true)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "QuanDoanBongToi5":
                if(turnInSkill3 >= 4)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    previewText.text = "Quân đoàn bóng tối - Cấp 5";
                    contenSkill.text = "Tăng số lượng phân thân lên 6.";
                    skillBGs[12].enabled = true;
                    scoreUpgradeText.text = "/5";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if (skill3Manager.playerCount == 6)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "QuanDoanBongToi6":
                if(turnInSkill3 >= 4)
                {
                    textureTutorial.texture = spriteQuanDoanBongToi; // Hiển thị camera hướng dẫn
                    cameras[2].gameObject.SetActive(true);
                    previewText.text = "Quân đoàn bóng tối - Cấp 6";
                    contenSkill.text = "Khi mở khóa cấp độ này thì khi người chơi dùng các kỹ năng liên quan đến 'Linh Hoạt' thì các phân thân sẽ bắt trước dùng theo.";
                    skillBGs[13].enabled = true;
                    scoreUpgradeText.text = "/5";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }
                break;

            //skill4--------------
            case "PhanNhan1":
                if (playerStatus.currentLevel >= 20)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    panelSkill4.SetActive(false);//an panel neu  du cap
                    previewText.text = "Phản nhãn - Cấp 1";
                    contenSkill.text = "Khi dùng kỹ năng thì player sẽ chuyển sang trạng thái phản nhãn trong vòng 25 giây, giúp hồi đầy máu, " +
                        "năng lượng và giúp đánh nhanh hơn.";
                    skillBGs[14].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }
                else
                {
                    previewPanel.SetActive(false);
                    panelSkill4.SetActive(true);//hien panel neu chua du cap
                    return; // Nếu chưa đủ cấp độ thì không hiển thị gì
                }if(skill4Manager.isUnlockSkill4 == true)
                {
                    buttonUnlock.SetActive(false);
                }
                    break;

            case "PhanNhan2":
                if(turnInSkill4 >= 1)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 2";
                    contenSkill.text = "Giảm thời gian hồi chiêu còn 70 giây.";
                    skillBGs[15].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if (skill4Manager.coolDownTime == 70f)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "PhanNhan3":
                if(turnInSkill4 >= 1)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 3";
                    contenSkill.text = "Tăng thời gian trạng thái Phản Nhãn lên 35 giây.";
                    skillBGs[16].enabled = true;
                    scoreUpgradeText.text = "/2";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if(skill4Manager.timeSkill4 == 35f)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "PhanNhan4":
                if(turnInSkill4 >= 3)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 4";
                    contenSkill.text = "Khi trong trạng thái Phản Nhãn bị kẻ địch tấn công sẽ phản lại sát thương nhận vào cho kẻ địch.";
                    skillBGs[17].enabled = true;
                    scoreUpgradeText.text = "/3";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if(skill4Manager.isReflectDamage == true)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "PhanNhan5":
                if(turnInSkill4 >= 4)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 5";
                    contenSkill.text = "Trong trạng thái phản nhãn, tốc độ di chuyển tăng 10%.";
                    skillBGs[18].enabled = true;
                    scoreUpgradeText.text = "/4";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if (skill4Manager.isUpSpeed == true)//tang toc do di chuyen khi mo khoa
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "PhanNhan6":
                if(turnInSkill4 >= 4)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 6";
                    contenSkill.text = "Trong trạng thái Phản Nhãn, miễn nhiễm tất cả hiệu ứng xấu.";
                    skillBGs[19].enabled = true;
                    scoreUpgradeText.text = "/4";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if (skill4Manager.isStun == true)
                {
                    buttonUnlock.SetActive(false);
                }
                break;

            case "PhanNhan7":
                if(turnInSkill4 >= 5)
                {
                    cameras[3].gameObject.SetActive(true);
                    textureTutorial.texture = spritePhanNhan; // Hiển thị camera hướng dẫn
                    previewText.text = "Phản nhãn - Cấp 7";
                    contenSkill.text = "Khi trong đang trong trạng thái Phãn Nhãn giúp tăng tỷ lệ bạo kích lên 100% và bất tử.";
                    skillBGs[20].enabled = true;
                    scoreUpgradeText.text = "/5";//so diem can de nang cap
                }else{
                    previewPanel.SetActive(false);
                }if(skill4Manager.isImmotal == true)
                {
                    buttonUnlock.SetActive(false);
                }
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
            //skill 1 dong cung --------------------------------
            case "DongCung1":
                if(playerStatus.score >=2)
                {
                    playerStatus.score -= 2; // trừ điểm khi mở khóa
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    Debug.Log("Đã mở khóa kỹ năng: ĐôngCung1");
                    ColorUnlockIcon();
                    //mo khoa de su dung skill1
                    skill1Manager.isUnlockSkill1 = true;
                    skill1Manager.iconSkill1.SetActive(true);
                    turnInSkill1 += 1;//nang cap theo trinh tu
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                
                break;
            case "DongCung2":
                if(turnInSkill1 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: ĐôngCung2");           
                    ColorUnlockIcon();
                    turnInSkill1 += 1;   
                    playerStatus.score -= 2;
                    //khi nâng cấp giảm thời gian hồi kỹ năng xuống còn 35f
                   
                    skill1Manager.cooldownSkill = 35f;
                    //cap nhat textscore(moon)
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;
            case "DongCung3":
                if(turnInSkill1 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: ĐôngCung3");
                    ColorUnlockIcon();
                    turnInSkill1 += 1;
                    playerStatus.score -= 2;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    //mo khoa de su dung
                    skill1Manager.timeSkill1 = 7f;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;
            case "DongCung4":
                if(turnInSkill1 >= 3 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: ĐôngCung4");
                    ColorUnlockIcon();
                    playerStatus.score -= 4;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                    playerStatus.scoreText[i].text = playerStatus.score.ToString();
                }
                    //mo khoa de su dung
                    skill1Manager.isDamaged = true;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;

            //skill 2 the ảnh --------------------------------
            case "TheAnh1":
                if (playerStatus.score >= 2)
                {
                    playerStatus.score -= 2; // trừ điểm khi mở khóa
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    Debug.Log("Đã mở khóa kỹ năng: TheAnh1");
                    ColorUnlockIcon();
                    turnInSkill2 += 1;
                    //mo khoa de su dung 
                    skill2Manager.isUnlockSkill2 = true;
                    skill2Manager.iconSkill2.SetActive(true);
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;
            case "TheAnh2":
                if(turnInSkill2 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: TheAnh2");
                    ColorUnlockIcon();
                    playerStatus.score -= 2;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                     //mo khoa de su dung --------
                    turnInSkill2 += 1;
                    //giam thoi gian hoi chieu con 35 giay
                  
                    skill2Manager.skillCooldown = 35;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
              
               
                break;
            case "TheAnh3":
                if (turnInSkill2 >= 2 && playerStatus.score >= 3)
                {
                    Debug.Log("Đã mở khóa kỹ năng: TheAnh3");
                    ColorUnlockIcon();
                    turnInSkill2 += 1;
                    playerStatus.score -= 3;
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill2Manager.timeSkill2 = 15;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }


                break;
            case "TheAnh4":
                if (turnInSkill2 >= 3 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: TheAnh4");
                    ColorUnlockIcon();
                    turnInSkill2 += 1;
                    playerStatus.score -= 4;
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                  skill2Manager.isExplosionSkill2 = true;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;

            //skill 3 quan doan bong ma --------------------------------
            case "QuanDoanBongToi1":
                if (playerStatus.score >= 2)
                {
                    playerStatus.score -= 2; // trừ điểm khi mở khóa
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa1");
                    ColorUnlockIcon();//đổi màu sao khi mở khóa 
                    turnInSkill3 += 1;
                    //mo khoa de su dung 
                    skill3Manager.isUnlockSkill3 = true;
                    skill3Manager.iconSkill3.SetActive(true);
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;
            case "QuanDoanBongToi2":
                if(turnInSkill3 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa2");
                    ColorUnlockIcon();
                    turnInSkill3 += 1;
                    playerStatus.score -= 2;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                   
                    skill3Manager.cooldownSkill = 70;//giam cooldown con 70
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
              
                break;
            case "QuanDoanBongToi3":
                if(turnInSkill3 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa3");
                    ColorUnlockIcon();
                    turnInSkill3 += 1;
                    playerStatus.score -= 3;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill3Manager.timeSkill3 = 40; //tang thoi gian ton tai clone len 40
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
              
                break;
            case "QuanDoanBongToi4":
                if(turnInSkill3 >= 3 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                    ColorUnlockIcon();
                    turnInSkill3 += 1;
                    playerStatus.score -= 4;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill3Manager.isDamaged = true;//tang dame len x2
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "QuanDoanBongToi5":
                if(turnInSkill3 >= 4 && playerStatus.score >= 5)
                {
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                    ColorUnlockIcon();
                    turnInSkill3 += 1;
                    playerStatus.score -= 5;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill3Manager.playerCount = 6;
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "QuanDoanBongToi6":
                if(turnInSkill3 >= 4 && playerStatus.score >= 5)
                {
                    Debug.Log("Đã mở khóa kỹ năng: QuanDoanBongMa4");
                    ColorUnlockIcon();
                    turnInSkill3 += 1;
                    playerStatus.score -= 5;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                    //chx co
                }

                break;

            //skill 4 Phản nhản --------------------------------
            case "PhanNhan1":
                if (playerStatus.score >= 2)
                {
                    playerStatus.score -= 2; // trừ điểm khi mở khóa
                    for (int i = 0; i < playerStatus.scoreText.Length; i++)
                    {
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan1");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    //mo khoa de su dung 
                    skill4Manager.isUnlockSkill4 = true;
                    skill4Manager.iconSkill4.SetActive(true);
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                break;
            case "PhanNhan2":
                if(turnInSkill4 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan2");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 2;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.coolDownTime = 70f;//giam thoi gian cooldown con 70
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "PhanNhan3":
                if(turnInSkill4 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan3");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 2;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.timeSkill4 = 35;//tang thoi gian len 35 giay
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "PhanNhan4":
                if(turnInSkill4 >= 3 && playerStatus.score >= 3)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan4");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 3;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.isReflectDamage = true;//bat trang thai phan dame
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "PhanNhan5":
                if(turnInSkill4 >= 4 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan5");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 4;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.isUpSpeed = true;//tang toc do di chuyen khi mo khoa
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "PhanNhan6":
                if(turnInSkill4 >= 4 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan6");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 4;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.isStun = true;//mo khoa trang thai khang stun 
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }

                break;
            case "PhanNhan7":
                if(turnInSkill4 >= 5 && playerStatus.score >= 5)
                {
                    Debug.Log("Đã mở khóa kỹ năng: PhanNhan7");
                    ColorUnlockIcon();
                    turnInSkill4 += 1;
                    playerStatus.score -= 5;
                    for(int i = 0; i < playerStatus.scoreText.Length; i++){
                        playerStatus.scoreText[i].text = playerStatus.score.ToString();
                    }
                    skill4Manager.isImmotal = true;//mo khoa trang thai bat tu
                    buttonUnlock.SetActive(false);//ẩn nút unlock nếu đã mở khóa
                }
                
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
    //bat tat camera
    private void CameraPreview()
    {
        foreach(Camera cam in cameras)
        {
            if (cam != null)
            {
                cam.gameObject.SetActive(false);
            }
        }
    }



    private void SimulateUnlockAllSkills()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("Đã nhấn Alt - Mở tất cả kỹ năng");

            for (int i = 0; i < buttonIconColor.Length; i++)
            {
                if (buttonIconColor[i] != null)
                {
                    buttonIconColor[i].color = Color.white; // Đổi màu tất cả icon thành trắng
                }
            }
            playerStatus.currentLevel = 30;
            // Đông Cung
            turnInSkill1 = 4;
            skill1Manager.isUnlockSkill1 = true;
            skill1Manager.iconSkill1.SetActive(true);
            skill1Manager.cooldownSkill = 35f;
            skill1Manager.timeSkill1 = 10f;
            skill1Manager.isDamaged = true;

            // Thế Ảnh
            turnInSkill2 = 4;
            skill2Manager.isUnlockSkill2 = true;
            skill2Manager.iconSkill2.SetActive(true);
            skill2Manager.skillCooldown = 35f;
            skill2Manager.timeSkill2 = 15f;
            skill2Manager.isExplosionSkill2 = true;

            // Quân Đoàn Bóng Tối
            turnInSkill3 = 6;
            skill3Manager.isUnlockSkill3 = true;
            skill3Manager.iconSkill3.SetActive(true);
            skill3Manager.cooldownSkill = 70f;
            skill3Manager.timeSkill3 = 40f;
            skill3Manager.isDamaged = true;
            skill3Manager.playerCount = 6;

            // Phản Nhân
            turnInSkill4 = 7;
            skill4Manager.isUnlockSkill4 = true;
            skill4Manager.iconSkill4.SetActive(true);
            skill4Manager.coolDownTime = 70f;
            skill4Manager.timeSkill4 = 35f;
            skill4Manager.isReflectDamage = true;
            skill4Manager.isUpSpeed = true;
            skill4Manager.isStun = true;
            skill4Manager.isImmotal = true;

            // Ẩn nút unlock (nếu cần)
            buttonUnlock.SetActive(false);
        }
    }
}
