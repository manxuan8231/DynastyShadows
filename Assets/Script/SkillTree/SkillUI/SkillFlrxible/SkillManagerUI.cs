using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEditor.Experimental.GraphView;

public class SkillManagerUI : MonoBehaviour
{
    public List<SkillData> skills;

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
        SkillData skill = skills[index];

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
        buttonRemove.gameObject.SetActive(true);

    }

    public void RemoveSkill()//gỡ trang bị kỹ năng
    {
        if (equippedSkillIndex < 0) return;

        // Xóa icon kỹ năng khỏi slot
        equippedSlotUI.sprite = null;
        equippedSlotUI.color = Color.clear;

        // Reset trạng thái trang bị
        equippedSkillIndex = -1;

        // Ẩn nút gỡ kỹ năng
        buttonRemove.gameObject.SetActive(false);
    }

    public void UseEquippedSkill()
    {
        if (equippedSkillIndex < 0) return;

        SkillData skill = skills[equippedSkillIndex];
        skill.cooldownSkilSlider.value = 0;
        // Cập nhật cooldown
        if (skill.isSkillCooldown)
        {
            skill.lastTimeSkill -= Time.deltaTime;
            skill.cooldownSkilSlider.value = skill.lastTimeSkill;

            if (skill.lastTimeSkill <= 0f)
            {
                skill.isSkillCooldown = false;
                skill.cooldownSkilSlider.gameObject.SetActive(false);
            }
            return; // Đang cooldown thì không dùng được kỹ năng
        }

        switch (skill.skillType)
        {
            case SkillType.RostbindSoul: //đóng băng
                CastRostbindSoul(skill);
                break;
            case SkillType.Fireball: //bắn cầu lửa
                CastFireball(skill);
                break;
            default:
                Debug.Log("khong có kỹ năng nào ");//khong có kỹ năng nào 
                break;
        }
    }


    private void CastRostbindSoul(SkillData skill)
    {
        if (Input.GetKeyDown(KeyCode.R) && !skill.isSkillCooldown)
        {
            Debug.Log("Bắn kỹ năng đóng băng");

            // Tạo hiệu ứng kỹ năng
            Instantiate(skill.skillPrefab, skill.spawnPoint.position, skill.spawnPoint.rotation);
           
            // Kích hoạt cooldown
            skill.isSkillCooldown = true;
            skill.lastTimeSkill = skill.cooldownSkill;

            // Kích hoạt và thiết lập Slider
            if (skill.cooldownSkilSlider != null)
            {
                skill.cooldownSkilSlider.maxValue = skill.cooldownSkill;
                skill.cooldownSkilSlider.value = skill.cooldownSkill;
                skill.cooldownSkilSlider.gameObject.SetActive(true);
            }
        }
    }

   


    private void CastFireball(SkillData skill)
    {
        if (Input.GetKeyDown(KeyCode.R) && !skill.isSkillCooldown)
        {
            Debug.Log("Bắn cầu lửa");

          
            // Kích hoạt cooldown
            skill.isSkillCooldown = true;
            skill.lastTimeSkill = skill.cooldownSkill;

            if (skill.cooldownSkilSlider != null)
            {
                skill.cooldownSkilSlider.maxValue = skill.cooldownSkill;
                skill.cooldownSkilSlider.value = skill.cooldownSkill;
                skill.cooldownSkilSlider.gameObject.SetActive(true);
            }
        }
    }

  
}
