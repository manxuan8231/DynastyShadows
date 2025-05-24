using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class KnightD : MonoBehaviour
{
    public float detectionRange = 15f; // Khoảng cách thấy enemy
    public float attackRange = 2f;     // Khoảng cách tấn công
    public float attackCooldown = 1.5f;

    private Transform targetEnemy;
    private float lastAttackTime = 0f;

    public Animator animator;
    public NavMeshAgent agent;

    public float killEnemy = 0f;

    private bool isKill = true;
    //tham chieu
    EnemyHP2 enemyHP2;
    TurnInQuest2 turnInQuest2;
    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHP2 = FindAnyObjectByType<EnemyHP2>();
        turnInQuest2 = FindAnyObjectByType<TurnInQuest2>();
       
    }

    void Update()
    {
        FindClosestEnemy();

        if (targetEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, targetEnemy.position);

            if (distance <= attackRange)
            {
                StopMoving();
                AttackEnemy();
            }
            else
            {
                MoveToTarget();
            }
        }
        else
        {
            StopMoving();
        }

        if (killEnemy >= 4 && isKill == true) 
        {
          
            turnInQuest2.isButtonF = true; // Đặt trạng thái hội thoại là true
            turnInQuest2.isContent = true; // Đặt trạng thái hội thoại là true
            isKill = false; // Đặt lại trạng thái skill
        }
    }

    // Tìm enemy gần nhất trong khoảng detectionRange
    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        targetEnemy = enemies
            .Where(e => Vector3.Distance(transform.position, e.transform.position) <= detectionRange)
            .OrderBy(e => Vector3.Distance(transform.position, e.transform.position))
            .Select(e => e.transform)
            .FirstOrDefault();
    }

    void MoveToTarget()
    {
        if (targetEnemy != null)
        {
            agent.SetDestination(targetEnemy.position);
            animator.SetBool("isRunning", true);
        }
    }

    void StopMoving()
    {
        agent.ResetPath();
        animator.SetBool("isRunning", false);
    }

    void AttackEnemy()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack");
          
        }
    }

    public void UpdateKillCount(float amount)
    {
        killEnemy += amount;

    }
}
