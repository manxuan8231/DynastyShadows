using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;
    public float attackRange = 3f;
    public bool isDead;
    public bool isPhase2;
    [Header("FSM States")]
    public BaseState currentState;
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;
    public DeathState deathState;
    public PhaseChangeState phase2State;
    public IdleCombatState idleCombatState;
    public BossAnimationData animationData;
    public List<GameObject> attackCollider;
    public BoxCollider leftDame;
    public BoxCollider rightDame;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        deathState = new DeathState(this);
        phase2State = new PhaseChangeState(this);
        idleCombatState = new IdleCombatState(this);
    }

    void Start()
    {
        TransitionToState(idleState);
    }

    void Update()
    {
        currentState?.UpdateState();
    }

    public void TransitionToState(BaseState newState)
    {
        if (currentState == newState) return;

        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }
    public void OnBoxDameLeft()
    {
        leftDame.enabled = true;
    }
    public void OffBoxDameLeft()
    {
        leftDame.enabled = false;
    }
    public void OnBoxDameRight()
    {
        rightDame.enabled = true;
    }
    public void OffBoxDameRight()
    {
        rightDame.enabled = false;
    }
    // Gọi từ animation event hoặc FSM
    public void EnableAttackColliders()
    {
        foreach (var col in attackCollider)
            col.SetActive(true);
    }

    public void DisableAttackColliders()
    {
        foreach (var col in attackCollider)
            col.SetActive(false);
    }

}