using NUnit.Framework.Constraints;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Boss1Controller : MonoBehaviour
{
    [Header("State Management")]
    public Boss1State currentState;

    [Header("Components")]
    public Animator anmt;
    public NavMeshAgent agent;
    public Transform player;
    public Boss1HP hp;

    [Header("Combat Settings")]
    public float attackTimer = -9f;
    public float attackCooldown = 9f;
    public bool isAttacking = false;
    public float attackRange = 5f;

    [Header("Detection & Movement")]
    public float radius = 100f;

    [Header("Skills")]
    public float skillRange = 15f;
    public float skillTimer = 0f;
    public float skillCooldown = 25f;
    public bool isUsingSkill = false;
    public GameObject skill1Prefabs;
    public GameObject skill2Prefabs;
    public Transform skill2Pos;
    public GameObject skill3Prefabs;
    public GameObject skill4Prefabs;
    public Vector3 playerPos;

    private void Awake()
    {
        InitializeComponents();
        InitializeState();
        FindPlayer();
    }

    private void InitializeComponents()
    {
        hp = GetComponent<Boss1HP>() ?? FindAnyObjectByType<Boss1HP>();
        agent = GetComponent<NavMeshAgent>();
        anmt = GetComponent<Animator>();

        if (skill1Prefabs != null)
            skill1Prefabs.SetActive(false);
        if (skill2Prefabs != null)
            skill2Prefabs.SetActive(false);
        if(skill3Prefabs != null)
        {
            skill3Prefabs.SetActive(false);
        } if(skill4Prefabs != null)
        {
            skill4Prefabs.SetActive(false);
        }
    }

    private void InitializeState()
    {
        ChangState(new Boss1IdleState(this));
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure player has 'Player' tag.");
        }
    }

    void Update()
    {
        currentState?.Update();
        playerPos = player.transform.position;
    }

    public void ChangState(Boss1State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    // Animation Event - called when attack animation completes
    public void OnAttackComplete()
    {
        isAttacking = false;
       
    }

    // Animation Event - called when skill animation completes
    public void OnSkillComplete()
    {
        isUsingSkill = false;
  
    }

    public void Skill2()
    {
        transform.LookAt(player.position);

        // Activate cục sét và bắn nó ra
        skill2Prefabs.SetActive(true);

        // Move cục sét tới vị trí bắn
        skill2Prefabs.transform.position = skill2Pos.position;
        skill2Prefabs.transform.rotation = skill2Pos.rotation;

        Rigidbody rb = skill2Prefabs.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reset physics trước
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
         

      //bắn
            Vector3 direction = (player.position - skill2Pos.position).normalized;
            Debug.Log($"Lightning direction: {direction}");
            rb.linearVelocity = direction * 10f;
        }

        // Tắt cục sét sau 6 giây
        StartCoroutine(DeactivateSkill2After(7.5f));
    }

    private IEnumerator DeactivateSkill2After(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (skill2Prefabs != null)
        {
            skill2Prefabs.SetActive(false);

            // Reset lại gravity cho lần sau
            Rigidbody rb = skill2Prefabs.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.useGravity = true;  // Bật lại gravity
            }
        }
        isUsingSkill = false;
    }

    public void Skill3()
    {
        skill3Prefabs.SetActive(true);
        StartCoroutine(CooldownSkill3(2));
    }
    IEnumerator CooldownSkill3(float duration)
    {
        yield return new WaitForSeconds(duration);
        skill3Prefabs.SetActive(false);
        isUsingSkill = false;

    }
    public void Skill4()
    {
        skill4Prefabs.SetActive(true);
        GameObject obj = Instantiate(skill4Prefabs,playerPos, Quaternion.identity);
        Destroy(obj,4.5f);

        isUsingSkill = false;


    }

}
