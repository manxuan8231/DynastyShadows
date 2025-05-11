using UnityEngine;
using UnityEngine.AI;

public class Enemy3 : MonoBehaviour
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
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    public Vector3 firstPos;
    public Animator animator;
    private string currentTrigger;

    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;


    //thời gian 
    public float attackCooldown = 5f;
    public float attackTimer = 0f;

    //máu của quái vật
    public float maxHealth;
    private float currentHealth;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        firstPos = transform.position;
        currentHealth = maxHealth; // Khởi tạo máu
        player = GameObject.FindGameObjectWithTag("Player").transform;
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20f);
        }
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

        if (distToPlayer < radius)
        {
         
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
        agent.isStopped = true; // Dừng lại khi tấn công
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
            agent.isStopped = false;
            attackTimer = 0f; //nếu như player đi xa
            ChangeState(EnemyState.Run);
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentState == EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;

        if (currentHealth > 0)
        {
            ChangeState(EnemyState.GetHit);

            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.5f);
        }
        else
        {
            currentHealth = 0;
            ChangeState(EnemyState.Death);
            agent.isStopped = true;

            // Hủy enemy sau 1.5 giây để animation kịp phát xong
            Destroy(gameObject, 3f);
        }
    }
    void BackToChase()
    {
        if (currentState != EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange)
            {
                ChangeState(EnemyState.Attack);
            }
            else
            {
                ChangeState(EnemyState.Run);
            }
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
}
