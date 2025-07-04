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
   [SerializeField] public float currentHealth;
    [SerializeField]  public float maxHealth = 2000f;
    //drop exp
    [SerializeField] public GameObject expPrefab;
    
    //gọi hàm
    Enemy1 enemy1;
    Quest3 quest3;
    NecController Necboss;
    //box nhận dame
    public BoxCollider boxDame;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    void OnEnable()
    {
        StartCoroutine(WaitThenReset());
    }
    IEnumerator WaitThenReset()
    {
        yield return null; // đợi 1 frame cho object "snap" lên NavMesh
        ResetEnemy();
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
        enemy1 = GetComponent<Enemy1>(); // <- GÁN Ở ĐÂY
        quest3 = FindAnyObjectByType<Quest3>();
        Necboss = FindAnyObjectByType<NecController>();
        
    }
    

 
   
    public void TakeDamage(float damage)
    {
        if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
      
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {

            enemy1.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemy1.enabled = true; // Bật lại Enemy1 để có thể chơi animation chết
            enemy1.agent.isStopped = true; // Dừng lại khi chết
            boxDame.enabled = false;
            DropItem(); // Gọi hàm rơi đồ
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            if (quest3 != null)
            {
                quest3.UpdateKillEnemy(1);
            }
            if(Necboss != null)
            {
                Necboss.EnemyCount();
            }

            StartCoroutine(WaitDeath()); // Chờ 5 giây trước khi trả về pool
            enemy1.ChangeState(Enemy1.EnemyState.Death);



        }
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }
        
   
    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(5f); // Thời gian chờ trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("Enemy1", gameObject); // Trả về pool thay vì Destroy để tái sử dụng
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

    void ResetEnemy()
    {
       
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        
        boxDame.enabled = true;
        if (enemy1.animator != null)
        {
            enemy1.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemy1.animator.Update(0f);
        }

        if (enemy1.agent != null && enemy1.agent.isOnNavMesh)
        {
            enemy1.agent.ResetPath();
            enemy1.agent.enabled = true;
        }

        enemy1.ChangeState(Enemy1.EnemyState.Idle);
    }
   

}

[System.Serializable]
public class  ItemDrop
{
    public GameObject itemPrefabs;
    public float dropRate;

}