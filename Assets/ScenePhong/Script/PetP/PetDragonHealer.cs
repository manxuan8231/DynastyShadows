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
    public float floatHeight = 1.5f;
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

    [Header("Heal VFX")]
    public GameObject healEffectPrefab;

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

        // Tự động gán player nếu null
        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        // Set độ cao cho NavMesh
        if (navMeshAgent != null)
            navMeshAgent.baseOffset = floatHeight;
    }

    void Update()
    {
        AvoidOtherPets();
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

        // NavMesh
        if (pathfindingType == PathfindingType.NavMesh && navMeshAgent != null && navMeshAgent.isOnNavMesh)
        {
            navMeshAgent.speed = speed;

            if (distance > followDistance)
            {
                navMeshAgent.SetDestination(player.position);
                anim?.SetBool("isFollowing", true);
            }
            else
            {
                navMeshAgent.ResetPath();
                anim?.SetBool("isFollowing", false);
            }

            return;
        }

        // A* Pathfinding
        if (pathfindingType == PathfindingType.AStar && aiPath != null && destinationSetter != null && aiPath.enabled)
        {
            aiPath.maxSpeed = speed;

            if (distance > followDistance)
            {
                destinationSetter.target = player;
                anim?.SetBool("isFollowing", true);
            }
            else
            {
                destinationSetter.target = null;
                anim?.SetBool("isFollowing", false);
            }

            return;
        }

        // Manual Move
        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 targetPos = player.position - direction * followDistance;
            targetPos.y = floatHeight;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            anim?.SetBool("isFollowing", true);
        }
        else
        {
            anim?.SetBool("isFollowing", false);
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

            anim?.SetTrigger("doBuff");

            if (audioSource != null && healSound != null)
                audioSource.PlayOneShot(healSound);

            // ✨ Spawn heal VFX trên Player
            if (healEffectPrefab != null && player != null)
            {
                GameObject healVFX = Instantiate(healEffectPrefab, player.position + Vector3.up * 0f, Quaternion.identity);
                Destroy(healVFX, 3f);
            }
        }
        else
        {
            Debug.Log("🧘 Máu chưa đủ thấp để buff.");
        }
    }
    void AvoidOtherPets()
    {
        float minPetDistance = 2f; // Khoảng cách tối thiểu giữa các pet
        GameObject[] pets = GameObject.FindGameObjectsWithTag("Pet");

        foreach (GameObject pet in pets)
        {
            if (pet != this.gameObject)
            {
                float dist = Vector3.Distance(transform.position, pet.transform.position);
                if (dist < minPetDistance)
                {
                    Vector3 pushDir = (transform.position - pet.transform.position).normalized;
                    transform.position += pushDir * (minPetDistance - dist) * Time.deltaTime;
                }
            }
        }
    }

}
