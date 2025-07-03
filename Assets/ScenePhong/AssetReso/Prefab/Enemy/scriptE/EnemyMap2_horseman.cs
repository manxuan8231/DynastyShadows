using System.Collections;
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
        Die
    }
    public EnemyState currentState;

    [SerializeField] public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] public Transform player;
    [SerializeField] public Vector3 firstPos;
    [SerializeField] public Animator animator;
    [SerializeField] public string currentTrigger;

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
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
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
            StartCoroutine(Waitingforflip());
            attackTimer = 0f; // Reset thời gian tấn công
            agent.isStopped = true; // Dừng lại khi tấn công

        }
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > attackRange + 1f)
        {
            agent.isStopped = false;
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
                break;
            case EnemyState.Walk:

                animator.SetTrigger("Walk");
                currentTrigger = "Walk";
                break;
            case EnemyState.Attack:
                if (agent.enabled == false) return; // Nếu agent không hoạt động thì không tấn công
                agent.isStopped = true; // Dừng lại khi tấn công 
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
        yield return new WaitForSeconds(0.05f);
        animator.SetTrigger("Attack");
    }
}
