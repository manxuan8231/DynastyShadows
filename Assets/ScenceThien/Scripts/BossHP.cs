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
        if (isDead || bossScript.currentState == BossScript.EnemyState.Death) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (sliderHp != null)
            sliderHp.value = currentHealth;

        if (currentHealth > 0)
        {
            bossScript.ChangeState(BossScript.EnemyState.GetHit);
            Invoke(nameof(BackToChase), 0.6f);
        }
        else
        {
            Die();
        }
    }

    void BackToChase()
    {
        if (!isDead && bossScript.currentState != BossScript.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, bossScript.player.position);
            if (dist <= bossScript.attackRange)
                bossScript.ChangeState(BossScript.EnemyState.Attack);
            else
                bossScript.ChangeState(BossScript.EnemyState.Run);
        }
    }

    void Die()
    {
        isDead = true;
        bossScript.ChangeState(BossScript.EnemyState.Death);
        bossScript.agent.isStopped = true;

 
        if (sliderHp != null)
            sliderHp.gameObject.SetActive(false);


        Destroy(gameObject, 5f);
    }
}