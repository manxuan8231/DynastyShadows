using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill3Manager : MonoBehaviour
{
    public Transform[] spawnClonePL;
    public string clonePLTag = "PlayerClone"; // Tag của clone player
    public float lastTime = -100;
    public float cooldownSkill = 100;
    public float timeSkill3 = 30f;//thoi gian skill 3 ton tai
    public float playerCount = 4;//player spawn 
    public bool isInputSkill3;
    public bool isLv6 = false; // kiem tra level 6 chưa
    public Slider cooldownSlider;
    public TextMeshProUGUI textCoolDownSkill;
    //unlock skill trong UI thi moi cho dung skill
    public bool isDamaged;//neu true thi dame tang x2
    public bool isUnlockSkill3 = false;
    public GameObject iconSkill3;

    //tham chieu
    public PlayerControllerState playerControllerState;

    private void Start()
    {
        textCoolDownSkill.enabled = false;
        isDamaged =false;
        iconSkill3.SetActive(false);
        isUnlockSkill3 = false;
        isInputSkill3 = true;
        cooldownSlider.maxValue = cooldownSkill;
        cooldownSlider.value = cooldownSkill;
        cooldownSlider.enabled = false;
          isLv6 = false; // kiem tra level 6 chưa
    playerControllerState = FindAnyObjectByType<PlayerControllerState>();
    }
    void Update()
    {
        float time =  Time.time - lastTime; 
        float remainingCooldown = Mathf.Clamp(cooldownSkill - time, 0, cooldownSkill);
        cooldownSlider.value = remainingCooldown;
        textCoolDownSkill.text = Mathf.FloorToInt(remainingCooldown).ToString();//cap nhap text
        if (remainingCooldown <= 0) {
            textCoolDownSkill.enabled = false;//neu be hon 0 thi tat text
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Time.time >= lastTime + cooldownSkill && isInputSkill3 == true && isUnlockSkill3 == true
            && playerControllerState.isController)
        {
            textCoolDownSkill.enabled = true;
           
            StartCoroutine(WaitMove());
            playerControllerState.animator.SetTrigger("Skill3");
            cooldownSlider.enabled = true;
            StartCoroutine(SpawnClone());
            lastTime = Time.time;
        }
    }

    public IEnumerator SpawnClone()
    {
        float spawnCount = Mathf.Min(playerCount, spawnClonePL.Length);
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = spawnClonePL[i].position;
            GameObject clone = ObjPoolingManager.Instance.GetEnemyFromPool(clonePLTag, spawnPosition);
            clone.transform.rotation = spawnClonePL[i].rotation;
          
            yield return new WaitForSeconds(0.3f); 
        }
    }
    public IEnumerator WaitMove()
    {
        playerControllerState.controller.enabled = false;
        yield return new WaitForSeconds(1);
        playerControllerState.controller.enabled = true;
    }
}
