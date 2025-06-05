using UnityEngine;
using UnityEngine.Audio;


public class PlayerCurrentState : PlayerState
{
    public PlayerCurrentState(PlayerControllerState player) : base(player) { }

    public override void Enter()
    {
        player.animator.runtimeAnimatorController = player.animatorDefauld;
        player.skill3Manager.isInputSkill3 = true;
        player.skill1Manager.isInputSkill1 = true;
    }

    public override void Update()
    {
        if (player.isController == true)
        {
            Move();
            Jump();
            Roll();
            player.comboAttack.InputAttack();
            //skill2
            if (player.skill2Manager.isInputSkill2)//để chuyển sang trạng thái skill2 animator
            {
             
                player.ChangeState(new Skill2State(player));
            }
            //skill4
            if (player.skill4Manager.isInputSkill4) //để chuyển sang trạng thái skill4 animator
            {
                player.ChangeState(new Skill4State(player));
            }
        }
        else
        {
           player.animator.SetBool("isWalking", false);
           player.animator.SetBool("isRunning", false);

        }
        
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


           player.animator.SetBool("isRunning",player. isRunning);
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
           player. animator.SetBool("isWalking", false);
          player. animator.SetBool("isRunning", false);
        }

    }

    public void Jump()
    {
       player.isGrounded = Physics.CheckSphere(player.groundCheck.position,player.groundDistance, player.groundMask);

        if (player.isGrounded && player.velocity.y < 0)
            player.velocity.y = -2f;
        //
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded && player.playerStatus.currentMana > 50)
        {
           player.playerStatus.TakeMana(50);
           player.velocity.y = Mathf.Sqrt(player.jumpHeight * -2f * player.gravity);
           player.audioSource.PlayOneShot(player.audioJump);
           player. animator.SetTrigger("jump");
        }
        //
       player. animator.SetBool("jumpLand", player.isGrounded && !player.wasGroundedLastFrame == true);

       player.wasGroundedLastFrame = player.isGrounded;

       player.velocity.y += player.gravity * Time.deltaTime;
       player. controller.Move(player.velocity * Time.deltaTime);
    }

    public void Roll()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && player.isGrounded && player.playerStatus.currentMana > 100 && Time.time >= player.rollColdownTime + 2f)
        {
           player. playerStatus.TakeMana(100);
           player. audioSource.PlayOneShot(player.audioRoll);
           player.animator.SetTrigger("Roll");
            player.rollColdownTime = Time.time;
        }
    }


    public override void Exit() {
        //tat ko cho phép nhap skill1 va skill3
        player.skill3Manager.isInputSkill3 = false;
        player.skill1Manager.isInputSkill1 = false;

    }
}
