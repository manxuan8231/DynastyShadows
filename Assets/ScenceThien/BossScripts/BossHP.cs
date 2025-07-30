using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHP : MonoBehaviour, IDamageable
{
    public Slider sliderHp;
    public Slider easeSliderHp;
    public float currentHealth;
    public float maxHealth = 10000f;
    public float lerpSpeed = 0.03f; // Tốc độ 
    public float showHpDistance = 50f; // khoảng cách tối đa để hiển thị thanh máu
    private BossScript bossScript;
    private bool isDead = false;
    private bool hasEnteredPhase2 = false;
    //tham chieu
    Quest3 quest3;
    QuestDesert5 questDesert5;
    void Start()
    {
        currentHealth = maxHealth;

        if (sliderHp != null)
        {
            sliderHp.maxValue = maxHealth;
            sliderHp.value = currentHealth;
            easeSliderHp.maxValue = maxHealth;
            easeSliderHp.value = currentHealth;
        }

        bossScript = GetComponent<BossScript>();
        quest3 = FindAnyObjectByType<Quest3>();
        questDesert5 = FindAnyObjectByType<QuestDesert5>();
    }

    void Update()
    {
        if (sliderHp != null && easeSliderHp != null)
        {
            // Làm mượt thanh máu
            if (sliderHp.value != easeSliderHp.value)
            {
                easeSliderHp.value = Mathf.Lerp(easeSliderHp.value, currentHealth, lerpSpeed);
            }

            // Ẩn/hiện thanh máu theo khoảng cách
            if (bossScript != null && bossScript.player != null)
            {
                float distance = Vector3.Distance(transform.position, bossScript.player.position);
                bool show = distance <= showHpDistance;

                sliderHp.gameObject.SetActive(show);
                easeSliderHp.gameObject.SetActive(show);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead || bossScript == null || bossScript.isDead)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (sliderHp != null)
            sliderHp.value = currentHealth;

        if (!hasEnteredPhase2 && currentHealth <= maxHealth / 2f)
        {
            hasEnteredPhase2 = true;
            bossScript.TransitionToState(bossScript.phase2State); // đã đúng
            return;
        }

        if (currentHealth >= 0)
        {
            StartCoroutine(ReturnToBattle(0.6f));
        }
        if(currentHealth == 0)
        {
          
            Die();
        }
    }

    IEnumerator ReturnToBattle(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isDead && !bossScript.isDead)
        {
            float dist = Vector3.Distance(transform.position, bossScript.player.position);
            if (dist <= bossScript.attackRange)
                bossScript.TransitionToState(bossScript.attackState);
            else
                bossScript.TransitionToState(bossScript.chaseState);
        }
    }

    void Die()
    {
        Debug.Log("Boss đã chết");
        isDead = true;
        bossScript.TransitionToState(bossScript.deathState);
        if (quest3 != null)
            quest3.UpdateKillOrk(1);//quest cap nhap khi kill osrk
        if (questDesert5 != null)
            questDesert5.UpdateKillBoss(1); // Bắt đầu nhiệm vụ Desert5
        if (bossScript.agent != null)
            bossScript.agent.isStopped = true;

        if (sliderHp != null)
            sliderHp.gameObject.SetActive(false);

        ObjPoolingManager.Instance.ReturnToPool("BossOrk1", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
       // Destroy(gameObject, 5f);
       
    }

}
