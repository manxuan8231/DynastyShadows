using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public enum HealthState
{
   Full,
   At30,
   At70,
   isDead
}
public class StatueQuestHP : MonoBehaviour,IDamageable
{
    public float currentHealth ;
    public float maxHealth ;
    public Slider sliderHp;
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    private HealthState healthState = HealthState.Full;
    public ActiveQuest8 quest8;

    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        quest8 = FindAnyObjectByType<ActiveQuest8>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (healthState == HealthState.isDead) return; 
      
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            healthState = HealthState.isDead; // Cập nhật trạng thái khi máu về 0
            quest8.StartQuest2();
            Destroy(gameObject); // Hủy đối tượng khi máu về 0
        }
        if (currentHealth <= maxHealth * 0.7f && healthState == HealthState.Full)
        {
            SpawnEnemies();
            Debug.Log("Đã spawn enemy khi máu về 70%");
            healthState = HealthState.At70; // Cập nhật trạng thái
        }
        if (currentHealth <= maxHealth * 0.3f && healthState == HealthState.At70)
        {
            Debug.Log("Máu đã xuống dưới 30%");
            // Thực hiện hành động khác nếu cần
            SpawnEnemies(); // Có thể spawn thêm nếu cần
            healthState = HealthState.At30; // Cập nhật trạng thái
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
