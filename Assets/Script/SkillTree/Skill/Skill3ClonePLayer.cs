using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Skill3ClonePLayer : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public AIPath aiPath;
    public Animator animator;
    public LayerMask enemyLayer;
   
    public float speed = 5f;
    public float detectionRange = 50f; 
    public float attackRange = 4f; 
    public float attackCooldown = 15f;

    //skin                         
    public SkinnedMeshRenderer[] skinnedMeshRenderers;
   // private bool isUsingNavMesh = true; // mặc định là NavMesh

    //bien luu tru enemy gan nhat
    private Transform nearestEnemy;
    private float lastAttackTime = -15f;
    //combostep skill fireball
    private int comboStep = 0;
   
    //tham chieu
    Skill3Manager skill3Manager;
    PlayerStatus playerStatus;
    DameZoneSkill3PL dameZoneSkill3PL;
    PlayerControllerState playerControllerState;
    public SkillUseHandler skillUseHandler;
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
       if(aiPath == null)
        {
            aiPath = GetComponent<AIPath>();
        }
        dameZoneSkill3PL= FindAnyObjectByType<DameZoneSkill3PL>();
        skillUseHandler = FindAnyObjectByType<SkillUseHandler>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
        aiPath.maxSpeed = speed;   
        StartCoroutine(ReturnToPool());
        animator.SetTrigger("Fish");
        InitMovementSystem();
    }

    void Update()
    {

        FindNearestEnemy();
      //  StartCoroutine(ReturnToPool());
        if (nearestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, nearestEnemy.position);

            if (distanceToEnemy >= attackRange && distanceToEnemy <= detectionRange)
            {
                
                MoveToEnemy();


            }
            else if(distanceToEnemy <= attackRange)
            {
              
                
                animator.SetBool("Run", false);
              
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackEnemy();
                    lastAttackTime = Time.time;
                }
             
            }
        }
        else// nếu không có kẻ thù gần thi tìm người chơi
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer >= attackRange)
            {
                MoveToPlayer();
            }
            else
            {
                // Dừng di chuyển
               
                   
                animator.SetBool("Run", false);
            }
           
        }
        if (playerStatus.currentHp <= 0)
        {
            ObjPoolingManager.Instance.ReturnToPool(skill3Manager.clonePLTag, gameObject);
        }
        //doi skin
        if (playerControllerState.isSkinSkill3Clone == true)
        {
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                skinnedMeshRenderers[i].enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                skinnedMeshRenderers[i].enabled = false;
            }
        }
        if (!player.activeSelf)
        {
            ObjPoolingManager.Instance.ReturnToPool(skill3Manager.clonePLTag, gameObject);
        }
    }
    void FindNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
        float closestDistance = Mathf.Infinity;
        nearestEnemy = null;

        foreach (Collider collider in hitColliders)
        {

            if (!collider.enabled) continue;
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
        if (nearestEnemy == null) return;

        animator.SetBool("Run", true);

        // Nếu AIPath đang hoạt động → dùng AIPath
        if (aiPath != null && aiPath.isActiveAndEnabled)
        {
            if (agent != null) agent.enabled = false;
            aiPath.enabled = true;

            aiPath.destination = nearestEnemy.position; // ✅ dùng nearestEnemy
        }
        else if (agent != null && agent.isActiveAndEnabled)
        {
            if (aiPath != null) aiPath.enabled = false;
            agent.enabled = true;

            agent.SetDestination(nearestEnemy.position); // ✅ dùng nearestEnemy
        }
        else
        {
            animator.SetBool("Run", false);
        }
    }

    public void MoveToPlayer()
    {
        if (player == null) return;

        animator.SetBool("Run", true);

        if (aiPath != null && aiPath.isActiveAndEnabled)
        {
            if (agent != null) agent.enabled = false;
            aiPath.enabled = true;

            aiPath.destination = player.transform.position;
        }
        else if (agent != null && agent.isActiveAndEnabled)
        {
            if (aiPath != null) aiPath.enabled = false;
            agent.enabled = true;

            agent.SetDestination(player.transform.position);
        }
        else
        {
            animator.SetBool("Run", false);
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
    void InitMovementSystem()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            // Có NavMesh → dùng agent
            agent.enabled = true;
            aiPath.enabled = false;
        }
        else
        {
            // Không có NavMesh → dùng aiPath
            agent.enabled = false;
            aiPath.enabled = true;
        }
    }

    public void StartDameZone()
    {
       
        dameZoneSkill3PL.beginDame();
    }
    public void EndDameZone()
    {
        dameZoneSkill3PL.endDame();
    }

    // Trong Skill3ClonePLayer
    public void PlayFireBallAnim()
    {
        if (!skill3Manager. isLv6) return;
        if (comboStep == 0)
        {
            animator.SetTrigger("FireBall1");
            comboStep = 1;
        }
        else if (comboStep == 1)
        {
            animator.SetTrigger("FireBall2");
            comboStep = 2;
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("FireBall3");
            comboStep = 0;
        }
       
       
    }
    public void PlayRainFireAnim()
    {
        if (!skill3Manager.isLv6) return;
        StartCoroutine(WaitForGravity());
        animator.SetTrigger("RainFire");

    }
    public IEnumerator WaitForGravity()
    {
       
            agent.enabled = false;
        
            aiPath.enabled = false;

        yield return new WaitForSeconds(3f);

   
            agent.enabled = true;
        
            aiPath.enabled = true;
    }

   
}
