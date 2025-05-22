using UnityEngine;
using UnityEngine.UI;

public class EnemyHP2 : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;
    public GameObject textDame;
    //gọi hàm
    Enemy2 enemy2;
    KnightD knightD;
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy2 = GetComponent<Enemy2>(); // <- GÁN Ở ĐÂY
        knightD = FindAnyObjectByType<KnightD>();
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
        if (enemy2.currentState == Enemy2.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
       
        sliderHp.value = currentHealth;
       


        if (currentHealth > 0)
        {
           

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            enemy2.ChangeState(Enemy2.EnemyState.Death);
            enemy2.agent.isStopped = true; // Dừng lại khi chết
            knightD.UpDateQuest2(1); // Cập nhật quest khi enemy chết
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
    public void TakeDamageHit(float damage)
    {
        if (enemy2.currentState == Enemy2.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
       
        sliderHp.value = currentHealth;



        if (currentHealth > 0)
        {
            enemy2.ChangeState(Enemy2.EnemyState.GetHit);

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            enemy2.ChangeState(Enemy2.EnemyState.Death);
            enemy2.agent.isStopped = true; // Dừng lại khi chết
            knightD.UpDateQuest2(1); // Cập nhật quest khi enemy chết
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }

   

    void BackToChase()
    {
        if (enemy2.currentState != Enemy2.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, enemy2.player.position);
            if (dist <= enemy2.attackRange)
            {
                enemy2.ChangeState(Enemy2.EnemyState.Attack);
            }
            else
            {
                enemy2.ChangeState(Enemy2.EnemyState.Run);
            }
        }
    }
}
