using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Skill2Manager : MonoBehaviour
{
    public GameObject clonePrefab;
    public Transform spawnPoint;
    public Slider coolDownSKillSlider;
    public float skillCooldown = 5f;

    private bool isSkillActive = false;
    private float cooldownTimer = 0f;
    private GameObject playerClone;

    void Start()
    {
        coolDownSKillSlider.gameObject.SetActive(false);
        coolDownSKillSlider.maxValue = skillCooldown;
        coolDownSKillSlider.value = skillCooldown;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isSkillActive)
        {
            ActivateCloneSkill();
        }

        // Nếu đang hồi chiêu, cập nhật timer và Slider
        if (isSkillActive)
        {
            cooldownTimer -= Time.deltaTime;
            coolDownSKillSlider.value = cooldownTimer;

            if (cooldownTimer <= 0f)
            {
                isSkillActive = false;
                cooldownTimer = 0f;
                coolDownSKillSlider.gameObject.SetActive(false);
                coolDownSKillSlider.value = skillCooldown;
            }
        }
    }

    void ActivateCloneSkill()
    {
        isSkillActive = true;
        cooldownTimer = skillCooldown;
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
            StartCoroutine(RestoreTagAfterDelay(player, originalTag, skillCooldown));
        }
        else
        {
            Debug.LogWarning("Không tìm thấy SK_DeathKnight trong scene.");
        }
    }

    private IEnumerator RestoreTagAfterDelay(GameObject obj, string originalTag, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
        {
            obj.tag = originalTag;
        }

        if (playerClone != null)
        {
            Destroy(playerClone);
        }
    }
}
