using System.Collections.Generic;
using UnityEditor.UIElements;
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
    Quest1 questManager; // Tham chiếu đến QuestManager
    ThuongNhan thuongNhan; // Tham chiếu đến ThuongNhan
                           //box nhận dame                      
    public BoxCollider boxDame;
    void OnEnable()
    {
        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy3 = GetComponent<Enemy3>(); // <- GÁN Ở ĐÂY
        questManager = FindAnyObjectByType<Quest1>(); // Lấy tham chiếu đến QuestManager
        thuongNhan = FindAnyObjectByType<ThuongNhan>(); // Lấy tham chiếu đến ThuongNhan
        boxDame = GetComponent<BoxCollider>(); // Lấy BoxCollider để nhận damage
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
            currentHealth = 0;
            enemy3.ChangeState(Enemy3.EnemyState.Death);
            enemy3.agent.isStopped = true; // Dừng lại khi chết
            DropItem();
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            
            // Gọi hàm cập nhật quest khi quái chết
            if (questManager != null) 
                questManager.UpdateQuestBacLam(1);
            if (thuongNhan != null)
                thuongNhan.UpdateKillEnemy(1); //  Cập nhật số lượng kẻ thù đã tiêu diệt trong quest thuong nhan
            ObjPoolingManager.Instance.ReturnToPool("Enemy3",gameObject); // Trả về pool thay vì Destroy để tái sử dụng
            
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
           
            currentHealth = 0;
            enemy3.ChangeState(Enemy3.EnemyState.Death);
            enemy3.agent.isStopped = true; // Dừng lại khi chết
            DropItem();
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
            // Gọi hàm cập nhật quest khi quái chết
            questManager.UpdateQuestBacLam(1);
            thuongNhan.UpdateKillEnemy(1); // Cập nhật số lượng kẻ thù đã tiêu diệt trong quest thuong nhan
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

    void ResetEnemy()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;

        boxDame.enabled = true;
        if (enemy3.animator != null)
        {
            enemy3.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemy3.animator.Update(0f);      // Đảm bảo không bị đứng hình ở frame cũ
        }
        // Reset trạng thái di chuyển
        if (enemy3.agent != null)
        {
            enemy3.agent.ResetPath();
            enemy3.agent.enabled = true;
        }

    }
}
