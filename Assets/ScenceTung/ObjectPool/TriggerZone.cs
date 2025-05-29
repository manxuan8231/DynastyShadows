using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public Transform[] spawnPoints;
    public int enemyCountToSpawn = 20;
    private bool hasSpawned = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            hasSpawned = true;
            SpawnEnemiesInstantly();
        }
    }

    void SpawnEnemiesInstantly()
    {
        for (int i = 0; i < enemyCountToSpawn; i++)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            EnemyPoolManager.Instance.GetEnemyFromPool(randomPoint.position);
        }
    }

}
