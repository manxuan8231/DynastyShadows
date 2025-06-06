using UnityEngine;

public class ComboAttack : MonoBehaviour
{

    [Header("Thời gian cooldown cho mỗi đòn combo")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.6f;
    [SerializeField] private float attack3Cooldown = 0.7f;

    private int comboStep = 0;
    private float nextAttackTime = 0f;
    public bool isAttack = true;
    private float coolDownAttackFly = -5f;
  
    
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
            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime
                && isAttack && playerStatus.currentMana > 50 && playerController.IsGrounded())
            {
                playerStatus.TakeMana(50);
                OnAttack();
            }

            // Tấn công khi đang trên không
            if (Input.GetMouseButtonDown(0)
                && isAttack && playerStatus.currentMana > 50 && !playerController.IsGrounded()
                && Time.time >= coolDownAttackFly + 5)
            {
                OnAttackFly();
                coolDownAttackFly = Time.time;
            }

            // Reset combo nếu không tấn công sau 1.2s
            if (Time.time >= nextAttackTime + 1.2f)
            {
                comboStep = 0;
            }
        }       
    }


    void OnAttack()
    {
        
        comboStep++;

        if (comboStep == 1)
        {
            playerController.animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attack1Cooldown;
        }
        else if (comboStep == 2)
        {
            playerController.animator.SetTrigger("Attack2");
            nextAttackTime = Time.time + attack2Cooldown;
        }
        else if (comboStep == 3)
        {
            playerController.animator.SetTrigger("Attack3");
            nextAttackTime = Time.time + attack3Cooldown;
            comboStep = 0;
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

    
   


    
}
