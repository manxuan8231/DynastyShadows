using Pathfinding;
using UnityEngine;

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
    public AIPath aiPath; // Sử dụng AIPath để di chuyển
    [SerializeField] public Transform player;
    public Vector3 firstPos;
    [SerializeField] public Animator animator;
    public string currentTrigger;
    public float distanceFirstPos = 0.2f;
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
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        firstPos = transform.position;
        enemyHP3 = FindAnyObjectByType<EnemyHP3>();
        player = FindClosestPlayer();
        damageBox.enabled = false; // Tắt box dame khi bắt đầu
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
    }                                                             

    // Update is called once per frame
    void Update()
    {
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
            aiPath.destination = player.position; // Chạy về phía player
            aiPath.canSearch = true; // Bật tìm đường
            aiPath.canMove = true; // Bật di chuyển
            aiPath.endReachedDistance = attackRange; // Khoảng cách kết thúc đường đi
            if (distToPlayer <= attackRange)
            {
                ChangeState(EnemyState.Attack); // Nếu tới gần đủ thì chuyển sang tấn công
            }
            else
            {
                ChangeState(EnemyState.Run); // Nếu chưa tới thì vẫn chạy
            }

        }
        else
        {
            // Nếu player ra khỏi phạm vi, quay lại chỗ cũ
            float backDist = Vector3.Distance(transform.position, firstPos);
            aiPath.destination = firstPos; // Quay về vị trí ban đầu
            aiPath.canSearch = true; // Bật tìm đường
            aiPath.canMove = true; // Bật di chuyển
            aiPath.endReachedDistance = distanceFirstPos; // Khoảng cách kết thúc đường đi về vị trí ban đầu
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
        
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
           
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
                aiPath.canMove = false; // ❌ DỪNG DI CHUYỂN
                aiPath.canSearch = false; // ❌ DỪNG TÌM ĐƯỜNG
                break;
            case EnemyState.Run:
                animator.SetTrigger("Run");
                currentTrigger = "Run";
                aiPath.canMove = true; // ✅ BẬT DI CHUYỂN
                aiPath.canSearch = true; // ✅ BẬT TÌM ĐƯỜNG
                break;
            case EnemyState.Attack:
                animator.SetTrigger("Attack");
                currentTrigger = "Attack";
                aiPath.canMove = false; // ❌ DỪNG DI CHUYỂN
                aiPath.canSearch = false; // ❌ DỪNG TÌM ĐƯỜNG
                break;
            case EnemyState.GetHit:
            
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;
            case EnemyState.Death:
                animator.SetTrigger("Death");
                currentTrigger = "Death";
                aiPath.canMove = false; // ❌ DỪNG DI CHUYỂN
                aiPath.canSearch = false; // ❌ DỪNG TÌM ĐƯỜNG
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
}


