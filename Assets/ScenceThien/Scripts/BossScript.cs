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

    private List<string> attackAnimationsPhase1 = new List<string> { "attack1", "attack2" };
    private List<string> attackAnimationsPhase2 = new List<string> { "attack2_0", "attack2_1", "attack3" };

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
        if (isDead) return;

        if (bossHP.currentHealth <= 0)
        {
            Die();
            return;
        }

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
                attackAnimationsPhase2[Random.Range(0, attackAnimationsPhase2.Count)] :
                attackAnimationsPhase1[Random.Range(0, attackAnimationsPhase1.Count)];

            anim.Play(selectedAttack);
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
                anim.Play("idle1");
                break;
            case EnemyState.Run:
                anim.Play("run");
                break;
            case EnemyState.Attack:
                agent.isStopped = true;
                timeAttacking = attCooldown;
                break;
            case EnemyState.GetHit:
                anim.Play("gethit");
                break;
            case EnemyState.Death:
                anim.Play("death");
                break;
        }
    }
}
