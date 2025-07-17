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
    private bool isUsingNavMesh = true; // mặc định là NavMesh

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
        StartCoroutine(ReturnToPool());
       
                                     // Kiểm tra có NavMesh ở vị trí hiện tại không
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            isUsingNavMesh = true;
            agent.enabled = true;
            aiPath.enabled = false;
        }
        else
        {
            isUsingNavMesh = false;
            agent.enabled = false;
            aiPath.enabled = true;
        }

       
        aiPath.maxSpeed = speed;


        
        StartCoroutine(ReturnToPool());
    }

    void Update()
    {

        FindNearestEnemy();
        StartCoroutine(ReturnToPool());
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
        }
        else
        {
            agent.enabled = false;
            aiPath.enabled = false;           
            animator.SetBool("Run", false);
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
        float dis = Vector3.Distance(transform.position, nearestEnemy.position);
        if (nearestEnemy != null && dis <= 50)
        {
            animator.SetBool("Run", true);
            agent.enabled = true;
            aiPath.enabled = true;
            if (isUsingNavMesh && agent.enabled)
            {
                agent.SetDestination(nearestEnemy.position);
            }
            else if (!isUsingNavMesh && aiPath.enabled)
            {
                aiPath.destination = nearestEnemy.position;
            }
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
        if (isUsingNavMesh)
            agent.enabled = false;
        else
            aiPath.enabled = false;

        yield return new WaitForSeconds(3f);

        if (isUsingNavMesh)
            agent.enabled = true;
        else
            aiPath.enabled = true;
    }

    //skill slash
    public void PlaySlashAnim()
    {
        if (!skill3Manager.isLv6) return;
        FindNearestEnemy();
        Vector3 dashDir = transform.forward;
        float dashDistance = 30f; // độ dài lướt tối đa

        // Raycast kiểm tra vật cản phía trước
        RaycastHit hit;
        Vector3 finalTargetPos = transform.position + dashDir * dashDistance;

        if (Physics.Raycast(transform.position, dashDir, out hit, dashDistance, LayerMask.GetMask("Ground")))
        {
            // Nếu trúng tường, chỉ dash tới trước tường một chút
            finalTargetPos = hit.point - dashDir * 0.5f;
        }
        StartCoroutine(DashToTarget(finalTargetPos, 0.25f));
        if (comboStep == 0)
        {
             
            animator.SetTrigger("Slash");
            comboStep = 1;
        }
        else if (comboStep == 1)
        {
            
            animator.SetTrigger("Slash2");
            comboStep = 2;
        }
        else if (comboStep == 2)
        {
           
            animator.SetTrigger("Slash3");
            comboStep = 0;
        }


    }
    IEnumerator DashToTarget(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = transform.position;
        float time = 0f;

        // Tạm tắt di chuyển AI
        if (isUsingNavMesh)
            agent.enabled = false;
        else
            aiPath.enabled = false;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
       
        // Bật lại di chuyển AI
        if (isUsingNavMesh)
            agent.enabled = true;
        else
            aiPath.enabled = true;
    }

   
}
