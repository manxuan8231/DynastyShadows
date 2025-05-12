using UnityEngine;
using UnityEngine.UI;

public class EnemyHP4 : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;

    //gọi hàm
    Enemy4 enemy4;

    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy4 = GetComponent<Enemy4>(); // <- GÁN Ở ĐÂY

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
        if (enemy4.currentState == Enemy4.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;



        if (currentHealth > 0)
        {
            enemy4.ChangeState(Enemy4.EnemyState.GetHit);

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            enemy4.ChangeState(Enemy4.EnemyState.Death);
            enemy4.agent.isStopped = true; // Dừng lại khi chết

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
    void BackToChase()
    {
        if (enemy4.currentState != Enemy4.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, enemy4.player.position);
            if (dist <= enemy4.attackRange)
            {
                enemy4.ChangeState(Enemy4.EnemyState.Attack);
            }
            else
            {
                enemy4.ChangeState(Enemy4.EnemyState.Run);
            }
        }
    }
}
