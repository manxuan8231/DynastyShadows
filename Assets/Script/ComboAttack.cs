using System.Collections;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{

    [Header("Thời gian cooldown cho mỗi đòn combo")]
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.6f;
    [SerializeField] private float attack3Cooldown = 0.7f;

    public int comboStep = 0;
    private float nextAttackTime = 0f;
    public bool isAttack = true;
    private float coolDownAttackFly = -5f;

    public Coroutine coroutine;
    //Goi ham
    PlayerStatus playerStatus;
    PlayerControllerState playerController;
   

    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
    }

    void Update()
    {

        //InputAttack();
    }
    public void InputAttack()
    {
        // Chỉ cho phép tấn công nếu chuột bị khóa (không hiện trên màn hình)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Tấn công trên mặt đất
            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime + attackCooldown
                && isAttack && playerStatus.currentMana > 50 && playerController.IsGrounded() && playerController.isAttack)
            {
                Transform enemy = playerController.GetNearestEnemy();
                if (enemy != null)
                {
                    Vector3 direction = (enemy.position - playerController.transform.position).normalized;
                    direction.y = 0; // chỉ xoay theo trục Y

                    if (direction != Vector3.zero)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        playerController.transform.rotation = targetRotation; // hoặc dùng Slerp nếu muốn mượt
                    }
                }
                playerStatus.TakeMana(50);
                OnAttack();
                nextAttackTime = Time.time;
            }

            // Tấn công khi đang trên không
            if (Input.GetMouseButtonDown(0)
                && isAttack && playerStatus.currentMana > 50 && !playerController.IsGrounded()
                && Time.time >= coolDownAttackFly + 1.5f)
            {
                OnAttackFly();
                coolDownAttackFly = Time.time;
            }

           
        }       
    }


    void OnAttack()
    {             
        // Chỉ tăng comboStep nếu cooldown đã hết
        if (comboStep == 0)
        {
            comboStep = 1;
            playerController.animator.SetTrigger("Attack1");
            attackCooldown = attack1Cooldown;
            if(coroutine != null)
            {
                StopCoroutine(coroutine); // Dừng coroutine nếu đang chạy
            }
            coroutine = StartCoroutine(WaitResetAttack());
        }
        else if (comboStep == 1)
        {

            comboStep = 2;
            playerController.animator.SetTrigger("Attack2");
            attackCooldown = attack2Cooldown;
            if (coroutine != null)
            {
                StopCoroutine(coroutine); // Dừng coroutine nếu đang chạy
            }
            coroutine = StartCoroutine(WaitResetAttack());
        }
        else if (comboStep == 2)
        {
            comboStep = 3;
            playerController.animator.SetTrigger("Attack3");
            attackCooldown = attack3Cooldown;
            if (coroutine != null)
            {
                StopCoroutine(coroutine); // Dừng coroutine nếu đang chạy
            }
            coroutine = StartCoroutine(WaitResetAttack());
        }
        else
        {
            comboStep = 0;
        }
    }

    void OnAttackFly()
    {
        playerController.animator.SetTrigger("FlyAttack");
    }

    
   public IEnumerator WaitResetAttack()
    {
        yield return new WaitForSeconds(1.5f); // Thời gian chờ trước khi reset
        comboStep = 0; // Reset comboStep về 0
        playerController.animator.ResetTrigger("Attack1");
        playerController.animator.ResetTrigger("Attack2");
        playerController.animator.ResetTrigger("Attack3");
        isAttack = true; // Cho phép tấn công lại
    }


    
}
