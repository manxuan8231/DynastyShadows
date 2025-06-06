using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skill4State : PlayerState
{
    public Skill4State(PlayerControllerState player) : base(player) { }
    //tham chieu

    public override void Enter()
    {
        
        player.animator.runtimeAnimatorController = player.animatorSkill4;//chay animator skill 4 khi bat dau
        player.animator.SetTrigger("Change");
        player.weaponSword.SetActive(false); //tat weapon khi chay skill4
        player.StartCoroutine(WaitChangeState()); //bat dau chay ham doi trang thai sau 10 giay
        Debug.Log("Chạy trạng thái skill4");
    }
    public override void Update()
    {
        if (player.isController == true)
        {
            Move();
            Jump();
            Roll();
            player.comboAttack.InputAttack();
            
        }
        else
        {
            player.animator.SetBool("isWalking", false);
            player.animator.SetBool("isRunning", false);

        }
    }
    public override void Exit() 
    {
        player.skill4Manager.isInputSkill4 = false; //chuyen thanh false de ko chay lai skill4
        player.weaponSword.SetActive(true); // weapon khi chay skill4
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
            player.audioSource.PlayOneShot(player.audioJump);
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
            player.audioSource.PlayOneShot(player.audioRoll);
            player.animator.SetTrigger("Roll");
            player.rollColdownTime = Time.time;
        }
    }

    //doi trạng thái sau 10 giây
    public IEnumerator WaitChangeState()
    {
        yield return new WaitForSeconds(player.skill4Manager.timeSkill4); // Thời gian chờ 10 giây rồi chuyển trạng thái
        player.ChangeState(new PlayerCurrentState(player)); // Trở về trạng thái hiện tại
        player.animator.runtimeAnimatorController = player.animatorDefauld; // Trở về animator mặc định
       
    }
}
