using UnityEngine;
using UnityEngine.AI;

public class Enemy3 : EnemyBase
{
    public enum EnemyState
    {
        Idle,
        Run,
        Attack,
        GetHit,
        Death
    }
    public EnemyState currentState;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public Transform player;
    [SerializeField] public Animator animator;
    public string currentTrigger;

    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;


    //thời gian 
    public float attackCooldown = 5f;
    public float attackTimer = 0f;

    //goi ham
    EnemyHP3 enemyHP3;

    //box damage
    public BoxCollider damageBox;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHP3 = FindAnyObjectByType<EnemyHP3>();
        player = FindClosestPlayer();
        damageBox.enabled = false; // Tắt box dame khi bắt đầu
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
    }


    // Update is called once per frame
    void Update()
    {
        if (!hasFirstPos && gameObject.activeSelf)
        {
            firstPos = transform.position; // Lưu vị trí ban đầu
            hasFirstPos = true; // Đánh dấu đã lưu vị trí ban đầu
        }
        player = FindClosestPlayer(); // Tìm player gần nhất
        switch (currentState)
        {
            case EnemyState.Idle:
                float distToPlayer = Vector3.Distance(transform.position, player.position);
                if (distToPlayer < radius)
                {
                    ChangeState(EnemyState.Run);
                }
                break;
            case EnemyState.Run:
                float dist = Vector3.Distance(transform.position, player.position);
                if (dist <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                {
                    Run();
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

        if (distToPlayer < radius )
        {
         if ( agent.enabled)
            agent.SetDestination(player.position); // Đuổi theo player
        }
        else
        {
            // Nếu player ra khỏi phạm vi, quay lại chỗ cũ
            float backDist = Vector3.Distance(transform.position, firstPos);
            agent.SetDestination(firstPos);

            if (backDist < 0.2f)
            {
                ChangeState(EnemyState.Idle); // Về tới nơi thì Idle lại
            }
           
        }
    }

    void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            // Thực hiện tấn công
            Debug.Log("Attack");
            animator.SetTrigger("Attack");
            attackTimer = 0f; // Reset thời gian tấn công
        }
        if(agent.enabled)
        agent.isStopped = true; // Dừng lại khi tấn công
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
            if (agent.enabled)
            {
                agent.isStopped = false;
            }
            attackTimer = 0f; //nếu như player đi xa
            ChangeState(EnemyState.Run);
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
            case EnemyState.Run:
               
                    animator.SetTrigger("Run");
                    currentTrigger = "Run";
                
                break;
            case EnemyState.Attack:
                animator.SetTrigger("Attack");
                currentTrigger = "Attack";
                break;
            case EnemyState.GetHit:
            
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;
            case EnemyState.Death:
                animator.SetTrigger("Death");
                currentTrigger = "Death";
                break;
        }
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Death");
    }

    public void EnableDamageBox()
    {
        damageBox.enabled = true; // Bật box dame khi tấn công
    }
    public void DisableDamageBox()
    {
        damageBox.enabled = false; // Tắt box dame khi không tấn công
    }
    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject go in players)
        {
            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = go.transform;
            }
        }

        return closest;
    }

    public override void ResetEnemy()
    {
        // 👇 Toàn bộ logic reset như trong EnemyHP4.ResetEnemy() hiện tại
        currentState = EnemyState.Idle;

        if (animator != null)
        {
            animator.Rebind();
            animator.Update(0f);
        }

        if (agent != null)
        {
            agent.ResetPath();
            agent.enabled = true;
        }

        // Có thể reset thêm gì đó nếu cần (disable box, reset timer,...)
    }
}


