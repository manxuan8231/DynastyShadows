using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Skill1Manager : MonoBehaviour
{
   
    public bool isUnlocked = false;

    //cooldown skill
    public Slider cooldownSkilSlider;
    public TextMeshProUGUI textCoolDownSkill;
    public float cooldownSkill = 50f;
    public float lastTimeSkill = -50f;
    public float timeSkill1 = 10; //thoi gian skill 1 ton tai
    public bool isSkillCooldown = false;
    public bool isDamaged = false;
    public bool isInputSkill1 = true;
    //skill 1 dong cung enemy
    public GameObject skillPrefab; // Prefab của kỹ năng
    public Transform spawnPoint; // Vị trí spawn kỹ năng
                                 //tham chieu
    private PlayerControllerState playerController;
    private PlayerStatus playerStatus;

    //unlock skill trong UI thi moi cho dung skill
    public bool isUnlockSkill1 = false;
    public GameObject iconSkill1;
    private void Start()
    {
        textCoolDownSkill.enabled = false;
        isDamaged = false;
        iconSkill1.SetActive(false);
        isSkillCooldown = false;
        playerController = FindAnyObjectByType<PlayerControllerState>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();

        //luu skill
        SkillTreeData skillData = SkillTreeHandler.LoadSkillTree();

        // Skill 1 - Đông Cung      
       isUnlockSkill1 = skillData.isUnlockSkill1;
        cooldownSkill = skillData.cooldownSkill1;
        timeSkill1 = skillData.timeSkill1;
         isDamaged = skillData.isDamagedSkill1;
        if (isUnlockSkill1)
            iconSkill1.SetActive(true);
    }
    private void Update()
    {
        // Cập nhật cooldown
        if (isSkillCooldown)
        {
            lastTimeSkill -= Time.deltaTime;
            cooldownSkilSlider.value = lastTimeSkill;
            textCoolDownSkill.text = Mathf.FloorToInt(lastTimeSkill).ToString();
            if (lastTimeSkill <= 0f)
            {
                isSkillCooldown = false;
                textCoolDownSkill.enabled = false;
                cooldownSkilSlider.gameObject.SetActive(false);
            }
            return;
        }

        // Kiểm tra nếu có enemy trong bán kính 50f
        bool isEnemyInRange = Physics.CheckSphere(transform.position, 50f, LayerMask.GetMask("Enemy"));

        // Chỉ khi có enemy trong vùng và các điều kiện khác mới cho dùng skill
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isSkillCooldown && isInputSkill1 && isUnlockSkill1
            && playerStatus.currentHp > 0 && isEnemyInRange && playerController.isController)
        {
            Debug.Log("Bắn kỹ năng đóng băng");
            textCoolDownSkill.enabled = true;

            // Tạo hiệu ứng kỹ năng
            GameObject skill1 = Instantiate(skillPrefab, spawnPoint.position, spawnPoint.rotation);

            StartCoroutine(WaitRig());

            // Kích hoạt cooldown
            isSkillCooldown = true;
            lastTimeSkill = cooldownSkill;

            if (cooldownSkilSlider != null)
            {
                cooldownSkilSlider.maxValue = cooldownSkill;
                cooldownSkilSlider.value = cooldownSkill;
                cooldownSkilSlider.gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator WaitRig()//cho chay animator
    {
        playerController.rigBuilder.enabled = true; // Bật RigBuilder để có thể chayj animator 
        yield return new WaitForSeconds(0.3f); // Chờ 1 giây
        playerController.rigBuilder.enabled = false; // Tắt RigBuilder sau khi hoàn thành
    }
}
