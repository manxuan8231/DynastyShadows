using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DragonRed : MonoBehaviour
{
    public GameObject player;
    public float detectionRangePL = 100f;
    public float stopDistance = 14f;
    public float moveSpeed = 3.5f;

    public bool isMove = true;

    public bool isAttack = true;
    public bool isTurnColi = false;
    // Cooldown tấn công
    public float attackCooldown = 2f;
    public float lastAttackTime = -2f;
    public int stepAttack = 0;
    
    // Tham chiếu
    public CapsuleCollider conllider;
    public AIPath aiPath;
    public Animator animator;
    private DragonRedHP dragonRedHP;
    private DragonRedFly dragonRedFly;
    public AudioSource audioSource; // Thêm AudioSource để phát âm thanh
    public AudioClip biteSound; // Âm thanh cắn
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        dragonRedHP = FindAnyObjectByType<DragonRedHP>();
        dragonRedFly = FindAnyObjectByType<DragonRedFly>();
        audioSource = GetComponent<AudioSource>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        isTurnColi = false;
        isAttack = false;
    }

    void Update()
    {
        
        MoveToPlayer();
        Attack();
       
        
    }
    //walk den player
    public void MoveToPlayer()
    {
        if (isAttack) return;
        if (dragonRedHP.isStunned) return;
        if(dragonRedFly.isFly) return; // Nếu đang bay thì không di chuyển
        FlipToPlayer();
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRangePL && distanceToPlayer > stopDistance && isMove)
        {
            if (aiPath.enabled == false) return;
            aiPath.destination = player.transform.position;
            if(dragonRedHP.currentHp > 6000)
            {
                animator.SetBool("IsWalking", true);
            }
            else if(dragonRedHP.currentHp <= 6000)
            {
                animator.SetBool("IsRunning", true);
            }
                
        }
        else
        {
            if(aiPath.enabled == false) return;
           
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }
    //tan cong  
    public void Attack()
    {
        if (dragonRedHP.isStunned) return;
       
        if (dragonRedFly.isFly) return; // Nếu đang bay thì không di chuyển
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetBool("IsWalking", false);
            isMove = false;
            StartCoroutine(WaitForAttack()); //dung de true false isattack
            // Combo attack logic
            if (stepAttack == 0)
            {

                stepAttack = 1;
                audioSource.PlayOneShot(biteSound); // Phát âm thanh cắns
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

        // Nếu người chơi ra khỏi phạm vi thì doi 2f ms cho di chuyen
        if (distanceToPlayer > stopDistance)
        {
            StartCoroutine(WaitMoveToPlayer());
           
        }
    }
    
    public void FlipToPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; // Đặt y về 0 để chỉ xoay trên mặt phẳng ngang
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
      
       
    }
    public IEnumerator WaitForAttack()
    {
        isAttack = true;
        yield return new WaitForSeconds(4f);
        isAttack = false;
      
    }
    public IEnumerator WaitMoveToPlayer()
    {
        yield return new WaitForSeconds(3f);
        isMove = true;
       
    }
   

   
}
