using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [Header("Detection & Attack")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public float stopDistance = 1.8f; // 👉 khoảng cách dừng lại gần Player (để không áp sát hoàn toàn)

    [Header("References")]
    public Transform player; // Kéo Player vào hoặc tự tìm theo tag
    public LayerMask raycastMask;

    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Không tìm thấy GameObject có tag 'Player'");
            }
        }

        // (Tuỳ game: Nếu không dùng Layer Collision thì có thể bỏ dòng sau)
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (distance > stopDistance)
            {
                agent.isStopped = false;
                Vector3 direction = (player.position - transform.position).normalized;
                Vector3 targetPosition = player.position - direction * stopDistance;

                agent.SetDestination(targetPosition);
                animator.SetBool("isWalking", true);
            }
            else
            {
                agent.isStopped = true;
                animator.SetBool("isWalking", false);
            }

            if (distance <= attackRange && CanSeePlayer())
            {
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Attack();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
        }
    }


    void Attack()
    {
        animator.SetTrigger("Attack");

        // Gây sát thương nếu Player còn sống
        TurretHP playerHealth = player.GetComponent<TurretHP>();
        if (playerHealth != null)
        {
            playerHealth.TakeDame(10f); // Có thể chỉnh lượng sát thương
        }
    }


    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 direction = (player.position + Vector3.up) - origin;

        Debug.DrawRay(origin, direction.normalized * attackRange, Color.red, 0.2f);

        if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, attackRange, raycastMask))
        {
            return hit.transform == player;
        }

        return false;
    }

    // 🔍 Tùy chọn: vẽ phạm vi detection/attack trong Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
