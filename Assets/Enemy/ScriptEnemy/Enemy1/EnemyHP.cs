using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour,IDamageable
{
    //xử lý máu
    [SerializeField] public Slider sliderHp;
    [SerializeField] private Slider easeSliderHp;
   [SerializeField] public float currentHealth;
    [SerializeField]  public float maxHealth = 2000f;
    [SerializeField] private float lerpSpeed = 0.04f;
    //drop exp
    [SerializeField] public GameObject expPrefab;
    
    //gọi hàm
    Enemy1 enemy1;
    Quest3 quest3;
    public NecController Necboss;
    //box nhận dame
    public BoxCollider boxDame;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    void OnEnable()
    {
        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại
    }
    private void Awake()
    {
        enemy1 = GetComponent<Enemy1>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        easeSliderHp.maxValue = currentHealth;
        easeSliderHp.value = currentHealth;
        enemy1 = GetComponent<Enemy1>(); // <- GÁN Ở ĐÂY
        quest3 = FindAnyObjectByType<Quest3>();
        Necboss = FindAnyObjectByType<NecController>();
        
    }
    public void Update()
    {
        if (sliderHp.value != easeSliderHp.value)
        {
            easeSliderHp.value = Mathf.Lerp(easeSliderHp.value, currentHealth, lerpSpeed);
        }
        if(currentHealth <= 0)
        {
            enemy1.animator.enabled = true;
            enemy1.enabled = true;
        }
    }



    public void TakeDamage(float damage)
    {
       // if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
      
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
       
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }
        else if (currentHealth <= 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.Death); // Đặt trạng thái là Death
            boxDame.enabled = false;
            DropItem(); // Gọi hàm rơi đồ
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            StartCoroutine(WaitDeath()); // Chờ 5 giây trước khi trả về pool

            if (Necboss != null)
            {
                Necboss.EnemyCount();
            }
            if (quest3 != null)
            {
                quest3.UpdateKillEnemy(1);
            }
          

        }
        
   
    }
    IEnumerator WaitDeath()
    {
        enemy1.animator.SetTrigger("Death"); // Chơi animation chết
        yield return new WaitForSeconds(5f); // Thời gian chờ trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("Enemy1", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
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
    void ResetEnemy()
    {
        boxDame.enabled = true;
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy1.ChangeState(Enemy1.EnemyState.Idle); // Đặt lại trạng thái về Idle
        if (enemy1.animator != null)
        {
            enemy1.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemy1.animator.Update(0f);      // Đảm bảo không bị đứng hình ở frame cũ
        }
        // Reset trạng thái di chuyển
        if (enemy1.agent != null)
        {
            enemy1.agent.ResetPath();
            enemy1.agent.enabled = true;
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


[System.Serializable]
public class  ItemDrop
{
    public GameObject itemPrefabs;
    public float dropRate;

}