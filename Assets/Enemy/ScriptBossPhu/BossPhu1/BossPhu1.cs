using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Walk,
    Attack1,
    Attack2,
    GetHit,
    Death,
    BackToPos
}

public class BossPhu1 : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public Vector3 firstPos;
    public Animator animator;
    public EnemyState currentState;
    public string currentTrigger;

    public float radius = 25f;
    public float attackRange = 4f;

    public float attackCooldown = 5f;
    public float attackTimer = 0f;
    public float SkillCooldown = 20f;
    public float SkillTimer = 0f;
    public bool hasTakenDamage = false;

   
    BossPhu1HP BossPhu1HP;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        firstPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (currentState == EnemyState.Death) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Gọi logic điều khiển tấn công
        AttackController(distanceToPlayer);

        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle(distanceToPlayer);
                break;

            case EnemyState.Walk:
                if (distanceToPlayer <= attackRange)
                {
                    Attack();
                }
                else
                {
                    HandleWalk(distanceToPlayer);
                }
                break;

            case EnemyState.BackToPos:
                HandleBackToPos();
                break;

            case EnemyState.Attack1:
            case EnemyState.Attack2:
                FacePlayer();
                break;

            case EnemyState.GetHit:
                // Có thể chờ hoạt ảnh kết thúc nếu cần
                break;
        }
    }

    void AttackController(float distance)
    {
        if (distance <= attackRange)
        {
            agent.isStopped = true;
            Attack(); // Xử lý logic tấn công
        }
        else if (distance <= radius)
        {
            ChangeState(EnemyState.Walk);
        }
        else
        {
            if (hasTakenDamage)
                ChangeState(EnemyState.BackToPos);
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    void Attack()
    {
        SkillTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            if (SkillTimer >= SkillCooldown)
            {
                SkillTimer = 0f;
                ChangeState(EnemyState.Attack2); // Skill đặc biệt
            }
            else
            {
                ChangeState(EnemyState.Attack1); // Đánh thường
            }
            attackTimer = 0f;
        }

        // Nếu player chạy ra xa thì đuổi theo
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
            agent.isStopped = false;
            ChangeState(EnemyState.Walk);
        }
    }

    void HandleIdle(float distance)
    {
        if (distance < radius)
        {
            ChangeState(EnemyState.Walk);
        }
    }

    void HandleWalk(float distance)
    {
        if (distance >= radius)
        {
            if (hasTakenDamage)
                ChangeState(EnemyState.BackToPos);
            else
                agent.SetDestination(firstPos);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void HandleBackToPos()
    {
        float distToPos = Vector3.Distance(transform.position, firstPos);

        agent.isStopped = false;
        agent.SetDestination(firstPos);

        if (distToPos < 0.5f)
        {
            hasTakenDamage = false;
            ChangeState(EnemyState.Idle);
        }
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        ResetAllTriggers();

        switch (newState)
        {
            case EnemyState.Idle:
                animator.SetTrigger("Idle");
                currentTrigger = "Idle";
                break;

            case EnemyState.Walk:
                animator.SetTrigger("Walk");
                currentTrigger = "Walk";
                break;

            case EnemyState.Attack1:
                agent.stoppingDistance = 1.5f;
                agent.isStopped = true;
                animator.SetTrigger("Attack1");
                currentTrigger = "Attack1";
                break;

            case EnemyState.Attack2:
                agent.stoppingDistance = 4f;
                agent.isStopped = true;
                animator.SetTrigger("Attack2");
                currentTrigger = "Attack2";
                break;

            case EnemyState.GetHit:
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;

            case EnemyState.Death:
                agent.isStopped = true;
                animator.SetTrigger("Death");
                currentTrigger = "Death";
                break;

            case EnemyState.BackToPos:
                agent.isStopped = false;
                animator.SetTrigger("BackToPos");
                currentTrigger = "BackToPos";
                break;
        }
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Death");
        animator.ResetTrigger("BackToPos");
    }

}