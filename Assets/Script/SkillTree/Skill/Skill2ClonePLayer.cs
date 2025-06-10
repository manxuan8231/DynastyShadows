using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Skill3ClonePLayer : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public LayerMask enemyLayer;
    public float speed = 5f;
    public float detectionRange = 50f; 
    public float attackRange = 4f; 
    public float attackCooldown = 15f;
   
    private Transform nearestEnemy;
    private float lastAttackTime = -15f;
    Skill3Manager skill3Manager;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        StartCoroutine(ReturnToPool());
    }

    void Update()
    {
        FindNearestEnemy();
        
        if (nearestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.position);
            
            if (distanceToEnemy <= attackRange)
            {
              
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackEnemy();
                    lastAttackTime = Time.time;
                }

            }
            else
            {
                
                MoveToEnemy();
            }
        }else{
            animator.SetBool("Run", false);
        }
      
    }

    void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        float closestDistance = Mathf.Infinity;
        nearestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = collider.transform;
            }
        }
    }

    public void MoveToEnemy()
    {
        if (nearestEnemy != null)
        { 
            animator.SetBool("Run", true);
            agent.SetDestination(nearestEnemy.position);
        }
    }

    public void AttackEnemy()
    {
        if (nearestEnemy != null)
        {
            // xoay mat 
            transform.LookAt(nearestEnemy);
            
            // Trigger attack animation
            animator.SetTrigger("Attack");
            animator.SetBool("Run", false);
         
           
        }
    }
   public  IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(skill3Manager.timeSkill3);
        ObjPoolingManager.Instance.ReturnToPool(skill3Manager.clonePLTag,gameObject);
    }
}
