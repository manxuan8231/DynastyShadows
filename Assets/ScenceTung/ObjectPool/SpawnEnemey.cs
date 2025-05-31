using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpawned) return;

        if (other.CompareTag("Player"))
        {
            SpawnEnemies();
            hasSpawned = true;
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
