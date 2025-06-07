using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill2Manager : MonoBehaviour
{
    public GameObject clonePrefab;
    public Transform spawnPoint;
    public Slider coolDownSKillSlider;
    public float lastTime = -50f;
    public float skillCooldown = 50f;
    
    public float timeSkill2 = 10f;//thời gian tồn tại của skill
    public bool isSkillActive = false;
    public bool isInputSkill2 = false;
    public bool isChangeSkill2 = false;
    public GameObject playerClone;
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

            if (lastTime <= 0f)
            {
                isSkillActive = false;
                lastTime = 0f;
                coolDownSKillSlider.gameObject.SetActive(false);
                coolDownSKillSlider.value = skillCooldown;
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
       
        
        
    }

    // Kích hoạt kỹ năng phân thân
    void ActivateCloneSkill()
    {
        isChangeSkill2 = true;
        effectRun.SetActive(true);
        isSkillActive = true;
        lastTime = skillCooldown;
        coolDownSKillSlider.gameObject.SetActive(true);
        coolDownSKillSlider.value = skillCooldown;

        // Tạo phân thân
        playerClone = Instantiate(clonePrefab, spawnPoint.position, spawnPoint.rotation);

        // Tắt tag của "SK_DeathKnight"
        GameObject player = GameObject.Find("SK_DeathKnight");
        if (player != null)
        {
            string originalTag = player.tag;
            player.tag = "Untagged";
            StartCoroutine(RestoreTagAfterDelay(player, originalTag, timeSkill2));
        }
        else
        {
            Debug.LogWarning("Không tìm thấy SK_DeathKnight trong scene.");
        }
        StartCoroutine(WaitForRemoveClone()); // đợi 10 giây để loại bỏ phân thân và effect

    }
    //khoi phục hồi tag sau một khoảng thời gian
    private IEnumerator RestoreTagAfterDelay(GameObject obj, string originalTag, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            obj.tag = originalTag;
        }

        
    }

    //dợi 10 giây để loại bỏ phân thân va effect
    public IEnumerator WaitForRemoveClone()
    {
        yield return new WaitForSeconds(10f); // Thời gian chờ trước khi loại bỏ phân thân
        if (playerClone != null)
        {
            Destroy(playerClone);
            effectRun.SetActive(false);
        }
        playerControllerState.isRemoveClone = false; // Đặt lại cờ để không loại bỏ phân thân nữa
    }

   
}
