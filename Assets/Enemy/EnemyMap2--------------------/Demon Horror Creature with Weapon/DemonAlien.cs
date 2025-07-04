using Pathfinding;
using System.Collections;
using UnityEngine;


public class DemonAlien : MonoBehaviour
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
    public Transform player;   
    public float stopRange = 2f; // Khoảng cách dừng lại khi đến gần người chơi
    public float detectionRange = 100f; // Khoảng cách phát hiện người chơi

    public float coolDownAttack = 10f; // Thời gian hồi chiêu tấn công
    public float lastAttackTime = -10f; // Thời gian tấn công cuối cùng
    public bool isAttack = false;   
    public float stepAttack = 0;

    //tham chieu 
    public Animator animator;
    public AIPath aiPath;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        aiPath = GetComponent<AIPath>();
      
    }

    
    void Update()
    {
        FlipToPlayer();
        TargetPlayer();
        Attack();

    }
   
   public void TargetPlayer()
    {
        if (!isAttack) return;
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= detectionRange && dist > stopRange)
        {
            aiPath.destination = player.position;// di chuyen thang toi player
            if (dist > 30) // Khoảng cách xa hơn 30 thì chạy
            {
               
                aiPath.maxSpeed = 15f; // Tốc độ chạy

                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);

            }
            else
            {
               
              
                   
                    aiPath.maxSpeed = 10f; // Tốc độ walk
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isWalking", true);
              
            }
        }
        else
        {
            aiPath.destination = transform.position; // Dừng lại nếu không có player
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }
    }
  

    public void Attack()
    {
       
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= stopRange && Time.time >= lastAttackTime + coolDownAttack && isAttack)
        {
           
            if (stepAttack == 0)
            {
                stepAttack = 1;
                animator.SetTrigger("punch1"); // Kích hoạt animation tấn công1
            }
            else if (stepAttack == 1)
            {
                stepAttack = 2;
                animator.SetTrigger("punch2"); // Kích hoạt animation tấn công1
            }
            else if (stepAttack == 2)
            {
                stepAttack = 0;
                animator.SetTrigger("punch3"); // Kích hoạt animation tấn công1
            }
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
            StartCoroutine(WaitAttack());
        }
       
    }
  
    public void FlipToPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; // Đặt y về 0 để chỉ xoay trên mặt phẳng ngang
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
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
    IEnumerator WaitAttack()
    {
        isAttack = false;
        yield return new WaitForSeconds(4f);
        isAttack = true;
    }
}
