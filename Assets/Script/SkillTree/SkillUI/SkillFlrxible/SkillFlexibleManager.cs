
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillFlexibleManager : MonoBehaviour
{
    
    //hinh icon skill
    public Sprite skill1;
    public Sprite skill2;
    public Sprite skill3;
    public Sprite skill4;
    public Sprite skill5;

    //hien thị thông tin skill
    public GameObject previewPanel;
    public TextMeshProUGUI previewText;
    public TextMeshProUGUI contenSkill;
    public TextMeshProUGUI scoreUpgradeText;
    public GameObject buttonUnlock;
    public GameObject buttonEquip;
    public GameObject buttonRemove;
    // Slot icon và màu sắc nút
    public RawImage slotIcon; // Slot HUD
    public Image[] buttonIconColor;
    public Image[] skillBGs;
    private string currentSkillID = "";
    private string equippedSkillID = "";

    //video preview
    [Header("TextureTutorial and camera")]
    public Camera[] cameras; // Mảng chứa các camera hướng dẫn
    public RawImage textureTutorial; // Hiển thị camera hướng dẫn
    public Texture spriteFireBall;
    public Texture spriteFireRain;
    public Texture spriteSlash;
    public Texture spriteShield;
    public Texture spriteEye;

    // Tham chiếu đến AudioSource và âm thanh
    public AudioSource skillAudioSource;
    public AudioClip buttonClick; 
    public PlayerStatus playerStatus;

    //kiem tra xem kỹ năng đã mở khóa hay chưa
    private bool isDongCung1Unlocked = false;
    private bool isDongCung2Unlocked = false;
    private bool isDongCung3Unlocked = false;
    private bool isDongCung4Unlocked = false;
    private bool isDongCung5Unlocked = false;

    //biến số để theo dõi số lần nâng cấp kỹ năng
    public float turnInSkill1 = 0f;

    public ItemSO itemQuestUnlock;
    public int activeSkillUnlock = 0;

    public ItemSO itemQuestUnlock2; // Biến để kiểm tra xem có hiển thị kỹ năng 5hay không
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        previewPanel.SetActive(false);
        HideAllHighlights();
        skillAudioSource = GetComponent<AudioSource>();

        buttonEquip.SetActive(false);
        buttonRemove.SetActive(false);
        slotIcon.color = Color.clear; // Ẩn icon ban đầu
    }
    private void Update()
    {
        
    }
    private void OnDisable()
    {
        previewPanel.SetActive(false);
        HideAllHighlights();

    }
    public void ShowPreview(string iconID)
    {
        skillAudioSource.PlayOneShot(buttonClick);
        previewPanel.SetActive(true);
        HideAllHighlights();
        currentSkillID = iconID;
        buttonUnlock.SetActive(true);
        CameraPreview();
        switch (iconID)
        {
            case "FireBall":
                textureTutorial.texture = spriteFireBall;
                cameras[0].gameObject.SetActive(true); // Kích hoạt camera cho FireBall
                previewText.text = "Cầu lửa";
                contenSkill.text = "Khi dùng kỹ năng này thì player sẽ bắn ra 3 cầu lửa nối tiếp combo có thời gian hồi chiêu 10 giây.";
                skillBGs[0].enabled = true;
                scoreUpgradeText.text = "/5";
                break;

            case "RainFire":
                textureTutorial.texture = spriteFireRain;
                cameras[1].gameObject.SetActive(true); // Kích hoạt camera cho RainFire
                previewText.text = "Mưa lửa";
                contenSkill.text = "Khi dùng kỹ năng thì người chơi sẽ bay lên thả các cầu lửa xuống gây sát thương vùng có thời gian hồi chiêu 10 giây.";
                skillBGs[1].enabled = true;
                scoreUpgradeText.text = "/7";
                break;

            case "Slash":
                textureTutorial.texture = spriteSlash;
                cameras[2].gameObject.SetActive(true); // Kích hoạt camera cho Slash
                previewText.text = "Trảm kích";
                contenSkill.text = "Khi dùng kỹ năng này thì player sẽ lao tới chém 3 lần nối tiếp có thời gian hồi chiêu 10 giây.";
                skillBGs[2].enabled = true;
                scoreUpgradeText.text = "/7";
                break;

            case "Shield":
               
                if (itemQuestUnlock != null && itemQuestUnlock.showSkill4 && activeSkillUnlock >= 3)
                {
                    textureTutorial.texture = spriteShield;
                    cameras[3].gameObject.SetActive(true); // Kích hoạt camera cho Shield
                    previewText.text = "Khiên Chắn";
                    contenSkill.text = "Khi dùng kỹ năng này thì player sẽ được lớp giáp ảo gồm 500 máu và kháng tất cả hiệu ứng.";
                    skillBGs[3].enabled = true;
                    scoreUpgradeText.text = "/4";
                }
                else
                {
                    previewPanel.SetActive(false); // Ẩn nếu chưa unlock
                    Debug.LogWarning("ItemQuestUnlock đang null hoặc chưa được mở khóa");
                }
                break;
            
            case "Eye":
                if (itemQuestUnlock2 != null && itemQuestUnlock2.showSkill5)
                {
                    textureTutorial.texture = spriteEye;
                    cameras[4].gameObject.SetActive(true); // Kích hoạt camera cho Eye
                    previewText.text = "Thần Nhãn";
                    contenSkill.text = "Khi dùng kỹ năng này sẽ giúp truy tìm dấu vết mà mắt thường khó nhìn thấy được và làm chậm những sinh vật sống.";
                    skillBGs[4].enabled = true;
                    scoreUpgradeText.text = "/7";
                }
                else
                {
                    previewPanel.SetActive(false); // Ẩn nếu chưa unlock
                    Debug.LogWarning("ItemQuestUnlock2 đang null hoặc chưa được mở khóa");
                    return;
                }
               
                break;

            default:
                previewText.text = "Thông tin chưa cập nhật.";
                contenSkill.text = "...";
                break;
        }

        // Cập nhật hiển thị nút Equip/Remove
        if (IsSkillUnlocked(currentSkillID))
        {
            if (equippedSkillID == currentSkillID)
            {
                buttonEquip.SetActive(false);
                buttonRemove.SetActive(true);
            }
            else
            {
                buttonEquip.SetActive(true);
                buttonRemove.SetActive(false);
            }

            buttonUnlock.SetActive(false); // Đã unlock rồi thì ẩn nút unlock
        }
        else
        {
            buttonEquip.SetActive(false);
            buttonRemove.SetActive(false);
            buttonUnlock.SetActive(true); // Chưa unlock thì hiện unlock
        }

    }

    public void ClosePreview()
    {
        previewPanel.SetActive(false);
        HideAllHighlights();
    }

    public void UnlockSkill1()
    {
        switch (currentSkillID)
        {
            case "FireBall":
                if(playerStatus.score > 2)
                {
                    isDongCung1Unlocked = true;
                    playerStatus.score -= 5;
                    UpdateScoreText(); 
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false); // Ẩn nút sau khi mở
                    //save
                    SkillTreeData skillTreeData = SkillTreeHandler.LoadSkillTree();
                    skillTreeData.isDongCung1Unlocked = isDongCung1Unlocked;
                    SkillTreeHandler.SaveSkillTree(skillTreeData);
                }

                break;

            case "RainFire":
                if (playerStatus.score >= 2)
                {
                    isDongCung2Unlocked = true;
                    playerStatus.score -= 7;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                    
                    //save
                    SkillTreeData skillTreeData = SkillTreeHandler.LoadSkillTree();
                    skillTreeData.isDongCung2Unlocked = isDongCung2Unlocked;
                    SkillTreeHandler.SaveSkillTree(skillTreeData);
                }
                break;

            case "Slash":
                if (playerStatus.score >= 2)
                {
                    isDongCung3Unlocked = true;
                    playerStatus.score -= 2;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                    //save
                    SkillTreeData skillTreeData = SkillTreeHandler.LoadSkillTree();
                    skillTreeData.isDongCung3Unlocked = isDongCung3Unlocked;
                    SkillTreeHandler.SaveSkillTree(skillTreeData);
                }
                break;

            case "Shield":

                if (playerStatus.score >= 4 && itemQuestUnlock.hasItemQuest)
                {
                    isDongCung4Unlocked = true;
                    playerStatus.score -= 4;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                    //save
                    SkillTreeData skillTreeData = SkillTreeHandler.LoadSkillTree();
                    skillTreeData.isDongCung4Unlocked = isDongCung4Unlocked;
                    SkillTreeHandler.SaveSkillTree(skillTreeData);
                }
                break;
            case "Eye":
                if (playerStatus.score >= 4)
                {
                    isDongCung5Unlocked = true;
                    playerStatus.score -= 4;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                    //save
                    SkillTreeData skillTreeData = SkillTreeHandler.LoadSkillTree();
                    skillTreeData.isDongCung5Unlocked = isDongCung5Unlocked;
                    SkillTreeHandler.SaveSkillTree(skillTreeData);
                }
               
                break;
        }
    }

    public void EquipSkill()
    {
        switch (currentSkillID)
        {
            case "FireBall":
                slotIcon.texture = skill1.texture;
                break;
            case "RainFire":
                slotIcon.texture = skill2.texture;
                break;
            case "Slash":
                slotIcon.texture = skill3.texture;
                break;
            case "Shield":
                slotIcon.texture = skill4.texture;
                break;
            case "Eye":
                slotIcon.texture = skill5.texture;
                break;
        }

        slotIcon.color = Color.white;
        equippedSkillID = currentSkillID;
        playerStatus.equipSkillID = currentSkillID; // Gán vào PlayerStatus

        buttonEquip.SetActive(false);
        buttonRemove.SetActive(true);
    }



    public void RemoveSkill()
    {
        slotIcon.texture = null;
        slotIcon.color = Color.clear;
        equippedSkillID = "";
        buttonEquip.SetActive(true);
        buttonRemove.SetActive(false);
    }

   

    private void UpdateScoreText()
    {
        for (int i = 0; i < playerStatus.scoreText.Length; i++)
        {
            playerStatus.scoreText[i].text = playerStatus.score.ToString();
        }
    }

    private void HideAllHighlights()
    {
        foreach (Image bg in skillBGs)
        {
            if (bg != null)
                bg.enabled = false;
        }
    }
    private void CameraPreview()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].gameObject.SetActive(false); //tắt cam để đổi camera
            }
        }
    }
    //kiểm tra xem kỹ năng đã mở khóa hay chưa
    private bool IsSkillUnlocked(string id)
    {
        switch (id)
        {
            case "FireBall": return isDongCung1Unlocked;
            case "RainFire": return isDongCung2Unlocked;
            case "Slash": return isDongCung3Unlocked;
            case "Shield": return isDongCung4Unlocked;
            case "Eye": return isDongCung5Unlocked;
        }
        return false;
    }

    
}
