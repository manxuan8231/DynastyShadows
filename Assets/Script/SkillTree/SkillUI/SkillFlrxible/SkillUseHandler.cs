using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUseHandler : MonoBehaviour
{
    public GameObject player;
    public float lastColldown = -10f;
    public float cooldownTime = 10f;
    public Slider cooldownSlider;
    public TextMeshProUGUI cooldownText;
   // public LayerMask enemyLayer;

    // Fireball combo
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboWindow = 3f; // thời gian cho phép giữa các combo
    private float nextComboAllowedTime = 0f; // chỉ cho phép bấm combo tiếp theo sau thời gian này
    //slash
    public GameObject auraSlash;
    //shield
    public GameObject shieldPrefab; // prefab của shield
    //sounds
    public AudioClip eyeSound;
    // Tham chiếu 
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    public Skill5Eye skill5Eye;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        skill5Eye = FindAnyObjectByType<Skill5Eye>();
        auraSlash.SetActive(false);
       
    }

    void Update()
    {
        string skillID = playerStatus.equipSkillID;

        // Kiểm tra combo timeout
        if (comboStep > 0 && Time.time > comboTimer)
        {
            ResetCombo();
        }

        switch (skillID)
        {
            case "FireBall":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextComboAllowedTime)
                {
                    if (Time.time < lastColldown + cooldownTime) return;

                    StartCoroutine(WaitMove());

                    GameObject enemy = FindEnemy();

                    foreach (Skill3ClonePLayer clone in Object.FindObjectsByType<Skill3ClonePLayer>(FindObjectsSortMode.None))
                    {
                        clone.PlayFireBallAnim();
                    }

                    if (comboStep == 0)
                    {
                        playerControllerState.animator.SetTrigger("FireBall1");
                        comboStep = 1;
                    }
                    else if (comboStep == 1)
                    {
                        playerControllerState.animator.SetTrigger("FireBall2");
                        comboStep = 2;
                    }
                    else if (comboStep == 2)
                    {
                        playerControllerState.animator.SetTrigger("FireBall3");
                        comboStep = 0;
                        lastColldown = Time.time;
                    }

                    comboTimer = Time.time + comboWindow;
                    nextComboAllowedTime = Time.time + 0.7f;
                }
                break;
            case "RainFire":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextComboAllowedTime)
                {
                    if (Time.time < lastColldown + cooldownTime) return;
                    StartCoroutine(WaitMove());
                    StartCoroutine(WaitForGraity());
                    foreach (Skill3ClonePLayer clone in Object.FindObjectsByType<Skill3ClonePLayer>(FindObjectsSortMode.None))
                    {
                        clone.PlayRainFireAnim();
                    }
                    playerControllerState.animator.SetTrigger("RainFire");
                    lastColldown = Time.time;
                    nextComboAllowedTime = Time.time + 0.7f;
                }
                break;
            case "Slash":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextComboAllowedTime)
                {
                    if (Time.time < lastColldown + cooldownTime) return;
                    foreach (Skill3ClonePLayer clone in Object.FindObjectsByType<Skill3ClonePLayer>(FindObjectsSortMode.None))
                    {
                        clone.PlaySlashAnim();
                    }
                    StartCoroutine(WaitMove());
                    FindEnemy();

                    Vector3 dashDir = player.transform.forward;
                    float dashDistance = 30f; // độ dài lướt tối đa

                    // Raycast kiểm tra vật cản phía trước
                    RaycastHit hit;
                    Vector3 finalTargetPos = player.transform.position + dashDir * dashDistance;

                    if (Physics.Raycast(player.transform.position, dashDir, out hit, dashDistance, LayerMask.GetMask("Ground")))
                    {
                        // Nếu trúng tường, chỉ dash tới trước tường một chút
                        finalTargetPos = hit.point - dashDir * 0.5f;
                    }

                   
                    playerControllerState.StartCoroutine(DashToTarget(finalTargetPos, 0.25f));

                   
                    if (comboStep == 0)
                    {
                        StartCoroutine(WaitForAuraFire());
                        playerControllerState.animator.SetTrigger("Slash");
                        comboStep = 1;
                    }
                    else if (comboStep == 1)
                    {
                        StartCoroutine(WaitForAuraFire());
                        playerControllerState.animator.SetTrigger("Slash2");
                        comboStep = 2;
                    }
                    else if (comboStep == 2)
                    {
                        StartCoroutine(WaitForAuraFire());
                        playerControllerState.animator.SetTrigger("Slash3");
                        comboStep = 0;
                        lastColldown = Time.time;
                    }

                    comboTimer = Time.time + comboWindow;
                    nextComboAllowedTime = Time.time + 0.5f;
                }
                break;
            case "Shield":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextComboAllowedTime)
                {
                    Debug.Log("Sử dụng Shield");
                    if (Time.time < lastColldown + cooldownTime) return; 
                    
                    GameObject shield = Instantiate(shieldPrefab, player.transform.position, player.transform.rotation);
                  
                    lastColldown = Time.time;
                    nextComboAllowedTime = Time.time + 0.7f;
                }
                break;
            case "Eye":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= nextComboAllowedTime)
                {
                    Debug.Log("Sử dụng mat than");
                    if (Time.time < lastColldown + cooldownTime) return;
                    playerControllerState.audioSource.PlayOneShot(eyeSound);
                    skill5Eye.isInputSkill = true;//bat skill
                    lastColldown = Time.time;
                    nextComboAllowedTime = Time.time + 0.7f;
                }
                break;
            default:
               
                break;
        }

        bool isOnCooldown = Time.time < lastColldown + cooldownTime;
        bool isInCombo = comboStep > 0 && Time.time < comboTimer;

        if (isOnCooldown || isInCombo)
        {
            if (cooldownSlider != null)
                cooldownSlider.gameObject.SetActive(isOnCooldown);

            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);

                if (isInCombo)
                {
                    float comboRemaining = comboTimer - Time.time;
                    cooldownText.text = "" + comboRemaining.ToString("F1") + "s";
                }
                else if (isOnCooldown)
                {
                    float cooldownRemaining = (lastColldown + cooldownTime) - Time.time;
                    cooldownText.text = Mathf.CeilToInt(cooldownRemaining).ToString() + "s";
                }
            }

            if (cooldownSlider != null && isOnCooldown)
            {
                float fillAmount = ((lastColldown + cooldownTime) - Time.time) / cooldownTime;
                cooldownSlider.value = fillAmount;
            }
        }
        else
        {
            if (cooldownSlider != null) cooldownSlider.gameObject.SetActive(false);
            if (cooldownText != null) cooldownText.gameObject.SetActive(false);
        }

       

    }
    void ResetCombo()
    {
        Debug.Log("Combo hết thời gian, reset và cooldown");
        comboStep = 0;
        lastColldown = Time.time;
    }

    IEnumerator DashToTarget(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = player.transform.position;
        float time = 0f;

        playerControllerState.controller.enabled = false;
        playerControllerState.controller.detectCollisions = false;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            playerControllerState.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        playerControllerState.transform.position = targetPosition;

        playerControllerState.controller.detectCollisions = true;
        playerControllerState.controller.enabled = true;
    }


   

    public GameObject FindEnemy()
    {
        float radius = 100f;
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider col in colliders)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = col.gameObject;
            }
        }

        if (closestEnemy != null && player != null)
        {
            Vector3 direction = (closestEnemy.transform.position - player.transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction);
                player.transform.rotation = targetRot;
            }
        }

        return closestEnemy;
    }
    

    public IEnumerator WaitMove()
    {
        playerControllerState.enabled = false;
        yield return new WaitForSeconds(0.8f);
        playerControllerState.enabled = true;
    }
    public IEnumerator WaitForGraity()
    {
        playerControllerState.gravity = 0f;
        playerControllerState.controller.enabled = false;
        yield return new WaitForSeconds(3f);
        playerControllerState.controller.enabled = true;
        playerControllerState.gravity = -9.81f;
    }

    //skill slash
    public IEnumerator WaitForAuraFire()
    {
        auraSlash.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        auraSlash.SetActive(false);
    }
    
   
}
