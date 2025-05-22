using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP3 : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;
    public GameObject textDame;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    //gọi hàm
    Enemy3 enemy3;
    QuestManager questManager; // Tham chiếu đến QuestManager
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy3 = GetComponent<Enemy3>(); // <- GÁN Ở ĐÂY
        questManager = FindAnyObjectByType<QuestManager>(); // Lấy tham chiếu đến QuestManager
    }
    public void DropItem()
    {
        float rand = Random.Range(0f, 100f); // số ngẫu nhiên từ 0 đến 100
        float cumulative = 0f;

        foreach (ItemDrop item in itemDrops)
        {
            cumulative += item.dropRate;
            if (rand <= cumulative)
            {
                Instantiate(item.itemPrefabs, transform.position + Vector3.up, Quaternion.identity);
                return;
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(100); // Gọi hàm giảm máu
        }
    }

    public void TakeDamage(float damage)
    {
        if (enemy3.currentState == Enemy3.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

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
            // Gọi hàm cập nhật quest khi quái chết
            questManager.UpdateQuestBacLam(1); // Cập nhật quest ở đây
            currentHealth = 0;
            enemy3.ChangeState(Enemy3.EnemyState.Death);
            enemy3.agent.isStopped = true; // Dừng lại khi chết
            DropItem();
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
    public void TakeDamageHit(float damage)
    {
        if (enemy3.currentState == Enemy3.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;



        if (currentHealth > 0)
        {
            enemy3.ChangeState(Enemy3.EnemyState.GetHit);

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            // Gọi hàm cập nhật quest khi quái chết
            questManager.UpdateQuestBacLam(1); // Cập nhật quest ở đây
            currentHealth = 0;
            enemy3.ChangeState(Enemy3.EnemyState.Death);
            enemy3.agent.isStopped = true; // Dừng lại khi chết
            DropItem();
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
   
    void BackToChase()
    {
        if (enemy3.currentState != Enemy3.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, enemy3.player.position);
            if (dist <= enemy3.attackRange)
            {
                enemy3.ChangeState(Enemy3.EnemyState.Attack);
            }
            else
            {
                enemy3.ChangeState(Enemy3.EnemyState.Run);
            }
        }
    }
}
