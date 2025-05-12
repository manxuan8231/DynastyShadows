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


    //thời gian cooldown
    public float attackCooldown = 5f;
    public float attackTimer = 0f;
    public float SkillCooldown = 15f;
    public float SkillTimer = 0f;
    public bool hasTakenDamage = false;

    bool utlAttack;
    bool isAttacking;
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
      

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
       
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdle(distanceToPlayer);
                break;
            case EnemyState.Walk:
              if(distanceToPlayer <= attackRange)
                {
                    Attack();
                }
                else
                    HandleWalk(distanceToPlayer);
                break;
            case EnemyState.BackToPos:
                HandleBackToPos();

                break;
            case EnemyState.Attack1:
            case EnemyState.Attack2:  
                break;
        }
    }

    void Attack()
    {
        SkillTimer += Time.deltaTime; //trigger attack 1
        attackTimer += Time.deltaTime;//trigger attack 2
        if (attackTimer >= attackCooldown)
        {

            if (SkillTimer >= SkillCooldown)
            {
                SkillTimer = 0;
                ChangeState(EnemyState.Attack2);
            }
            else
            {
               
               ChangeState(EnemyState.Attack1 );
            }
            attackTimer = 0;
        }


        float dist = Vector3.Distance(transform.position, player.position);
        if(dist > attackRange + 1f)
        {
            agent.isStopped = false;
            attackTimer = 0f; //nếu như player đi xa
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
            {
                ChangeState(EnemyState.BackToPos);
            }
            else
            {
                ChangeState(EnemyState.Walk);
                agent.SetDestination(firstPos);
            }
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
        if (currentState == newState) return; // Tránh spam trigger nếu không đổi trạng thái

        currentState = newState;
        ResetAllTriggers(); // Reset hết trigger cũ trước khi set cái mới

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
                agent.stoppingDistance = 3;
                agent.isStopped = true; // Dừng lại khi tấn công
                animator.SetTrigger("Attack1");
                currentTrigger = "Attack1";
                break;
            case EnemyState.Attack2:
                agent.stoppingDistance = 3;
                agent.isStopped = true; // Dừng lại khi tấn công
                animator.SetTrigger("Attack2");
                currentTrigger = "Attack2";
                break;
            case EnemyState.GetHit:


                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;
            case EnemyState.Death:
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
