using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;
    //drop exp
    public GameObject expPrefab;
    
    //gọi hàm
    Enemy1 enemy1;
    //box nhận dame
    public BoxCollider boxDame;
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy1 = GetComponent<Enemy1>(); // <- GÁN Ở ĐÂY

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void TakeDamage(float damage)
    {
        if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
       
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.Death);
            enemy1.agent.isStopped = true; // Dừng lại khi chết
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 3f);
            // Tạo expPrefab tại vị trí của enemy
        }
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }
        
   
    }
    public void TakeDamageHit(float damage)
    {
        if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.Death);
            enemy1.agent.isStopped = true; // Dừng lại khi chết
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 3f);
            // Tạo expPrefab tại vị trí của enemy
          
        }
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);


            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }

       
    }

   

    void BackToChase()
    {
        if (enemy1.currentState != Enemy1.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, enemy1.player.position);
            if (dist <= enemy1.attackRange)
            {
                enemy1.ChangeState(Enemy1.EnemyState.Attack);
            }
            else
            {
                enemy1.ChangeState(Enemy1.EnemyState.Run);
            }
        }
    }
}
