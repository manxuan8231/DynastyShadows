using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DragonRed : MonoBehaviour
{
    public GameObject player;
    public float detectionRangePL = 100f;
    public float stopDistance = 14f;
    public float moveSpeed = 3.5f;

    private bool isMove = true;
    private bool isFlipAllowed = true;
    private bool isAttack = true;
    // Cooldown tấn công
    public float attackCooldown = 2f;
    public float lastAttackTime = -2f;
    public int stepAttack = 0;
   
    // Tham chiếu
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    private DragonRedHP dragonRedHP;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        dragonRedHP = FindAnyObjectByType<DragonRedHP>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        MoveToPlayer();
        Attack();
        FlyAndDashBack();
    }
    //walk den player
    public void MoveToPlayer()
    {
        if (dragonRedHP.isStunned) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRangePL && distanceToPlayer > stopDistance && isMove)
        {
            navMeshAgent.SetDestination(player.transform.position);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            navMeshAgent.ResetPath();
            animator.SetBool("IsWalking", false);
        }
    }
    //tan cong 
    public void Attack()
    {
        if (dragonRedHP.isStunned) return;
        if(isAttack == false) return;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // Nếu đang trong cooldown và được phép flip thì xoay mặt về phía player
        if (Time.time < lastAttackTime + attackCooldown && isFlipAllowed)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            return;
        }

        if (distanceToPlayer <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            isMove = false;
            isFlipAllowed = false;
            StartCoroutine(WaitToFlipBack()); // Sau khi đánh xong thì mới cho xoay lại sau 4 giây
            dragonRedHP.strugglePoint++;// Tăng điểm vật lộn khi tấn công
            // Combo attack logic
            if (stepAttack == 0)
            {
                stepAttack = 1;
                animator.SetTrigger("BasicAttack");
                
            }
            else if (stepAttack == 1)
            {
                stepAttack = 2;
                animator.SetTrigger("AttackHand");
            }
            else if (stepAttack == 2)
            {
                stepAttack = 0;
                animator.SetTrigger("AttackFlame");
            }

            lastAttackTime = Time.time;
        }

        // Nếu người chơi ra khỏi phạm vi thì reset combo và đợi mới được di chuyển lại
        if (distanceToPlayer > stopDistance)
        {
            StartCoroutine(WaitMoveToPlayer());
           
        }
    }
    //fly rồi lùi ra sau
    public void FlyAndDashBack()
    {
        if (dragonRedHP.isStunned) return;

        if (dragonRedHP.strugglePoint >= 5)
        {
            isAttack = false; // Ngừng tấn công
            animator.SetBool("Fly",true); // Chạy animation bay lên
            isMove = false;
            isFlipAllowed = false;
            StartCoroutine(FlyUpThenRetreat());
        }
    }

    public IEnumerator WaitToFlipBack()
    {
        yield return new WaitForSeconds(3f);
        isFlipAllowed = true;
    }

    public IEnumerator WaitMoveToPlayer()
    {
        yield return new WaitForSeconds(4f);
        isMove = true;
    }
    private IEnumerator FlyUpThenRetreat()
    {
        animator.SetBool("Fly", false);
        navMeshAgent.enabled = false;

        Vector3 startPos = transform.position;
        Vector3 flyUpPos = startPos + Vector3.up * 5f;

        float duration = 1f;
        float time = 0f;

        // Bay lên
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, flyUpPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = flyUpPos;

        yield return new WaitForSeconds(2f); // Giữ trên không

        // Lùi về phía sau khi đang bay
        Vector3 retreatPos = flyUpPos - transform.forward * 10f;
        time = 0f;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(flyUpPos, retreatPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = retreatPos;

      
        animator.SetBool("FlyBack", true);

        // (Tuỳ chọn) Hạ xuống lại mặt đất sau khi animation FlyBack chạy
        yield return new WaitForSeconds(41f); // chờ animation FlyBack một chút
        animator.SetBool("FlyBack", false);
        Vector3 groundPos = new Vector3(retreatPos.x, startPos.y, retreatPos.z);
        time = 0f;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(retreatPos, groundPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = groundPos;

        // Bật lại NavMesh và di chuyển
        navMeshAgent.enabled = true;
        StartCoroutine(WaitToFlipBack());
    }



    public IEnumerator DashToTarget(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        navMeshAgent.enabled = false;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        navMeshAgent.enabled = true;
    }
}
