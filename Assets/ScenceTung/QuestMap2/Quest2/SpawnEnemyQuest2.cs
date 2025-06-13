using UnityEngine;

public class SpawnEnemyQuest2 : MonoBehaviour
{

    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;
    public NPCDeathQuest NPCDeathQuest;
    private void Start()
    {
        NPCDeathQuest = FindFirstObjectByType<NPCDeathQuest>();
    }


    private void Update()
    {
        if (NPCDeathQuest.isQuest1 == true)
        {
            if (hasSpawned) return;
                SpawnEnemies();
               hasSpawned = true; // Đánh dấu đã spawn
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
