using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHealth;
    public float maxHealth = 2000f;
    //drop exp
    public GameObject expPrefab;
    
    //gọi hàm
    Enemy1 enemy1;
    Quest3 quest3;
    //box nhận dame
    public BoxCollider boxDame;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemy1 = GetComponent<Enemy1>(); // <- GÁN Ở ĐÂY
        quest3 = FindAnyObjectByType<Quest3>();
    }

    // Update is called once per frame
    void Update()
    {
       
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
            enemy1.ChangeState(Enemy1.EnemyState.Death);
            enemy1.agent.isStopped = true; // Dừng lại khi chết
            boxDame.enabled = false;
            DropItem(); // Gọi hàm rơi đồ
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 3f);
            quest3.UpdateKillEnemy(1);
            // Tạo expPrefab tại vị trí của enemy
        }
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }
        
   
    }
    public void TakeDamageHit(float damage)
    {
        if (enemy1.currentState == Enemy1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            enemy1.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemy1.enabled = true; // Bật lại Enemy1 để có thể chơi animation chết
            enemy1.ChangeState(Enemy1.EnemyState.Death);
            enemy1.agent.isStopped = true; // Dừng lại khi chết
            boxDame.enabled = false;
            DropItem(); // Gọi hàm rơi đồ
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject, 3f);
            quest3.UpdateKillEnemy(1);//
            // Tạo expPrefab tại vị trí của enemy

        }
        if (currentHealth > 0)
        {
            enemy1.ChangeState(Enemy1.EnemyState.GetHit);


            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }

       
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
}

[System.Serializable]
public class  ItemDrop
{
    public GameObject itemPrefabs;
    public float dropRate;

}