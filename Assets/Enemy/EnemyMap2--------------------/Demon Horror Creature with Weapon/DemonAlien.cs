using Pathfinding;
using System.Collections;
using UnityEngine;


public class DemonAlien : MonoBehaviour
{
    public enum EnemyState
    {
       idle, targetPl,attack,flee,skill,teleBack
    }
    public EnemyState currentState;
    [Header("---------Thong so tinh khoan cach--------")]
    public Transform player;   
    public float stopRange = 2f; 
    public float detectionRange = 100f; // Khoảng cách phát hiện người chơi

    [Header("--------Attack Cooldown-----------")]
    public float coolDownAttack = 10f; // Thời gian hồi chiêu tấn công
    public float lastAttackTime = -10f; 
    public float stepAttack = 0;

    [Header("--------------Thong so de tele-----------")]
    public float scoreTele = 0;
    public float scoreMaxTele = 5;

    [Header("--------------Tham chieu ------------")]
    //tham chieu 
    public Animator animator;
    public AIPath aiPath;
    private DemonAlienHp demonAlienHp;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindClosestPlayer();
        demonAlienHp = FindAnyObjectByType<DemonAlienHp>();
        aiPath = GetComponent<AIPath>();
      
        currentState = EnemyState.idle;
    }

    
    void Update()
    {
        //neu yeu mau thi chay tron
        if (demonAlienHp.currentHp <= demonAlienHp.maxHp * 0.3f) //mau be hon 20%
        {
            ChangerState(EnemyState.flee);
        }
        //neu du diem thi tele
        if (scoreTele >= scoreMaxTele) 
        { 
            scoreTele = scoreMaxTele;
        }

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
            case EnemyState.targetPl://
                TargetPlayer();
                break;
            case EnemyState.attack://
                Attack();
                break;
            case EnemyState.flee://chay tron
                FleeFromPlayer();
                break;

        }
    }
    public void ChangerState(EnemyState stateNew)//dung de chuyen trang thai 
    {
        currentState = stateNew;
        switch (currentState) { 
            case EnemyState.idle:
                break;
            case EnemyState.targetPl:
                break;
            case EnemyState.attack:
                break;
            case EnemyState.flee://chay tron

                break;
        }
    }


    //------------------------cac ham chuc nang----------

    //thay player thi run hoac walk
    public void TargetPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist <= detectionRange && dist > stopRange)
        {
            aiPath.destination = player.position;// di chuyen thang toi player
            if (dist >= 20) // Khoảng cách xa hơn 30 thì chạy
            {
               
                aiPath.maxSpeed = 15f; 

                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", true);

            }
            else if(dist < 19)
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

    //chuyen trang thai cua cac ham-----------------
    public void StartAttack()
    {
        ChangerState(EnemyState.attack);
    }
}
