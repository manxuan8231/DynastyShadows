using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class PetDragonRed : MonoBehaviour
{
    public enum PathfindingType { None, NavMesh, AStar }
    public PathfindingType pathfindingType = PathfindingType.None;

    [Header("Follow Settings")]
    public Transform player;
    public float followDistance = 2.5f;
    public float moveSpeed = 3f;
    public float speedChangeDistance = 5f;
    private PlayerStatus playerStats;

    [Header("Buff Settings")]
    public BuffManager buffManager;
    private float buffCooldown = 5f;
    private float buffTimer = 0f;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    public float hoverOffset = 3f;

    [Header("Audio")]
    public AudioClip damageBuffSound;
    private AudioSource audioSource;

    [Header("VFX")]
    public GameObject damageBuffEffectPrefab;

    private Animator anim;

    private NavMeshAgent navMeshAgent;
    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
            playerStats = player.GetComponent<PlayerStatus>();

        if (playerStats == null)
            playerStats = FindAnyObjectByType<PlayerStatus>();

        if (buffManager == null)
            buffManager = FindAnyObjectByType<BuffManager>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

        if (navMeshAgent != null)
            navMeshAgent.baseOffset = 0f;
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
            BuffDamageToPlayer();
        }
    }

    void AnimateFloating()
    {
        if (player == null) return;

        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = player.position.y + hoverOffset + newY;
        transform.position = pos;
    }

    void FollowPlayer()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        float speed = (distance > speedChangeDistance) ? moveSpeed * 1.5f : moveSpeed * 0.5f;

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

        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 targetPos = player.position - direction * followDistance;
            targetPos.y = player.position.y + hoverOffset;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            anim?.SetBool("isFollowing", true);
        }
        else
        {
            anim?.SetBool("isFollowing", false);
        }
    }

    void BuffDamageToPlayer()
    {
        if (playerStats == null || buffManager == null) return;

        Debug.Log("🔥 Rồng Đỏ buff sát thương tạm thời cho người chơi!");
        buffManager.BuffDamage();

        anim?.SetTrigger("doBuff");

        if (audioSource != null && damageBuffSound != null)
            audioSource.PlayOneShot(damageBuffSound);

        if (damageBuffEffectPrefab != null && player != null)
        {
            // Instantiate effect at player position, no rotation
            GameObject vfx = Instantiate(damageBuffEffectPrefab, player.position, Quaternion.identity);

            // Make it follow player but not inherit rotation or scale
            vfx.transform.SetParent(player, worldPositionStays: true);

            // Optional: ensure localRotation and localScale are untouched
            vfx.transform.localRotation = Quaternion.identity;

            Destroy(vfx, 1f);
        }
    }

    void AvoidOtherPets()
    {
        float minPetDistance = 2f;
        GameObject[] pets = GameObject.FindGameObjectsWithTag("Pet");

        foreach (GameObject pet in pets)
        {
            if (pet != this.gameObject)
            {
                Vector3 offset = transform.position - pet.transform.position;
                offset.y = 0f; // chỉ đẩy trên mặt phẳng XZ

                float dist = offset.magnitude;
                if (dist < minPetDistance && dist > 0.01f)
                {
                    Vector3 pushDir = offset.normalized;
                    float pushForce = (minPetDistance - dist) * 0.5f;

                    transform.position += pushDir * pushForce * Time.deltaTime;
                }
            }
        }
    }
}
