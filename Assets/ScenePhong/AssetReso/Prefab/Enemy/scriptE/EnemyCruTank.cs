using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCruTank : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Walk,
        Attack,
        GetHit,
        Skill1,
        Die,
        Dodge
    }
    public EnemyState currentState;

    public AIPath aiPath;
    public Transform player;
    public Vector3 firstPos;
    public Animator animator;
    public string currentTrigger;

    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;

    //thời gian 
    public float attackCooldown = 5f;
    private float attackTimer = 0f;


    //box dame
    public BoxCollider damageBox;
    public DameWeaponCruTank dameCruTank;

    //goi ham
    EnemyCruTankHP enemyCruTankHP;
    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        enemyCruTankHP = FindAnyObjectByType<EnemyCruTankHP>();
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
        dameCruTank = FindAnyObjectByType<DameWeaponCruTank>();

    }
    void Start()
    {
        damageBox.enabled = false; // Tắt box dame khi bắt đầu   
        firstPos = transform.position;
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
                    ChangeState(EnemyState.Walk);
                }
                break;

            case EnemyState.Walk:

                float dist = Vector3.Distance(transform.position, player.position);
                if (dist <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                {
                    Walk();
                }
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }
    void Walk()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer < radius)
        {
            aiPath.destination = player.position; // Đuổi theo player
        }
        else
        {
            // Nếu player ra khỏi phạm vi, quay lại chỗ cũ
            float backDist = Vector3.Distance(transform.position, firstPos);
            aiPath.destination = firstPos;

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
            StartCoroutine(Waitingforflip());
            attackTimer = 0f; // Reset thời gian tấn công
            aiPath.isStopped = true; // Dừng lại khi tấn công

        }
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
            aiPath.isStopped = false;
            attackTimer = 0f; //nếu như player đi xa
            ChangeState(EnemyState.Walk);
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
                aiPath.canMove = false; // Dừng di chuyển khi ở trạng thái Idle
                aiPath.canSearch = false; // Dừng tìm kiếm khi ở trạng thái Idle
                break;
            case EnemyState.Walk:

                animator.SetTrigger("Walk");
                currentTrigger = "Walk";
                aiPath.canMove = true; // Bật di chuyển khi ở trạng thái Walk
                aiPath.canSearch = true; // Bật tìm kiếm khi ở trạng thái Walk
                break;
            case EnemyState.Attack:
                if (aiPath.enabled == false) return; // Nếu agent không hoạt động thì không tấn công
                aiPath.isStopped = true; // Dừng lại khi tấn công 
                aiPath.canMove = false; // Dừng di chuyển khi ở trạng thái Attack
                aiPath.canSearch = false; // Dừng tìm kiếm khi ở trạng thái AttackAq
                animator.SetTrigger("Attack");
                currentTrigger = "Attack";
                break;        
            case EnemyState.GetHit:
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;
            case EnemyState.Die:
                animator.SetTrigger("Die");
                currentTrigger = "Die";
                break;
           

        }
    }

    void ResetAllTriggers()
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("GetHit");
        animator.ResetTrigger("Die");
        animator.ResetTrigger("Skill1");
        animator.ResetTrigger("Dodge");

    }
    public void BatDamageBox()
    {
        dameCruTank.beginDame(); // Bật box dame khi tấn công
    }
    public void TatDamageBox()
    {
        dameCruTank .endDame(); // Tắt box dame sau khi tấn công xong
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
    public IEnumerator Waitingforflip()
    {
        transform.LookAt(player);
        yield return new WaitForSeconds(0.02f);
        animator.SetTrigger("Attack");

    }
}
