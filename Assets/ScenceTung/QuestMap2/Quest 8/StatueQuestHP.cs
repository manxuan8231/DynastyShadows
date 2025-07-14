using UnityEngine;
using UnityEngine.UI;

public class StatueQuestHP : MonoBehaviour,IDamageable
{
    public float currentHealth ;
    public float maxHealth ;
    public Slider sliderHp;
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;


    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 3f); // Hủy đối tượng khi máu về 0
        }
        if (currentHealth <= maxHealth * 0.3f && !hasSpawned)
        {
            SpawnEnemies();
            hasSpawned = true; // Đánh dấu đã spawn
            Debug.Log("Đã spawn enemy khi máu về 30%");
        }
    }
    void SpawnEnemies()
    {
        int count = Mathf.Min(enemySpawnCount, spawnPoints.Length);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = spawnPoints[i].position;

            GameObject enemy = ObjPoolingManager.Instance.GetEnemyFromPool(enemyTag, spawnPos);

            if (enemy == null)
            {
                Debug.LogWarning("Enemy không đủ trong pool!");
            }
        }

    }
}
