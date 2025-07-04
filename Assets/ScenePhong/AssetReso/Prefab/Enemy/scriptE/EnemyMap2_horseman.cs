using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMap2_horseman : MonoBehaviour
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
    public Animator playerAnimator;


    public float dodgeDistance = 3f;
    public float dodgeChance = 0.5f; // 50% né đòn
    private bool isDodging = false;

    //khoảng cách
    public float radius = 20f;
    public float attackRange = 2f;

    //thời gian 
    public float attackCooldown = 5f;
    private float attackTimer = 0f;


    //box dame
    public BoxCollider damageBox;
    public DameWeaponHorseMan dameWeaponHorseMan;

    //goi ham
    EnemyHorseManHP enemyHorseManHP;
    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        player = FindClosestPlayer();
        enemyHorseManHP = FindAnyObjectByType<EnemyHorseManHP>();
        ChangeState(EnemyState.Idle); // Khởi tạo trạng thái ban đầu
        dameWeaponHorseMan = FindAnyObjectByType<DameWeaponHorseMan>();

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

    //------------------------------------------------------------------------
    //------------------------------------------------------------------------


    public void TryDodge()
    {
        if (isDodging || currentState == EnemyState.Die || currentState == EnemyState.Dodge)
            return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= dodgeDistance && Random.value < dodgeChance)
        {
            Vector3 dodgeDir = GetDodgeDirection();
            StartCoroutine(DoDodge(dodgeDir));
        }
    }

    Vector3 GetDodgeDirection()
    {
        return transform.right; // luôn né sang trái
    }

    IEnumerator DoDodge(Vector3 direction)
    {
        enemyHorseManHP.isTakedame = false;
        isDodging = true;
        aiPath.isStopped = true;

        ChangeState(EnemyState.Dodge); // Gọi animation né
        float distanceDodged = 0f;
        float dodgeSpeed = 17f; // tốc độ né
        float maxDodgeDistance = 4f; // khoảng cách né mong muốn
        Vector3 startPos = transform.position;

        // Né đến khi đi đủ khoảng cách mong muốn
        while (distanceDodged < maxDodgeDistance)
        {
            Vector3 move = direction * dodgeSpeed * Time.deltaTime;
            transform.position += move;
            distanceDodged = Vector3.Distance(startPos, transform.position);
            yield return null;
        }

        // Chờ thêm một chút cho animation né hoàn thành
        yield return new WaitForSeconds(0.5f); // thời gian tùy chỉnh theo clip

        enemyHorseManHP.isTakedame = true;
        isDodging = false;
        aiPath.isStopped = false;

        ChangeState(EnemyState.Walk);
    }



    //------------------------------------------------------------------------
    //------------------------------------------------------------------------

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
            case EnemyState.Attack:
                if (aiPath.enabled == false) return; // Nếu agent không hoạt động thì không tấn công
                aiPath.isStopped = true; // Dừng lại khi tấn công 
                animator.SetTrigger("Attack");
                currentTrigger = "Attack";
                break;
            case EnemyState.Skill1:
                animator.SetTrigger("Skill1");
                currentTrigger = "Skill1";
                break;
            case EnemyState.GetHit:
                animator.SetTrigger("GetHit");
                currentTrigger = "GetHit";
                break;
            case EnemyState.Die:
                animator.SetTrigger("Die");
                currentTrigger = "Die";
                break;
            case EnemyState.Dodge:
                animator.SetTrigger("Dodge"); // Tạo trigger "Dodge" trong Animator
                currentTrigger = "Dodge";
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
        dameWeaponHorseMan.beginDame(); // Bật box dame khi tấn công
    }
    public void TatDamageBox()
    {
        dameWeaponHorseMan.endDame(); // Tắt box dame sau khi tấn công xong
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
