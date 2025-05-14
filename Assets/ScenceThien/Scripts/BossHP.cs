using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 10000f;

    private BossScript bossScript;
    private bool isDead = false;
    private bool hasEnteredPhase2 = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (sliderHp != null)
        {
            sliderHp.maxValue = maxHealth;
            sliderHp.value = currentHealth;
        }

        bossScript = GetComponent<BossScript>();
    }

    void Update()
    {
   
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(500);
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

        // Chuyển phase nếu máu thấp
        if (!hasEnteredPhase2 && currentHealth <= maxHealth / 2f)
        {
            hasEnteredPhase2 = true;
            bossScript.TransitionToState(bossScript.phaseChangeState);
            return;
        }

        if (currentHealth > 0)
        {
            bossScript.TransitionToState(bossScript.getHitState);
            StartCoroutine(ReturnToBattle(0.6f));
        }
        else
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
        isDead = true;
        bossScript.TransitionToState(bossScript.deathState);

        if (bossScript.agent != null)
            bossScript.agent.isStopped = true;

        if (sliderHp != null)
            sliderHp.gameObject.SetActive(false);

        Destroy(gameObject, 5f);
    }
}
