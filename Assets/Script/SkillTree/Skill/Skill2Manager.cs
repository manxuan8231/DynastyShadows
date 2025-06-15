using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill2Manager : MonoBehaviour
{
    public GameObject clonePrefab;
    public Transform spawnPoint;
    public Slider coolDownSKillSlider;
    public TextMeshProUGUI textCoolDownSkill;
    public float lastTime = -50f;
    public float skillCooldown = 50f;
    
    public float timeSkill2 = 10f;//thời gian tồn tại của skill
    public bool isSkillActive = false;
    public bool isInputSkill2 = false;
    public bool isChangeSkill2 = false;
    public bool isExplosionSkill2 = false;
    public GameObject playerClone;
    public GameObject prohibitedIcon; // cản ko cho dùng skill khi sử dụng skill 4 canvas 
    public bool isHibitedIcon;//kieemr tra 
    //effect
    public GameObject effectRun;

    public PlayerControllerState playerControllerState;

    //unlock skill trong UI thi moi cho dung skill
    public bool isUnlockSkill2 = false;
    public GameObject iconSkill2;
    void Start()
    {
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        coolDownSKillSlider.gameObject.SetActive(false);
        isUnlockSkill2 = false;
        iconSkill2.SetActive(false);
        coolDownSKillSlider.maxValue = skillCooldown;
        coolDownSKillSlider.value = skillCooldown;
        effectRun.SetActive(false);
        isExplosionSkill2 = false;
        isHibitedIcon = false;
        textCoolDownSkill.enabled= false;
        prohibitedIcon.SetActive(false); // Ẩn biểu tượng cấm sử dụng skill 2 ban đầu
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isSkillActive && isUnlockSkill2 == true && isInputSkill2 == true)
        {
           
            ActivateCloneSkill();
        }

        // Nếu đang hồi chiêu, cập nhật timer và Slider
        if (isSkillActive)
        {
            lastTime -= Time.deltaTime;
            coolDownSKillSlider.value = lastTime;
            textCoolDownSkill.text = Mathf.FloorToInt(lastTime).ToString();
            if (lastTime <= 0f)
            {
                isSkillActive = false;
                lastTime = 0f;
                coolDownSKillSlider.gameObject.SetActive(false);
                coolDownSKillSlider.value = skillCooldown;
                textCoolDownSkill.enabled = false;
            }
        }
        // Xử lý việc loại bỏ phân thân nếu cần
        if (playerControllerState.isRemoveClone == true)
        {
            if (playerClone != null)
            {
                Destroy(playerClone);
            }
            playerControllerState.isRemoveClone = false;
        }
        if(isHibitedIcon == true)
        {
            prohibitedIcon.SetActive(true); // Hiển thị biểu tượng cấm sử dụng skill 2
        }
        else
        {
            prohibitedIcon.SetActive(false); // Ẩn biểu tượng cấm sử dụng skill 2
        }


    }

    // Kích hoạt kỹ năng phân thân
    void ActivateCloneSkill()
    {
        textCoolDownSkill.enabled = true;
        isChangeSkill2 = true;
        effectRun.SetActive(true);
        isSkillActive = true;
        lastTime = skillCooldown;
        coolDownSKillSlider.gameObject.SetActive(true);
        coolDownSKillSlider.value = skillCooldown;

        // Tạo phân thân
        playerClone = Instantiate(clonePrefab, spawnPoint.position, spawnPoint.rotation);

       
        StartCoroutine(WaitForRemoveClone()); // đợi 10 giây để loại bỏ phân thân và effect

    }
   
    //dợi 10 giây để loại bỏ phân thân va effect
    public IEnumerator WaitForRemoveClone()
    {
       
        yield return new WaitForSeconds(timeSkill2); // Thời gian chờ trước khi loại bỏ phân thân
        if (playerClone != null)
        {
            Destroy(playerClone);
            effectRun.SetActive(false);
        }
        playerControllerState.isRemoveClone = false; // Đặt lại cờ để không loại bỏ phân thân nữa
        prohibitedIcon.SetActive(false); // Ẩn biểu tượng cấm sử dụng skill 2 ban đầu
    }

   
}
