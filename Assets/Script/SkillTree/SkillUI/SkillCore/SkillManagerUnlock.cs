using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SkillManagerUnlock : MonoBehaviour
{
    public List<SkillDataUnlock> skills;

    public GameObject panelPreview;
    public TextMeshProUGUI previewName;
    public Button buttonUnlock;// nút mở khóa kỹ năng
    public Button buttonEquip;// nút trang bị kỹ năng
    public Button buttonRemove;// nút hủy trang bị kỹ năng

    public Image equippedSlotUI;//slot ngoai man hinh HUD

    private int currentSkillIndex = -1;// chỉ số kỹ năng hiện tại đang xem trước
    private int equippedSkillIndex = -1;// chỉ số kỹ năng đã trang bị

    private GameObject activeVideoCamera = null;

    //tham chieu
    private PlayerControllerState playerController;
    void Start()
    {
        panelPreview.SetActive(false);
        buttonEquip.gameObject.SetActive(false);
        buttonRemove.gameObject.SetActive(false);

        playerController = FindAnyObjectByType<PlayerControllerState>();
        // Khi bắt đầu, tắt tất cả videoCamera của skill để chắc chắn
        foreach (var skill in skills)
        {
            if (skill.videoCamera != null)
            {
                skill.videoCamera.SetActive(false);
            }
        }
    }
    private void Update()
    {

        UseEquippedSkill();//quan ly trang thai skill

    }
    public void PreviewSkill(int index)
    {
        currentSkillIndex = index;
        SkillDataUnlock skill = skills[index];

        panelPreview.SetActive(true);
        previewName.text = skill.skillName;

        buttonUnlock.interactable = !skill.isUnlocked;
        buttonEquip.gameObject.SetActive(skill.isUnlocked);

        buttonRemove.gameObject.SetActive(equippedSkillIndex == currentSkillIndex);

        // Tắt camera đang bật nếu có
        if (activeVideoCamera != null)
        {
            activeVideoCamera.SetActive(false);
            activeVideoCamera = null;
        }

        // Bật camera của skill đang preview
        if (skill.videoCamera != null)
        {
            skill.videoCamera.SetActive(true);
            activeVideoCamera = skill.videoCamera;
        }
    }

    public void UnlockSkill()
    {
        if (currentSkillIndex < 0) return;

        skills[currentSkillIndex].isUnlocked = true;
        buttonUnlock.interactable = false;
        buttonEquip.gameObject.SetActive(true);
    }

    public void EquipSkill()
    {
        if (currentSkillIndex < 0 || !skills[currentSkillIndex].isUnlocked) return;

        equippedSkillIndex = currentSkillIndex;
        equippedSlotUI.sprite = skills[equippedSkillIndex].skillIcon;
        equippedSlotUI.color = Color.white;
        buttonRemove.gameObject.SetActive(true);

    }
    public void UseEquippedSkill()
    {
        if (equippedSkillIndex < 0) return;

        SkillDataUnlock skill = skills[equippedSkillIndex];
        

        switch (skill.skillType)
        {
            case SkillTypeUnlock.DongCung:
                DongCung( skill);
                break;
            case SkillTypeUnlock.TheAnh: 
               TheAnh(skill);
                break;
            case SkillTypeUnlock.BongMa:
                BongMa(skill);
                break;
            case SkillTypeUnlock.PhanNhan:             
                PhanNhan(skill);
                break;
            default:
                Debug.Log("khong có kỹ năng nào ");//khong có kỹ năng nào 
                break;
        }
    }

    private void DongCung(SkillDataUnlock skill)
    {
        
    }
    private void TheAnh(SkillDataUnlock skill)
    {

    }
    private void BongMa(SkillDataUnlock skill)
    {

    }
    private void PhanNhan(SkillDataUnlock skill)
    {

    }

}
