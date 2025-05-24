using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SkillManagerUI : MonoBehaviour
{
    public List<SkillData> skills;

    public GameObject panelPreview;
    public TextMeshProUGUI previewName;
    public Button buttonUnlock;
    public Button buttonEquip;
    public Image equippedSlotUI;

    private int currentSkillIndex = -1;
    private int equippedSkillIndex = -1;

    private GameObject activeVideoCamera = null;

    void Start()
    {
        panelPreview.SetActive(false);
        buttonEquip.gameObject.SetActive(false);

        // Khi bắt đầu, tắt tất cả videoCamera của skill để chắc chắn
        foreach (var skill in skills)
        {
            if (skill.videoCamera != null)
            {
                skill.videoCamera.SetActive(false);
            }
        }
    }

    public void PreviewSkill(int index)
    {
        currentSkillIndex = index;
        SkillData skill = skills[index];

        panelPreview.SetActive(true);
        previewName.text = skill.skillName;

        buttonUnlock.interactable = !skill.isUnlocked;
        buttonEquip.gameObject.SetActive(skill.isUnlocked);

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

    public void ClosePreview()
    {
        panelPreview.SetActive(false);

        if (activeVideoCamera != null)
        {
            activeVideoCamera.SetActive(false);
            activeVideoCamera = null;
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
    }

    public Transform skillSpawnPoint; // vi tri

    public void UseEquippedSkill()
    {
        if (equippedSkillIndex < 0) return;

        SkillData skill = skills[equippedSkillIndex];

        switch (skill.skillType)
        {
            case SkillType.RostbindSoul: //đóng băng
                CastRostbindSoul(skill);
                break;
            case SkillType.Fireball: //bắn cầu lửa
                CastFireball(skill);
                break;
            default:
                Debug.Log("No skill effect assigned.");//khong có kỹ năng nào 
                break;
        }
    }

    private void CastRostbindSoul(SkillData skill)
    {
        Debug.Log("dong bang ke dich");

    }
    private void CastFireball(SkillData skill)
    {
        Debug.Log("ban cau lua");

    }
   
}
