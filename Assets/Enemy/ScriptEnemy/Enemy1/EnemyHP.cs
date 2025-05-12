using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;

    //gọi hàm
    Enemy1 enemy1;

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(100); // Gọi hàm giảm máu
        }
    }

    public void TakeDamage(float damage)
    {
        if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        enemy1.agent.isStopped = true; 

        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            enemy1.ChangeState(Enemy1.EnemyState.Death);
            enemy1.agent.isStopped = true; // Dừng lại khi chết

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
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
