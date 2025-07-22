using UnityEngine;
using UnityEngine.AI;
using Pathfinding; // A* Pathfinding Project

public class PetDragonHealer : MonoBehaviour
{
    public enum PathfindingType { None, NavMesh, AStar }
    public PathfindingType pathfindingType = PathfindingType.None;

    [Header("Follow Settings")]
    public Transform player;
    public float followDistance = 2.5f;
    public float moveSpeed = 3f;
    public float speedChangeDistance = 5f;
    public float floatHeight = 1.5f; // ⚡ Độ cao mặc định
    private PlayerStatus playerStats;

    [Header("Buff Settings")]
    public BuffManager buffManager;
    private float buffCooldown = 5f;
    private float buffTimer;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    private Vector3 startPos;

    [Header("Audio")]
    public AudioClip healSound;
    private AudioSource audioSource;

    private Animator anim;

    // AI Components
    private NavMeshAgent navMeshAgent;
    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;

    void Start()
    {
        startPos = transform.position;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerStats = FindAnyObjectByType<PlayerStatus>();

        // 🔁 Tự động gán player nếu chưa có
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        // Nếu dùng NavMesh, đảm bảo agent không bị lệch độ cao
        if (navMeshAgent != null)
            navMeshAgent.baseOffset = floatHeight;
    }

    void Update()
    {
        AnimateFloating();
        FollowPlayer();

        buffTimer += Time.deltaTime;
        if (buffTimer >= buffCooldown)
        {
            buffTimer = 0f;
            HealPlayer();
        }
    }

    void AnimateFloating()
    {
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, floatHeight + newY, transform.position.z);
    }

    void FollowPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float speed = (distance > speedChangeDistance) ? moveSpeed * 1.5f : moveSpeed * 0.5f;

        // AI: NavMesh
        if (pathfindingType == PathfindingType.NavMesh && navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.speed = speed;

            if (distance > followDistance)
            {
                navMeshAgent.SetDestination(player.position);
                if (anim != null) anim.SetBool("isFollowing", true);
            }
            else
            {
                navMeshAgent.ResetPath();
                if (anim != null) anim.SetBool("isFollowing", false);
            }

            return;
        }

        // AI: A*
        if (pathfindingType == PathfindingType.AStar && aiPath != null && destinationSetter != null && aiPath.enabled)
        {
            aiPath.maxSpeed = speed;

            if (distance > followDistance)
            {
                destinationSetter.target = player;
                if (anim != null) anim.SetBool("isFollowing", true);
            }
            else
            {
                destinationSetter.target = null;
                if (anim != null) anim.SetBool("isFollowing", false);
            }

            return;
        }

        // Manual Move (Không dùng AI)
        Vector3 direction = (player.position - transform.position).normalized;
        if (distance > followDistance)
        {
            Vector3 targetPos = player.position - direction * followDistance;
            targetPos.y = floatHeight; // 🔁 Đảm bảo cao đúng tầm

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            if (anim != null) anim.SetBool("isFollowing", true);
        }
        else
        {
            if (anim != null) anim.SetBool("isFollowing", false);
        }
    }

    void HealPlayer()
    {
        if (playerStats == null || buffManager == null) return;

        float healthPercent = (float)playerStats.currentHp / playerStats.maxHp;

        if (healthPercent < 0.6f)
        {
            Debug.Log("🐉 Rồng Xanh hồi máu cho người chơi!");
            buffManager.BuffHP();

            if (anim != null)
                anim.SetTrigger("doBuff");

            if (audioSource != null && healSound != null)
                audioSource.PlayOneShot(healSound);
        }
        else
        {
            Debug.Log("🧘 Máu chưa đủ thấp để buff.");
        }
    }
}
