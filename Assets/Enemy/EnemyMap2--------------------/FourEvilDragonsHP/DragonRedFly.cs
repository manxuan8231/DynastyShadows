using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class DragonRedFly : MonoBehaviour
{
    public Transform[] posiTarget; // Các vị trí bay
    public bool isFly = false; // Kiểm tra xem có đang bay hay không
    public bool isFlyTakeOff = true; // Kiểm tra để chỉ bay lên 1 lần
    private bool isFlyingToTargets = false;
    private bool isFlipToPlayer = false;
    //attack
    public float cooldownAttack = 8f; // Thời gian hồi chiêu tấn công
    public float lastAttackTime = -8f; // Thời gian tấn công cuối cùng
    public float timeToDash = 10f; 
    public float rangePosi = 0.1f;
    public float stepAttack = 0; // Biến để theo dõi bước tấn công
    public bool isAttackFly = false; // Kiểm tra xem có đang tấn công hay không
   
    // Tham chiếu  
    private DragonRedHP dragonRedHP;
    private DragonRed dragonRed;
    private EvenAnimatorDraRed evenAnimatorDraRed;
    void Start()
    {
        dragonRedHP = FindAnyObjectByType<DragonRedHP>();
        dragonRed = FindAnyObjectByType<DragonRed>();
        evenAnimatorDraRed = FindAnyObjectByType<EvenAnimatorDraRed>();

        List<Transform> targets = new List<Transform>();
        foreach (Transform t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
        {
            if (t.name.StartsWith("posiRed"))
            {
                targets.Add(t);
            }
        }
        posiTarget = targets.ToArray();

    }

    void Update()
    {
        FlyTrigger();
        AttackFly();
        FlipToPlayer();
        //neu ko bay du vi tri thi tu xuong khi het mana
        if(dragonRedHP.currentMana <= 0)
        {
            StartCoroutine(WaitTurnOffFly(7));
        }
        //het giap thi tat fly
        if (dragonRedHP.currentArmor <= 0) {
            StartCoroutine(WaitTurnOffFly(1));
        }
    }

    // Kích hoạt bay khi du điều kiện
    public void FlyTrigger()
    {
        if (dragonRedHP.isStunned) return;
        
        if (dragonRedHP.currentMana >= dragonRedHP.maxMana && isFlyTakeOff)
        {
            
           
          
            dragonRed.animator.SetTrigger("FlyTakeOff"); // Animation bay lên
            isFly = true;
            dragonRed.isAttack = false;
            dragonRed.isMove = false;
            dragonRed.isFlipAllowed = false;

            isFlyTakeOff = false;// Đảm bảo chỉ bay lên một lần
            StartCoroutine(StartFlyingAfterDelay(4f));
        }
    }
    public void AttackFly()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, dragonRed.player.transform.position);
        if (isAttackFly && Time.time >= lastAttackTime + cooldownAttack && distanceToPlayer <= 70f) 
        {
            dragonRedHP.currentMana -= 150; // Giảm mana khi tấn công\
            dragonRedHP.UpdateUI(); // Cập nhật UI mana
            if (stepAttack==0)
            {
                stepAttack++;
                dragonRed.animator.SetTrigger("FlyFlame");
                
            }
            else if (stepAttack == 1)
            {
                stepAttack--;
                dragonRed.navMeshAgent.speed = 50f;             
                dragonRed.animator.SetTrigger("FlyGlide");
               
            }
           

            lastAttackTime = Time.time; // Cập nhật thời gian tấn công cuối cùng
        }
        if (isAttackFly)
        {
                   
            dragonRed.navMeshAgent.SetDestination(dragonRed.player.transform.position); // Di chuyển đến vị trí tấn công
        }
    }
    public void FlipToPlayer()
    {
        if (isFlipToPlayer == false) return;
        if(dragonRed.isAttack) return; // Nếu đang tấn công thì không quay lại player
        Vector3 direction = (dragonRed.player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 15f);

    }
   
    IEnumerator StartFlyingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isFlyingToTargets)
        {
            isFlyingToTargets = true;
            StartCoroutine(FlyToTargets());
        }
    }

    IEnumerator FlyToTargets()
    {
        foreach (Transform target in posiTarget)
        {
            while (Vector3.Distance(transform.position, target.position) > rangePosi)
            {
                dragonRed.animator.SetBool("FlyForWard", true); 
                //transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 25);
                dragonRed.navMeshAgent.speed = 60f; // Tốc độ bay
                if (dragonRed.navMeshAgent.enabled == true)
                {
                    dragonRed.navMeshAgent.SetDestination(target.position);
                }            
                yield return null;
            }
            Debug.Log("Đã đến vị trí: " + target.name + " | Vị trí: " + target.position);
            dragonRed.animator.SetBool("FlyForWard", false);
            isFlipToPlayer = true;
             //dợi tan cong
             yield return new WaitForSeconds(1f); 
            isAttackFly = true; // Bật trạng thái tấn công bay

            yield return new WaitForSeconds(5f);
            isAttackFly = false;
        }
        if(dragonRedHP.currentMana <= 0)
        {
            if (isFly)
            {
                dragonRed.animator.SetTrigger("FlyLand");
                isFly = false;
            }
            yield return new WaitForSeconds(2f); // Thời gian hạ cánh
            dragonRed.navMeshAgent.speed = 10f; // Trả về tốc độ bình thường
            evenAnimatorDraRed.EndEffectFlame();//tat hiệu ứng lửa


            isFlyingToTargets = false;
            dragonRed.isAttack = true;
            dragonRed.isMove = true;
            dragonRed.isFlipAllowed = true;
            isAttackFly = false;
            isFlyTakeOff = true;
        }
    }
     
    IEnumerator WaitTurnOffFly(float time)//neu loi ko xuong thi goi ham nay
    {
        yield return new WaitForSeconds(time);
        if (dragonRedHP.currentMana <= 0)
        {
            if (isFly)
            {
                dragonRed.animator.SetTrigger("FlyLand");
                isFly = false;
            }
            yield return new WaitForSeconds(2f); // Thời gian hạ cánh
            dragonRed.navMeshAgent.speed = 10f; // Trả về tốc độ bình thường
            evenAnimatorDraRed.EndEffectFlame();//tat hiệu ứng lửa


            isFlyingToTargets = false;
            dragonRed.isAttack = true;
            dragonRed.isMove = true;
            dragonRed.isFlipAllowed = true;
            isAttackFly = false;
            isFlyTakeOff = true;
        }
    }
   
}
