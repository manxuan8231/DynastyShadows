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

    // Fireball combo
    private int comboStep = 0;
    private float comboTimer = 0f;
    public float comboWindow = 2f; // thời gian cho phép giữa các combo
    private float nextComboAllowedTime = 0f; // chỉ cho phép bấm combo tiếp theo sau thời gian này

    // Tham chiếu 
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
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

                    foreach (Skill3ClonePLayer clone in FindObjectsOfType<Skill3ClonePLayer>())
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
                    foreach (Skill3ClonePLayer clone in FindObjectsOfType<Skill3ClonePLayer>())
                    {
                        clone.PlayRainFireAnim();
                    }
                    playerControllerState.animator.SetTrigger("RainFire");
                    lastColldown = Time.time;
                    nextComboAllowedTime = Time.time + 0.7f;
                }
                break;
            case "DongCung3":
            case "DongCung4":
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
}
