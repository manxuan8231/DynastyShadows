using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class BossScript : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;

    public float detectionRange = 15f;
    public float attackRange = 2.5f;
    public float attackCooldown = 2f;

    public List<string> phase1Attacks;
    public List<string> phase2Attacks;

    public bool isPhase2 = false;
    public bool isDead = false;

    private BaseState currentState;

    // States
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;
    public GetHitState getHitState;
    public PhaseChangeState phaseChangeState;
    public DeathState deathState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Init FSM
        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        getHitState = new GetHitState(this);
        phaseChangeState = new PhaseChangeState(this);
        deathState = new DeathState(this);

        TransitionToState(idleState);
    }

    void Update()
    {
        currentState?.UpdateState();
    }

    public void TransitionToState(BaseState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }

    public string GetRandomAttack()
    {
        var attacks = isPhase2 ? phase2Attacks : phase1Attacks;
        return attacks[Random.Range(0, attacks.Count)];
    }
}