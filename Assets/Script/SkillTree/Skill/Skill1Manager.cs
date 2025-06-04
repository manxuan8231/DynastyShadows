using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class Skill1Manager : MonoBehaviour
{
   
    public bool isUnlocked = false;

    //cooldown skill
    public Slider cooldownSkilSlider;
    public float cooldownSkill = 10f;
    public float lastTimeSkill = 0f;
    public bool isSkillCooldown = false;

    public bool isInputSkill1 = true;
    //skill 1 dong cung enemy
    public GameObject skillPrefab; // Prefab của kỹ năng
    public Transform spawnPoint; // Vị trí spawn kỹ năng
                                 //tham chieu
    private PlayerControllerState playerController;

    //unlock skill trong UI thi moi cho dung skill
    public bool isUnlockSkill1 = false;
    public GameObject iconSkill1;
    private void Start()
    {
        iconSkill1.SetActive(false);
        isSkillCooldown = false;
        playerController = FindAnyObjectByType<PlayerControllerState>();

    }
    private void Update()
    {
        // Cập nhật cooldown
        if (isSkillCooldown)
        {
            lastTimeSkill -= Time.deltaTime;
            cooldownSkilSlider.value = lastTimeSkill;

            if (lastTimeSkill <= 0f)
            {
                isSkillCooldown = false;
                cooldownSkilSlider.gameObject.SetActive(false);
            }
            return; // Đang cooldown thì không dùng được kỹ năng
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isSkillCooldown && isInputSkill1 == true && isUnlockSkill1 == true)
        {
            Debug.Log("Bắn kỹ năng đóng băng");

            // Tạo hiệu ứng kỹ năng
            Instantiate(skillPrefab, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine(WaitRig());

            // Kích hoạt cooldown
            isSkillCooldown = true;
            lastTimeSkill = cooldownSkill;

            // Kích hoạt và thiết lập Slider
            if (cooldownSkilSlider != null)
            {
                cooldownSkilSlider.maxValue = cooldownSkill;
                cooldownSkilSlider.value = cooldownSkill;
                cooldownSkilSlider.gameObject.SetActive(true);
            }
        }
    }
    private IEnumerator WaitRig()
    {
        playerController.rigBuilder.enabled = true; // Bật RigBuilder để có thể chayj animator 
        yield return new WaitForSeconds(0.3f); // Chờ 1 giây
        playerController.rigBuilder.enabled = false; // Tắt RigBuilder sau khi hoàn thành
    }
}
