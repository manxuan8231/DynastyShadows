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
   //skin
   public SkinnedMeshRenderer[] skinnedMeshRenderers;
   
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
        agent = GetComponent<NavMeshAgent>();
        dameZoneSkill3PL= FindAnyObjectByType<DameZoneSkill3PL>();
        skillUseHandler = FindAnyObjectByType<SkillUseHandler>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();
        animator = GetComponent<Animator>();
        agent.speed = speed;
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
            if(agent.enabled == true)
            {
                // Di chuyển đến kẻ thù
                agent.SetDestination(nearestEnemy.position);
            }
           
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
        if(comboStep == 0)
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
        StartCoroutine(WaitForGravity());
        animator.SetTrigger("RainFire");

    }
    public IEnumerator WaitForGravity()
    {
        agent.enabled = false;
        yield return new WaitForSeconds(3f);
        agent.enabled = true;
       
    }
}
