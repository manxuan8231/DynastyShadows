using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DragonRed : MonoBehaviour
{
    public GameObject player;// người chơi
    public float detectionRangePL = 100f;//phat hien nguoi choi
    public float stopDistance = 14f; // Khoảng cách dừng di chuyển khi đến gần người chơi
    public float moveSpeed = 3.5f; // Tốc độ di chuyển của rồng đỏ
    //attackcooldown
    public float attackCooldown = 2f; // Thời gian chờ giữa các đợt tấn công
    public float lastAttackTime = -2f; // Thời gian của lần tấn công cuối cùng
   

    //tham chieu
    public NavMeshAgent navMeshAgent;
    public Animator animator;
    private DragonRedHP dragonRedHP; // Tham chiếu đến script DragonRedHP

    void Start()
    {
       
        navMeshAgent = GetComponent<NavMeshAgent>(); // Lấy NavMeshAgent từ đối tượng này
        animator = GetComponent<Animator>(); // Lấy Animator từ đối tượng này
        dragonRedHP = FindAnyObjectByType<DragonRedHP>();



        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Tìm người chơi trong cảnh        
        }
        
    }

    void Update()
    {
       
        MoveToPlayer();
        Attack();
    }


    
    // Di chuyển rồng đỏ về phía người chơi nếu trong phạm vi phát hiện
    public void MoveToPlayer()
    {
        if (dragonRedHP.isStunned) return; //Không di chuyển khi đang stun
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position); // Tính khoảng cách đến người chơi
        if (distanceToPlayer <= detectionRangePL && distanceToPlayer > stopDistance)
        {
            navMeshAgent.SetDestination(player.transform.position); // Di chuyển đến vị trí của người chơi
            animator.SetBool("IsWalking", true); // Kích hoạt hành động di chuyển trong Animator
        }
        else
        {
            navMeshAgent.ResetPath(); // Dừng di chuyển nếu không còn trong phạm vi phát hiện
            animator.SetBool("IsWalking", false); // Tắt hành động di chuyển trong Animator
        }
    }

    public void Attack()
    {
        if (dragonRedHP.isStunned) return; //Không di chuyển khi đang stun
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer <= stopDistance && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("AttackHand"); // Kích hoạt hành động tấn công trong Animator
         
            lastAttackTime = Time.time; // Cập nhật thời gian của lần tấn công cuối cùng
        }
    }

    //tinh vi tri dash toi
    public IEnumerator DashToTarget(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = transform.position;
        float time = 0f;
        while (time < duration)
        {
           
            navMeshAgent.enabled = false; // Tắt NavMeshAgent trong lúc dash
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }
        transform.position = targetPosition;
        navMeshAgent.enabled = true; // Bật lại NavMeshAgent
    }

   
}
