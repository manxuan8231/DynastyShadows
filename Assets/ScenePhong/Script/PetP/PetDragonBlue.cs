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
    public float hoverOffset = 3f;
    private PlayerStatus playerStats;

    [Header("Buff Settings")]
    public BuffManager buffManager;

    [Header("Manual Buff")]
    public float manualBuffCooldown = 60f;
    private float manualBuffTimer = 0f;
    private bool canManualBuff = true;

    [Header("Floating Animation")]
    public float floatAmplitude = 0.25f;
    public float floatFrequency = 1f;

    [Header("Audio & VFX")]
    public AudioClip manaSound;
    public GameObject manaEffectPrefab;
    public float manaEffectHeightOffset = 0.5f;

    private AudioSource audioSource;
    private Animator anim;
    private NavMeshAgent navMeshAgent;
    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null) player = foundPlayer.transform;
        }

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
            navMeshAgent.baseOffset = hoverOffset;
    }

    void Update()
    {
        if (player == null) return;

        AvoidOtherPets();
        AnimateFloating();
        FollowPlayer();

        // Buff thủ công bằng phím Z
        if (canManualBuff && Input.GetKeyDown(KeyCode.Z))
        {
            ManualBuffMana();
        }

        // Cooldown buff thủ công
        if (!canManualBuff)
        {
            manualBuffTimer -= Time.deltaTime;
            if (manualBuffTimer <= 0f)
            {
                canManualBuff = true;
            }
        }
    }

    void AnimateFloating()
    {
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        Vector3 pos = transform.position;
        pos.y = player.position.y + hoverOffset + newY;
        transform.position = pos;
    }

    void FollowPlayer()
    {
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
            Vector3 dir = (player.position - transform.position).normalized;
            Vector3 targetPos = player.position - dir * followDistance;
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

    public void ManualBuffMana()
    {
        if (!canManualBuff || playerStats == null || buffManager == null) return;

        Debug.Log("💧 Buff mana thủ công bằng phím Z.");
        buffManager.Buffmana();
        PlayBuffEffects();

        canManualBuff = false;
        manualBuffTimer = manualBuffCooldown;
    }

    void PlayBuffEffects()
    {
        anim?.SetTrigger("doBuff");

        if (audioSource != null && manaSound != null)
            audioSource.PlayOneShot(manaSound);

        if (manaEffectPrefab != null && player != null)
        {
            GameObject vfx = Instantiate(
                manaEffectPrefab,
                player.position + Vector3.up * manaEffectHeightOffset,
                Quaternion.identity
            );

            vfx.transform.SetParent(player, worldPositionStays: true);
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
                offset.y = 0f;
                float dist = offset.magnitude;

                if (dist < minPetDistance && dist > 0.01f)
                {
                    Vector3 pushDir = offset.normalized;
                    float pushStrength = (minPetDistance - dist) * 0.5f;
                    transform.position += pushDir * pushStrength * Time.deltaTime;
                }
            }
        }
    }
}
