using Pathfinding;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;


public class DemonAlien : MonoBehaviour
{
    public enum EnemyState
    {
      None, Idle, TargetPl, Attack, Flee, Skill, Anger, Die
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
    //Skill
    public float coolDownSkill = 10f; // Thời gian hồi chiêu tấn công
    public float lastSkillTime = -10f;
    public float stepSkill = 0;
    public float timeUseSkill = 0;
    public bool isSkillTele = false;//neu dang dung skill ma bi danh thi tele
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
        currentState = EnemyState.Idle;
    }

    
    void Update()
    {
        if (currentState == EnemyState.Die) return;
        FleeToPlayer();
        Tele();
        //cap nhap cac trnag thai
        switch (currentState)
        {
            case EnemyState.Idle://Idle
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance <= detectionRange)
                {
                    ChangerState(EnemyState.TargetPl);
                }
                break;
            case EnemyState.TargetPl:
                TargetPlayer();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Flee://chay tron
                FleeFromPlayer();
                break;
            case EnemyState.Skill:
                Skill();    
                break;
            case EnemyState.Anger:
                
                break;
            case EnemyState.Die:         
                
                break;

        }
    }
    public void ChangerState(EnemyState stateNew)//dung de chuyen trang thai 
    {
        if (currentState == stateNew) return;
        currentState = stateNew;
        switch (currentState) 
        { 
            case EnemyState.Idle:
                break;
            case EnemyState.TargetPl:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Flee://chay tron
                break;
            case EnemyState.Skill:             
    
                break;
            case EnemyState.Die:
                Death();
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

        
        if (dist > 50f)
        {
            // Hướng sau lưng player
            Vector3 behindPlayer = player.position - player.forward * 6f;

            // Tìm node hợp lệ gần vị trí sau lưng
            NNInfo nodeInfo = AstarPath.active.GetNearest(behindPlayer, NNConstraint.Default);

            if (nodeInfo.node != null && nodeInfo.node.Walkable)
            {
                transform.position = nodeInfo.position;
            }

            return; 
        }

        // Nếu trong vùng phát hiện và còn khoảng cách
        if (dist <= detectionRange && dist > stopRange)
        {
            aiPath.destination = player.position;

            if (dist >= 15f)
            {
                aiPath.maxSpeed = 15f;
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);
            }
            else if (dist < 14f)
            {
                aiPath.maxSpeed = 5f;
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            aiPath.destination = transform.position;
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            ChangerState(EnemyState.Attack);
        }

        if (!playerControllerState.controller.enabled)
        {
            playerControllerState.controller.enabled = true;
        }
    }


    //Attack
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
                demonAlienHp.currentMana -= 400;
              
            }
            else if (stepAttack == 1)
            {
               
                stepAttack = 2;
                animator.SetTrigger("punch2"); // Kích hoạt animation tấn công1
                demonAlienHp.currentMana -= 400;

            }
            else if (stepAttack == 2)
            {
                
                stepAttack = 0;
                animator.SetTrigger("punch3");
                demonAlienHp.currentMana -= 400;

            }
            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
          
        }
        else if (dist > stopRange) 
        {
            ChangerState(EnemyState.TargetPl);
        }
       
    }
    //Skill
    public void Skill()
    {
        aiPath.canMove = false;
        if (Time.time >= lastSkillTime + coolDownSkill) 
        {
            if (stepSkill == 0 && demonAlienHp.currentMana >= 30)
            {
                stepSkill++;
                animator.SetTrigger("short");//ban 
                timeUseSkill = 7f;//thgian de doi qua trnag thai target
                scoreMaxTele += 3;             
            }
            else if(stepSkill == 1)
            {
                stepSkill++;            
                animator.SetTrigger("telePathic");//la Skill hut player lai
                timeUseSkill = 4f;
                scoreMaxTele += 3;
            }
            else if (stepSkill == 2 && demonAlienHp.currentMana > 0)
            {
                stepSkill = 0;
                animator.SetTrigger("throw");//la Skill nem bong
                timeUseSkill = 8f;
                scoreMaxTele -= 6;
                demonAlienHp.currentMana -= 600;
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
        Vector3 FleeTarget = transform.position + runDirection * 40f;
        aiPath.maxSpeed = 15;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance >= 50)
        {
            FlipToPlayer();
            animator.SetBool("isRunning", false);
            demonAlienHp.currentHp += 40 * Time.deltaTime;
            demonAlienHp.UpdateUI();
        }
        else if(distance < 50)
        {
            aiPath.destination = FleeTarget;
            animator.SetBool("isRunning", true);
           
        }
        if (demonAlienHp.currentHp > demonAlienHp.maxHp * 0.3f) //lon hon 20%
        {
            ChangerState(EnemyState.Idle);
        }
    }
    //tele
    public void Tele()
    {
        if (scoreTele >= scoreMaxTele)
        {
            scoreTele = 0;
            evenAlien.audioSource.PlayOneShot(evenAlien.teleClip);
            demonAlienHp.currentArmor += 300;
            demonAlienHp.UpdateUI();
            StartCoroutine(WaitCanMoveAndChangerSkill(2f));//tam dung canmove và đợi 2f dùng Skill
            animator.SetTrigger("comeOut");
            // Tính hướng ngược với player
            Vector3 directionAway = (transform.position - player.position).normalized;
            Vector3 targetPos = transform.position + directionAway * 45f;

            // Tìm vị trí gần nhất trên Graph
            NNInfo nodeInfo = AstarPath.active.GetNearest(targetPos, NNConstraint.Default);

            if (nodeInfo.node != null && nodeInfo.node.Walkable)
            {
                transform.position = nodeInfo.position;
            }
            
        }
    }

    //chay tron neu mau be hon 30%
    public void FleeToPlayer()
    {
        if (demonAlienHp.currentHp <= demonAlienHp.maxHp * 0.2f) //20%
        {
            ChangerState(EnemyState.Flee);
        }
    }
    
    //die
    public void Death()
    {
        demonAlienHp.colliTakeDame.enabled = false;
        animator.SetTrigger("die");
        Destroy(gameObject,2);
    }

    //chuyen trang thai cua cac ham-----------------
    public void StartAttack()
    {
        ChangerState(EnemyState.Attack);
    }
    public void StartTargetPl()
    {
        ChangerState(EnemyState.TargetPl);
    }

    //IEnumerator-------------------------------
    public IEnumerator WaitCanMoveAndChangerSkill(float secont)//dùng để tắt canmove và sử dụng Skill sau secon
    {
        aiPath.canMove = false;
        yield return new WaitForSeconds(secont);
        aiPath.canMove = true;
        ChangerState(EnemyState.Skill);
        
    }
    public IEnumerator WaitChangerStateTarget(float secont)
    {
        isSkillTele = true;
        yield return new WaitForSeconds(secont);     
        aiPath.canMove = true;
        ChangerState(EnemyState.TargetPl);
        evenAlien.effectShort.SetActive(false);
        isSkillTele = false;
    }
}
