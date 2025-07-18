using Pathfinding;
using System.Collections;
using UnityEngine;

public class EnemyMap2_1 : MonoBehaviour
{
   public enum EnemyState
    {
        Idle,
        Run,
        Attack,
        GetHit,
        Skill1,
        Death
    }
    public EnemyState currentState;

    public AIPath ai;
    [SerializeField] public Transform player;
    [SerializeField] public Vector3 firstPos;
    [SerializeField] public Animator animator;
    [SerializeField] public string currentTrigger;
    [SerializeField] float endDistance = 0.2f;
    public bool hasFirstPos = false;
    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;

    //thời gian 
    public float attackCooldown = 5f;
    private float attackTimer = 0f;


  

    //box dame
    public BoxCollider damageBox;
    public DameZoneWeapon DameZoneWeapon;

    //goi ham
    EnemyHP enemyHP;
    PlayerControllerState playerControllerState;
    private void Awake()
    {
        ai = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        enemyHP = FindAnyObjectByType<EnemyHP>();
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
        DameZoneWeapon = FindAnyObjectByType<DameZoneWeapon>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();

    }
    void Start()
    {
        damageBox.enabled = false; // Tắt box dame khi bắt đầu   
        firstPos = transform.position; 
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
 void Run()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer < radius)
        {
            // Nếu player trong phạm vi, chạy về phía player
            ai.canMove = true; // Bật di chuyển
            ai.canSearch = true; // Bật tìm kiếm
            ai.destination = player.position;
            ChangeState(EnemyState.Run);
           
        }
        else
        {
            // Nếu player ra khỏi phạm vi, quay lại chỗ cũ
            float backDist = Vector3.Distance(transform.position, firstPos);
            ai.canMove = true; // Bật di chuyển
            ai.canSearch = true;
            ai.destination = firstPos;
            ai.endReachedDistance = endDistance; // Khoảng cách kết thúc

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
            ai.endReachedDistance = attackRange;
            // Thực hiện tấn công
            Debug.Log("Attack");
            animator.SetTrigger("Attack");
            attackTimer = 0f; // Reset thời gian tấn công

        }
        float dist = Vector3.Distance(transform.position, player.position);
        if(dist > attackRange + 1f)
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
                ai.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = false; // Tắt tìm kiếm khi ở trạng thái Idle
                break;
            case EnemyState.Run:
               
                animator.SetTrigger("Run");
                currentTrigger = "Run";
                ai.canMove = true; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = true; // Tắt tìm kiếm khi ở trạng thái Idle
                break;
            case EnemyState.Attack:
              StartCoroutine(PrepareThenAttack()); // Gọi coroutine để chuẩn bị tấn công
                animator.SetTrigger("Attack");
                currentTrigger = "Attack";
                ai.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = false; // Tắt tìm kiếm khi ở trạng thái Idle

                break;
            case EnemyState.Skill1:
                animator.SetTrigger("Skill1");
                currentTrigger = "Skill1";
                ai.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = false; // Tắt tìm kiếm khi ở trạng thái Idle
                break;
            case EnemyState.GetHit:
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                ai.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = false; // Tắt tìm kiếm khi ở trạng thái Idle
                break;
            case EnemyState.Death:
                animator.SetTrigger("Death");
                currentTrigger = "Death";
                ai.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                ai.canSearch = false; // Tắt tìm kiếm khi ở trạng thái Idle

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
        animator.ResetTrigger("Skill1");
    }
    public void EnableDamageBox()
    {
        DameZoneWeapon.beginDame(); // Bật box dame khi tấn công
    }
    public void DisableDamageBox()
    {
        DameZoneWeapon.endDame(); // Tắt box dame sau khi tấn công xong
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
