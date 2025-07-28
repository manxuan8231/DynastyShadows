using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP4 : MonoBehaviour, IDamageable
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;

    //box nhận dame
    public BoxCollider boxDame;

    //gọi hàm
    Enemy4 enemy4;
    QuestDesert5 questDesert5;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();

    void OnEnable()
    {
        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại
    }
    private void Awake()
    {
        enemy4 = GetComponent<Enemy4>();
    }
    public void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy4 = GetComponent<Enemy4>(); // <- GÁN Ở ĐÂY
        questDesert5 = FindAnyObjectByType<QuestDesert5>();
        boxDame = GetComponent<BoxCollider>(); // Lấy BoxCollider để nhận damage
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void DropItem()
    {
        foreach (ItemDrop item in itemDrops)
        {
            float chance = item.dropRate / 100f;
            if (Random.value <= chance) // randon ra số từ 0 đến 1
            {
                Instantiate(item.itemPrefabs, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        //if (enemy4.currentState == Enemy4.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;

        if (currentHealth > 0)
        {
            enemy4.ChangeState(Enemy4.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }
        else if(currentHealth <= 0)
        {
            boxDame.enabled = false; // Tắt box dame khi chết
            enemy4.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemy4.enabled = true; // Bật lại Enemy2 để có thể chơi animation chết
            enemy4.agent.enabled = true; // Bật lại NavMeshAgent để có thể chơi animation chết
            currentHealth = 0;
            enemy4.ChangeState(Enemy4.EnemyState.Death);
            if(enemy4.agent.enabled)
            enemy4.agent.isStopped = true; // Dừng lại khi chết
            if(questDesert5 != null){
            questDesert5.UpdateKillEnemy(1); // Cập nhật số lượng kẻ thù đã giết trong nhiệm vụ
            }
            DropItem();
            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            StartCoroutine(WaitDeath()); // Trả về pool thay vì Destroy để tái sử dụng
        }
    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(5f); // Chờ 5 giây trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("Enemy4", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
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

    void ResetEnemy()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = maxHealth;
        sliderHp.value = maxHealth;
        enemy4.firstPos = transform.position; // Reset vị trí ban đầu

        boxDame.enabled = true;
        if (enemy4.animator != null)
        {
            enemy4.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemy4.animator.Update(0f);      // Đảm bảo không bị đứng hình ở frame cũ
        }
        // Reset trạng thái di chuyển
        if (enemy4.agent != null)
        {
            enemy4.agent.ResetPath();
            enemy4.agent.enabled = true;
        }

    }
}
