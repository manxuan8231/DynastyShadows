using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
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
    [SerializeField] public string currentTrigger;
    public Vector3 firstPos;
    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;


    //thời gian 
    public float attackCooldown = 5f;
    private float attackTimer = 0f;

  

    //box dame
    public BoxCollider damageBox;

    //goi ham
    EnemyHP enemyHP;
    PlayerControllerState playerControllerState;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        enemyHP = FindAnyObjectByType<EnemyHP>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu

    }
    void Start()
    {
        damageBox.enabled = false; // Tắt box dame khi bắt đầu   
        firstPos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {

      
        player = FindClosestPlayer(); 

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
                if (dist <= attackRange )
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
    public void Run()
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

   public void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            // Thực hiện tấn công
            Debug.Log("Attack");
            animator.SetTrigger("Attack");
            attackTimer = 0f; // Reset thời gian tấn công
            agent.isStopped = true; // Dừng lại khi tấn công

        }
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist > attackRange + 1f)
        {
            agent.isStopped = false;
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
                if (agent.enabled == true)
                {
                    agent.isStopped = true; // Dừng lại khi tấn công 
                }
                StartCoroutine(PrepareThenAttack()); // Gọi coroutine để chuẩn bị tấn công
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

    public IEnumerator PrepareThenAttack()//goi khi enemy chuan bi tan cong
    {
        playerControllerState.isEnemyPreparingAttack = true;
        yield return new WaitForSeconds(1f);
        playerControllerState.isEnemyPreparingAttack = false;

    }
}
