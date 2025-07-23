using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class PetDragonBlue : MonoBehaviour
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
    private float buffCooldown = 180f; // 3 phút
    private float buffTimer;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;
    private Vector3 startPos;

    [Header("Audio")]
    public AudioClip manaSound;
    private AudioSource audioSource;

    [Header("Mana Buff VFX")]
    public GameObject manaEffectPrefab;

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

        if (player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();

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
            TryBuffMana();
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

    void TryBuffMana()
    {
        if (buffManager == null) return;

        float manaPercent = buffManager.GetPlayerManaPercent();
        Debug.Log("⚡ Mana Percent: " + manaPercent);

        if (manaPercent < 0.1f) // đúng là dưới 10%
        {
            Debug.Log("🐉 Rồng Xanh Dương buff mana cho người chơi!");
            buffManager.BuffMana();

            anim?.SetTrigger("doBuff");

            if (audioSource != null && manaSound != null)
                audioSource.PlayOneShot(manaSound);

            if (manaEffectPrefab != null && player != null)
            {
                GameObject vfx = Instantiate(manaEffectPrefab, player.position + Vector3.up * 0f, Quaternion.identity);
                Destroy(vfx, 3f);
            }
        }
        else
        {
            Debug.Log("🔋 Mana chưa dưới 10%, chưa cần buff.");
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
