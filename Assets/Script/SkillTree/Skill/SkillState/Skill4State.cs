using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skill4State : PlayerState
{
    public Skill4State(PlayerControllerState player) : base(player) { }
    [Header("Thời gian cooldown cho mỗi đòn combo")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.6f;
    [SerializeField] private float attack3Cooldown = 0.7f;
    private bool isMovement = false;
    private int comboStep = 0;
    private float nextAttackTime = 0f;
    public bool isAttack = true;
    private float coolDownAttackFly = -5f;


    public override void Enter()
    {
        
        player.animator.runtimeAnimatorController = player.animatorSkill4;//chay animator skill 4 khi bat dau
        player.animator.SetTrigger("Change");
        player.weaponSword.SetActive(true); //tat weapon khi chay skill4
        player.StartCoroutine(WaitChangeState()); //bat dau chay ham doi trang thai sau 10 giay
        Debug.Log("Chạy trạng thái skill4");
       
    }
    public override void Update()
    {
        if (player.isController == true && isMovement ==true)
        {
            Move();
            Jump();
            Roll();
            InputAttack();
            
        }
        else
        {
            player.animator.SetBool("isWalking", false);
            player.animator.SetBool("isRunning", false);

        }
    }
    public override void Exit() 
    {
        player.skill4Manager.isChangeStateSkill4 = false; //chuyen thanh false de ko chay lai skill4
        player.weaponSword.SetActive(false); // weapon khi chay skill4
        player.skill4Manager.ToggleSkill4(false); //tat model skill 4
      
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            player.isRunning = Input.GetKey(KeyCode.LeftShift) && player.playerStatus.currentMana > 0;
            float speed = player.isRunning ? player.runSpeed : player.walkSpeed;


            player.animator.SetBool("isRunning", player.isRunning);
            player.animator.SetBool("isWalking", !player.isRunning);

            Vector3 camForward = player.cameraTransform.forward;
            Vector3 camRight = player.cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;
            player.controller.Move(moveDir * speed * Time.deltaTime);

            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
        else
        {
            player.animator.SetBool("isWalking", false);
            player.animator.SetBool("isRunning", false);
        }

    }

    public void Jump()
    {
        player.isGrounded = Physics.CheckSphere(player.groundCheck.position, player.groundDistance, player.groundMask);

        if (player.isGrounded && player.velocity.y < 0)
            player.velocity.y = -2f;
        //
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded && player.playerStatus.currentMana > 50)
        {
            player.playerStatus.TakeMana(50);
            player.velocity.y = Mathf.Sqrt(player.jumpHeight * -2f * player.gravity);
            player.audioSource.PlayOneShot(player.evenAnimator.audioJump);
            player.animator.SetTrigger("jump");
        }
        //
        player.animator.SetBool("jumpLand", player.isGrounded && !player.wasGroundedLastFrame == true);

        player.wasGroundedLastFrame = player.isGrounded;

        player.velocity.y += player.gravity * Time.deltaTime;
        player.controller.Move(player.velocity * Time.deltaTime);
    }

    public void Roll()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.isGrounded && player.playerStatus.currentMana > 100 && Time.time >= player.rollColdownTime + 2f)
        {
            player.playerStatus.TakeMana(100);
            player.audioSource.PlayOneShot(player.evenAnimator.audioRoll);
            player.animator.SetTrigger("Roll");
            player.rollColdownTime = Time.time;
        }
    }
    public void InputAttack()
    {
        // Chỉ cho phép tấn công nếu chuột bị khóa (không hiện trên màn hình)
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Tấn công trên mặt đất
            if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime
                && isAttack && player.playerStatus.currentMana > 50 && player.IsGrounded())
            {
                player.playerStatus.TakeMana(50);
                OnAttack();
            }

            // Tấn công khi đang trên không
            if (Input.GetMouseButtonDown(0)
                && isAttack && player.playerStatus.currentMana > 50 && !player.IsGrounded()
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
            player.animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attack1Cooldown;
        }
        else if (comboStep == 2)
        {
            player.animator.SetTrigger("Attack2");
            nextAttackTime = Time.time + attack2Cooldown;
        }
        else if (comboStep == 3)
        {
            player.animator.SetTrigger("Attack3");
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
        player.animator.SetTrigger("FlyAttack");
        player.StartCoroutine(WaitGravity());
    }
    //doi trạng thái sau 10 giây
    public IEnumerator WaitChangeState()
    {

        isMovement = false;//
        yield return new WaitForSeconds(2f);
        isMovement = true;//cho di chuyen
        yield return new WaitForSeconds(player.skill4Manager.timeSkill4); // Thời gian chờ 10 giây rồi chuyển trạng thái
        player.ChangeState(new PlayerCurrentState(player)); // Trở về trạng thái hiện tại
        player.animator.runtimeAnimatorController = player.animatorDefauld; // Trở về animator mặc định
       
    }

    //doi roi xuong
    public IEnumerator WaitGravity() 
    {
        player.thirdPersonOrbitCamera.flipCamera= true;//tat bat flip theo camera
        player.gravity = 0;
        yield return new WaitForSeconds(2.4f);
        player.gravity = -9.81f;
        player.thirdPersonOrbitCamera.flipCamera= false;
    }
}
