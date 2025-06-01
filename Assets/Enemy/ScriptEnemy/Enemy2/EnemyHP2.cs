using NUnit.Framework;
using System.Collections.Generic;
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
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    //box nhận dame                      
    public BoxCollider boxDame;
    private void Awake()
    {
        enemy2 = GetComponent<Enemy2>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy2 = GetComponent<Enemy2>(); // <- GÁN Ở ĐÂY
       knightD= FindAnyObjectByType<KnightD>();
        boxDame = GetComponent<BoxCollider>(); // Lấy BoxCollider để nhận damage
    }
    
    void OnEnable()
    {
        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại
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

    // Update is called once per frame
    void Update()
    {
       
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
            DropItem();       
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
           if(knightD != null)
            {
                knightD.UpdateKillCount(1); // Gọi hàm cập nhật quest
            }
          ObjPoolingManager.Instance.ReturnToPool("Enemy2", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
          
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


            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            enemy2.ChangeState(Enemy2.EnemyState.Death);
            enemy2.agent.isStopped = true; // Dừng lại khi chết
            DropItem();

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            if (knightD != null)
            {
                knightD.UpdateKillCount(1); // Gọi hàm cập nhật quest
            }
            ObjPoolingManager.Instance.ReturnToPool("Enemy2", gameObject); // Trả về pool thay vì Destroy để tái sử dụng

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

    void ResetEnemy()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;

        boxDame.enabled = true;
        if (enemy2.animator != null)
        {
            enemy2.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemy2.animator.Update(0f);      // Đảm bảo không bị đứng hình ở frame cũ
        }
        // Reset trạng thái di chuyển
        if (enemy2.agent != null)
        {
            enemy2.agent.ResetPath();
            enemy2.agent.enabled = true;
        }

    }
}

