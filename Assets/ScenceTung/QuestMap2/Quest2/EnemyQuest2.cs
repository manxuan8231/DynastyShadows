using UnityEngine;

public class EnemyQuest2 : MonoBehaviour
{
    public Transform player;
    public float viewRadius = 15f;
    [Range(0, 360)]
    public float viewAngle = 120f;

    public LayerMask playerMask;
    public LayerMask obstacleMask;

    private bool playerInSight;

    public Transform[] spawnPoints; // Vị trí spawn sẵn
    public int enemySpawnCount; // Số enemy muốn spawn
    public string enemyTag; // Tag của enemy dùng để gọi từ pool
    public bool hasSpawned = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform; // Tìm player trong scene
    }

    void Update()
    {
        DetectPlayer();
        if (playerInSight)
        {
            if (!hasSpawned)
            {
                SpawnEnemies();
                hasSpawned = true; // Đánh dấu đã spawn
            }
        }

    }

    void DetectPlayer()
    {
        playerInSight = false;

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < viewRadius)
        {
            float angle = Vector3.Angle(transform.forward, dirToPlayer);
            if (angle < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer, distanceToPlayer, obstacleMask))
                {
                    // Kiểm tra xem player có trong tầm nhìn không
                    playerInSight = true;

                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftDir = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftDir * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightDir * viewRadius);
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