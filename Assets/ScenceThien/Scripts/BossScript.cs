using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BossScript : MonoBehaviour
{
    [SerializeField] public Transform player;
    [FormerlySerializedAs("Anim")] public Animator anim;
    public float attCooldown = 2.5f;
    public float distance = 20f;
    public float attackRange = 2f;
    [SerializeField] public NavMeshAgent agent;
    public float timeAttacking = 0f;

    BossHP bossHP;

    public bool isPhaseTwo = false;
    public bool isDead = false;

    public enum EnemyState
    {
        Idle,
        Run,
        Attack,
        GetHit,
        Death
    }
    public EnemyState currentState;

    private List<string> attackTriggersPhase1 = new List<string> { "Attack 1", "Attack 2" };
    private List<string> attackTriggersPhase2 = new List<string> { "Attack 3", "Attack 4", "Attack 5" };

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
        bossHP = GetComponent<BossHP>();
        currentState = EnemyState.Idle;
    }

    void Update()
    {

        if (!isPhaseTwo && bossHP.currentHealth <= bossHP.maxHealth * 0.5f)
        {
            EnterPhaseTwo();
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                if (Vector3.Distance(transform.position, player.position) < distance)
                {
                    ChangeState(EnemyState.Run);
                }
                break;

            case EnemyState.Run:
                Run();
                if (Vector3.Distance(transform.position, player.position) <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Run()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < distance)
        {
            agent.SetDestination(player.position);
            transform.LookAt(player);
        }
    }

    public void Attack()
    {
        timeAttacking += Time.deltaTime;

        if (timeAttacking >= attCooldown)
        {
            string selectedAttack = isPhaseTwo ?
                attackTriggersPhase2[Random.Range(0, attackTriggersPhase2.Count)] :
                attackTriggersPhase1[Random.Range(0, attackTriggersPhase1.Count)];

            anim.SetTrigger(selectedAttack);
            timeAttacking = 0f;
        }

        agent.isStopped = true;

        if (Vector3.Distance(transform.position, player.position) > attackRange + 1f)
        {
            agent.isStopped = false;
            ChangeState(EnemyState.Run);
        }
    }

    void EnterPhaseTwo()
    {
        isPhaseTwo = true;
    }

    void Die()
    {
        isDead = true;
        ChangeState(EnemyState.Death);
        agent.isStopped = true;
        Debug.Log("Boss has died.");
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState || isDead) return;

        currentState = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                anim.SetTrigger("Idle");
                break;
            case EnemyState.Run:
                anim.SetTrigger("Run");
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                timeAttacking = attCooldown;
                break;
            case EnemyState.Death:
                anim.SetTrigger("Death");
                break;
        }
    }
}
