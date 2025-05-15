using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;
    public float attackRange = 3f;
    public bool isDead;

    [Header("FSM States")]
    public BaseState currentState;
    public IdleState idleState;
    public ChaseState chaseState;
    public AttackState attackState;
    public GetHitState getHitState;
    public DeathState deathState;
    public PhaseChangeState phase2State;
    public IdleCombatState idleCombatState;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        idleState = new IdleState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        getHitState = new GetHitState(this);
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
}