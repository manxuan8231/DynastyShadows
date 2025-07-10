using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP3 : MonoBehaviour, IDamageable
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
    ActiveStartTimeLine6 activeStartTimeLine6;                                  
    public BoxCollider boxDame;
   
    private void Awake()
    {
        enemy3 = GetComponent<Enemy3>();
        activeStartTimeLine6 = FindFirstObjectByType<ActiveStartTimeLine6>(); // Lấy tham chiếu đến ActiveStartTimeLine6
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
        foreach (ItemDrop item in itemDrops)
        {
            float chance = item.dropRate / 100f;
            if (Random.value <= chance) // randon ra số từ 0 đến 1
            {
                Instantiate(item.itemPrefabs, transform.position + Vector3.up, Quaternion.identity);
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
            enemy3.ChangeState(Enemy3.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
                    Invoke(nameof(BackToChase), 0.2f);
        }
        else
        {
            enemy3.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemy3.enabled = true; // Bật lại Enemy2 để có thể chơi animation chết
            enemy3.agent.enabled = true; // Bật lại NavMeshAgent để có thể chơi animation chết
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
            if (activeStartTimeLine6 != null)
            {
                activeStartTimeLine6.Count(); // Bật timeline 6
            }
            StartCoroutine(WaitDeath()); // Chờ 5 giây trước khi trả về pool
        }
    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(5f); // Chờ 5 giây trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("Enemy3", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
        enemy3.hasFirstPos = false; // Đặt lại hasFirstPos để có thể spawn lại
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
