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

    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy2 = GetComponent<Enemy2>(); // <- GÁN Ở ĐÂY

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
        ShowTextDame(damage);
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

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
    public void TakeDamageHit(float damage)
    {
        if (enemy2.currentState == Enemy2.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        ShowTextDame(damage);
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

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }

    public void ShowTextDame(float damage)
    {
        // Random lệch trái/phải: -0.5 đến 0.5
        float offsetX = Random.Range(-1f, 1f);
        float offsetY = Random.Range(1,3);
        float offsetZ = Random.Range(-1f, 1f);
        Vector3 spawnPos = transform.position + new Vector3(offsetX, offsetY, offsetZ); // Lệch X, cao lên trục Y

        GameObject effectText = Instantiate(textDame, spawnPos, Quaternion.identity);
        Destroy(effectText, 0.5f);
        // Truyền dame vào prefab
        TextDamePopup popup = effectText.GetComponent<TextDamePopup>();
        if (popup != null)
        {
            popup.Setup(damage);
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
