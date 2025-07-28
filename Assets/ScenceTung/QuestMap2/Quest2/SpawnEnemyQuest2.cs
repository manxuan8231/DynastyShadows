using UnityEngine;

public class SpawnEnemyQuest2 : MonoBehaviour
{

    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;
    public NPCDeathQuest NPCDeathQuest;
    SuccesQuest2 succesQuest2;
    private void Start()
    {
        NPCDeathQuest = FindFirstObjectByType<NPCDeathQuest>();
        succesQuest2 = FindFirstObjectByType<SuccesQuest2>();
    }


    private void Update()
    {
        if(succesQuest2.isQuest2Complete) return; // Nếu quest 2 đã hoàn thành thì không spawn nữa

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
