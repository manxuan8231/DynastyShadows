using Pathfinding;
using System.Collections;
using UnityEngine;


public class DemonAlien : MonoBehaviour
{
    public enum EnemyState
    {
       idle, targetPl,attack,flee,skill,die
    }
    public EnemyState currentState;
    [Header("---------Thong so tinh khoan cach--------")]
    public Transform player;   
    public float stopRange = 2f; 
    public float detectionRange = 100f; // Khoảng cách phát hiện người chơi

    [Header("--------Cooldown-----------")]
    public float coolDownAttack = 10f; // Thời gian hồi chiêu tấn công
    public float lastAttackTime = -10f; 
    public float stepAttack = 0;
    //skill
    public float coolDownSkill = 10f; // Thời gian hồi chiêu tấn công
    public float lastSkillTime = -10f;
    public float stepSkill = 0;
    public float timeUseSkill = 0;
    [Header("--------------Thong so de tele-----------")]
    public float scoreTele = 0;
    public float scoreMaxTele = 5;

    [Header("--------------Tham chieu ------------")]
    //tham chieu 
    public Animator animator;
    public AIPath aiPath;
    private DemonAlienHp demonAlienHp;
    private EvenAlien evenAlien;
    private PlayerControllerState playerControllerState;
   
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        demonAlienHp = FindAnyObjectByType<DemonAlienHp>();
        aiPath = GetComponent<AIPath>();
        evenAlien = FindAnyObjectByType<EvenAlien>();
        playerControllerState = FindAnyObjectByType <PlayerControllerState>();  
        currentState = EnemyState.idle;
    }

    
    void Update()
    {
        FleeToPlayer();
        Tele();
        //cap nhap cac trnag thai
        switch (currentState)
        {
            case EnemyState.idle://idle
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= detectionRange)
                {
                    ChangerState(EnemyState.targetPl);
                }
                break;
            case EnemyState.targetPl:
                TargetPlayer();
                break;
            case EnemyState.attack:
                Attack();
                break;
            case EnemyState.flee://chay tron
                FleeFromPlayer();
                break;
            case EnemyState.skill:
                Skill();    
                break;
            case EnemyState.die:         
                aiPath.isStopped = false;

                break;

        }
    }
    public void ChangerState(EnemyState stateNew)//dung de chuyen trang thai 
    {
        currentState = stateNew;
        switch (currentState) 
        { 
            case EnemyState.idle:
                break;
            case EnemyState.targetPl:
                break;
            case EnemyState.attack:
                break;
            case EnemyState.flee://chay tron
                break;
            case EnemyState.skill:             
                break;
            case EnemyState.die:
                if (!animator.enabled) { 
                    animator.enabled = true;
                }
                if (aiPath.enabled)
                {
                    aiPath.enabled = false;
                }
                
                animator.SetTrigger("die");

                break;
        }
    }


    //------------------------cac ham chuc nang----------
    //gan nhat player
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

    //thay player thi run hoac walk
    public void TargetPlayer()
    {
       
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= detectionRange && dist > stopRange)
        {
            aiPath.destination = player.position;// di chuyen thang toi player
            if (dist >= 15) // Khoảng cách xa hơn 15 thì chạy
            {
               
                aiPath.maxSpeed = 15f; 
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);

            }
            else if(dist < 14)
            {
                    aiPath.maxSpeed = 5f; // Tốc độ walk
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isWalking", true);          
            }
        }
        else
        {
            aiPath.destination = transform.position; // Dừng lại nếu không có player
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            ChangerState(EnemyState.attack);
        }
        if (!playerControllerState.controller.enabled)
        {
            playerControllerState.controller.enabled = true;
        }
    }
   
    //attack
    public void Attack()
    {
        FlipToPlayer();
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= stopRange && Time.time >= lastAttackTime + coolDownAttack)
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
                animator.SetTrigger("punch3");
              
               
            }
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
          
        }
        else if (dist > stopRange) 
        {
            ChangerState(EnemyState.targetPl);
        }
       
    }
    //skill
    public void Skill()
    {
        aiPath.canMove = false;
        if (Time.time >= lastSkillTime + coolDownSkill) 
        {
            if (stepSkill == 0)
            {
                stepSkill++;
                animator.SetTrigger("short");//ban 
                timeUseSkill = 7f;//thgian de doi qua trnag thai target
            }
            else if(stepSkill == 1)
            {
                stepSkill++;            
                animator.SetTrigger("telePathic");//la skill hut player lai
                timeUseSkill = 4f;
            }
            else if (stepSkill == 2)
            {
                stepSkill = 0f;
                animator.SetTrigger("throw");//la skill nem bong
                timeUseSkill = 8f;
            }
            StartCoroutine(WaitChangerStateTarget(timeUseSkill));

            lastSkillTime = Time.time;    
        }
     
    }
    //flip
    public void FlipToPlayer()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; // Đặt y về 0 để chỉ xoay trên mặt phẳng ngang
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        // transform.LookAt(player.transform.position);
    }
    //chay tron
    public void FleeFromPlayer()
    {      
       
        Vector3 runDirection = (transform.position - player.position).normalized;   
        Vector3 fleeTarget = transform.position + runDirection * 20f;
        aiPath.maxSpeed = 15;
        aiPath.destination = fleeTarget;  
        animator.SetBool("isRunning", true);
        if (demonAlienHp.currentHp > demonAlienHp.maxHp * 0.3f) //lon hon 20%
        { 
            ChangerState(EnemyState.idle);
        }
    }
    //tele
    public void Tele()
    {
        if (scoreTele >= scoreMaxTele)
        {
            scoreTele = 0;
            evenAlien.audioSource.PlayOneShot(evenAlien.teleClip);
            demonAlienHp.currentArmor = demonAlienHp.maxArmor;//hoi lai giap
            demonAlienHp.UpdateUI();
            StartCoroutine(WaitCanMoveAndChangerSkill(2f));//tam dung canmove và đợi 2f dùng skill
            animator.SetTrigger("comeOut");
            // Tính hướng ngược với player
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 targetPos = transform.position + directionAway * 45f;

            // Tìm vị trí gần nhất trên Graph
            NNInfo nodeInfo = AstarPath.active.GetNearest(targetPos, NNConstraint.Default);

            if (nodeInfo.node != null && nodeInfo.node.Walkable)
            {
                transform.position = nodeInfo.position;
                Debug.Log("Đã teleport đến: " + nodeInfo.position);
            }
            else
            {
                Debug.LogWarning("Không tìm được vị trí hợp lệ để teleport.");
            }
            
        }
    }

    //chay tron neu mau be hon 30%
    public void FleeToPlayer()
    {
        if (demonAlienHp.currentHp <= demonAlienHp.maxHp * 0.3f) //mau be hon 20%
        {
            ChangerState(EnemyState.flee);
        }
    }
    
   

    //chuyen trang thai cua cac ham-----------------
    public void StartAttack()
    {
        ChangerState(EnemyState.attack);
    }
    public void StartTargetPL()
    {
        ChangerState(EnemyState.targetPl);
    }

    //IEnumerator-------------------------------
    public IEnumerator WaitCanMoveAndChangerSkill(float secont)//dùng để tắt canmove và sử dụng skill sau secon
    {
        aiPath.canMove = false;
        yield return new WaitForSeconds(secont);
        aiPath.canMove = true;
        ChangerState(EnemyState.skill);
        
    }
    public IEnumerator WaitChangerStateTarget(float secont)
    {
       
        yield return new WaitForSeconds(secont);     
        aiPath.canMove = true;
        ChangerState(EnemyState.targetPl);
        evenAlien.effectShort.SetActive(false);
    }
}
