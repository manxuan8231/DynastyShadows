using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillFlexibleManager : MonoBehaviour
{
    public Sprite skill1;
    public Sprite skill2;
    public Sprite skill3;
    public Sprite skill4;

    public GameObject previewPanel;
    public TextMeshProUGUI previewText;
    public TextMeshProUGUI contenSkill;
    public TextMeshProUGUI scoreUpgradeText;
    public GameObject buttonUnlock;
    public GameObject buttonEquip;
    public GameObject buttonRemove;

    public RawImage slotIcon; // Slot HUD
    public Image[] buttonIconColor;
    public Image[] skillBGs;
    private string currentSkillID = "";
    private string equippedSkillID = "";

    public AudioSource skillAudioSource;
    public AudioClip buttonClick;

    [Header("Tham chiếu")]
    public PlayerStatus playerStatus;

    public float turnInSkill1 = 0f;

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

    public void ShowPreview(string iconID)
    {
        skillAudioSource.PlayOneShot(buttonClick);
        previewPanel.SetActive(true);
        HideAllHighlights();
        currentSkillID = iconID;
        buttonUnlock.SetActive(true);

        switch (iconID)
        {
            case "DongCung1":
                previewText.text = "Đông cứng - Cấp 1";
                contenSkill.text = "Đông cứng kẻ địch 10 giây, hồi chiêu 50 giây.";
                skillBGs[0].enabled = true;
                scoreUpgradeText.text = "/2";
                break;

            case "DongCung2":
                previewText.text = "Đông cứng - Cấp 2";
                contenSkill.text = "Giảm hồi chiêu còn 35 giây.";
                skillBGs[1].enabled = true;
                scoreUpgradeText.text = "/2";
                break;

            case "DongCung3":
                previewText.text = "Đông cứng - Cấp 3";
                contenSkill.text = "Tăng thời gian đông cứng lên 20 giây.";
                skillBGs[2].enabled = true;
                scoreUpgradeText.text = "/2";
                break;

            case "DongCung4":
                previewText.text = "Đông cứng - Cấp 4";
                contenSkill.text = "Gây sát thương dựa vào sát thương bạo kích.";
                skillBGs[3].enabled = true;
                scoreUpgradeText.text = "/4";
                break;

            default:
                previewText.text = "Thông tin chưa cập nhật.";
                contenSkill.text = "...";
                break;
        }

        // Cập nhật hiển thị nút Equip/Remove
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
    }

    public void ClosePreview()
    {
        previewPanel.SetActive(false);
    }

    public void UnlockSkill1()
    {
        switch (currentSkillID)
        {
            case "DongCung1":
                if(playerStatus.score > 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: DongCung1");
                    ColorUnlockIcon();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                }
              
                break;

            case "DongCung2":
                if (turnInSkill1 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: DongCung2");
                    ColorUnlockIcon();
                    turnInSkill1 += 1;
                    playerStatus.score -= 2;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                }
                break;

            case "DongCung3":
                if (turnInSkill1 >= 1 && playerStatus.score >= 2)
                {
                    Debug.Log("Đã mở khóa kỹ năng: DongCung3");
                    ColorUnlockIcon();
                    turnInSkill1 += 1;
                    playerStatus.score -= 2;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                }
                break;

            case "DongCung4":
                if (turnInSkill1 >= 3 && playerStatus.score >= 4)
                {
                    Debug.Log("Đã mở khóa kỹ năng: DongCung4");
                    ColorUnlockIcon();
                    playerStatus.score -= 4;
                    UpdateScoreText();
                    buttonEquip.SetActive(true);
                    buttonRemove.SetActive(false);
                    buttonUnlock.SetActive(false);
                }
                break;
        }
    }

    public void EquipSkill()
    {
        switch (currentSkillID)
        {
            case "DongCung1":
                slotIcon.texture = skill1.texture;
                break;
            case "DongCung2":
                slotIcon.texture = skill2.texture;
                break;
            case "DongCung3":
                slotIcon.texture = skill3.texture;
                break;
            case "DongCung4":
                slotIcon.texture = skill4.texture;
                break;
        }

        slotIcon.color = Color.white;
        equippedSkillID = currentSkillID;

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

    private void ColorUnlockIcon()
    {
        int index = -1;
        switch (currentSkillID)
        {
            case "DongCung1": index = 0; break;
            case "DongCung2": index = 1; break;
            case "DongCung3": index = 2; break;
            case "DongCung4": index = 3; break;
        }

        if (index >= 0 && index < buttonIconColor.Length && buttonIconColor[index] != null)
        {
            buttonIconColor[index].color = Color.white;
        }
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
}
