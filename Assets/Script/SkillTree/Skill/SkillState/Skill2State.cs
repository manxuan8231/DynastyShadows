using System.Collections;
using Unity.Cinemachine.Samples;
using Unity.VisualScripting;
using UnityEngine;

public class Skill2State : PlayerState
{
    public Skill2State(PlayerControllerState player) : base(player) { }
    public LayerMask enemyLayer = LayerMask.GetMask("Enemy"); // Layer của enemy
    private bool isMove = false;
   
    private bool isAttack = true; // Biến để kiểm tra trạng thái tấn công
    private bool isChangeState = false; // Biến để kiểm tra trạng thái chuyển đổi
    public override void Enter()
    {
        isChangeState = true;
        isAttack = true;
        player.animator.runtimeAnimatorController = player.animatorSkill2;
        player.skill2Manager.effectRun.SetActive(true);
        Debug.Log("Chạy trạng thái skill2");
        player.skill2Manager.isHibitedIcon = true; // cấm sử dụng skill con lại icon
        player.StartCoroutine(WaitForMove()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại
        player.StartCoroutine(WaitForSkill2()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại

       
    }

    public override void Update()
    {
       
        if (isMove == true && player.isController == true)
        {
            Move();
            Jump();
            DashToNearestEnemyAndAttack();
        }
        else
        {
           

        }
       if(Input.GetKeyDown(KeyCode.F))
       {
           
            player.animator.SetBool("Run", false);
        
       }
        if (player.animator.enabled == false) {
            //skill2
            player.animator.runtimeAnimatorController = player.animatorDefauld; // Trở về animator mặc định
            player.skill2Manager.isHibitedIcon = false; // cam skill icon
           
            player.ChangeState(new PlayerCurrentState(player)); // Trở về trạng thái hiện tại
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
           
        }
       

        player.wasGroundedLastFrame = player.isGrounded;

        player.velocity.y += player.gravity * Time.deltaTime;
        if(player.controller.enabled == true)
            player.controller.Move(player.velocity * Time.deltaTime);
    }
    public override void Exit()
    {
         player.skill2Manager.isHibitedIcon = false;    
        player.skill2Manager.isChangeSkill2 = false;
        isMove = true; // Đặt lại trạng thái di chuyển
        player.animator.runtimeAnimatorController = player.animatorDefauld; // Quay về bộ điều khiển hoạt hình mặc định
        player.skill2Manager.effectRun.SetActive(false);
        isChangeState = false;
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            
            float speed = 30;//tốc độ di chuyển khi sử dụng skill2

            player.animator.SetBool("Run", true);

            Vector3 camForward = player.cameraTransform.forward;
            Vector3 camRight = player.cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;
            if(player.controller.enabled == true)
            {
                player.controller.Move(moveDir * speed * Time.deltaTime);
            }

            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
        else
        {
            player.animator.SetBool("Run", false);
           
        }
    }
    // Hàm dash đến enemy gần nhất và tấn công
    void DashToNearestEnemyAndAttack()
    {
        float searchRadius = 50f;
        GameObject nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        // Tìm enemy gần nhất trong phạm vi
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, searchRadius);
        foreach (var col in colliders)
        {
            if (((1 << col.gameObject.layer) & enemyLayer) != 0)
            {
                float dist = Vector3.Distance(player.transform.position, col.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearestEnemy = col.gameObject;
                }
            }
        }

       
        // Nếu có enemy gần thì cho phép dash
        if (nearestEnemy != null && Input.GetMouseButtonDown(0) && isAttack == true && Cursor.visible == false)
        {
            // Giữ nguyên Y của player
            Vector3 playerPos = player.transform.position;
            Vector3 enemyPos = nearestEnemy.transform.position;
            enemyPos.y = playerPos.y;

            Vector3 dashDir = (enemyPos - playerPos).normalized;
            Vector3 dashTarget = enemyPos + dashDir * -3f; // Dash cách enemy một chút
            Vector3 y = player.transform.position + Vector3.up * 2f;
            // Check nếu có tường chắn giữa player và dashTarget
            LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Obstacle") | LayerMask.GetMask("Wall");
            if (!Physics.Linecast(y, dashTarget, mask)) 
            {
                isAttack = false;
                isMove = false;
                player.controller.enabled = false;

                player.StartCoroutine(DashToTarget(dashTarget, 0.3f));

                player.transform.rotation = Quaternion.LookRotation(dashDir);
                player.animator.SetTrigger("Attack");
                player.isRemoveClone = true;
                player.controller.enabled = true;
                player.StartCoroutine(WaitForChangeState());
            }         
        }


        // Nếu không có enemy thì vẫn cho chém bình thường
        if (Input.GetMouseButtonDown(0) && isAttack == true && Cursor.visible == false)
        {
            isAttack = false;
            player.animator.SetTrigger("Attack");
            player.isRemoveClone = true;
            player.StartCoroutine(WaitForChangeState());
        }
    }

    // Hàm dash đến vị trí mục tiêu
    IEnumerator DashToTarget(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = player.transform.position;
        float time = 0f;

        while (time < duration)
        {
            player.controller.enabled = false; // Tắt controller trong lúc dash
            time += Time.deltaTime;
            float t = time / duration;
            player.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        player.transform.position = targetPosition;
        player.controller.enabled = true; // Bật lại controller
    }

    //doi về trạng thái hiện tại sau khi kết thúc skill2
    public IEnumerator WaitForChangeState()
    {

        yield return new WaitForSeconds(0.8f); // Thời gian chờ trước khi chuyển về trạng thái hiện tại
        player.skill2Manager.isHibitedIcon = false; // Bỏ cấm sử dụng skill 2
        player.ChangeState(new PlayerCurrentState(player)); // Quay về trạng thái hiện tại
    }
    // Đợi 10 giây để loại bỏ phân thân
    public IEnumerator WaitForSkill2()
    {
        yield return new WaitForSeconds(player.skill2Manager.timeSkill2); // Thời gian chờ trước khi chuyển về trạng thái hiện tại
        Debug.Log("Kết thúc skill2");
        if (isChangeState == true)
        {
            player.skill2Manager.isHibitedIcon = false; // Bỏ cấm sử dụng skill 2
            player.ChangeState(new PlayerCurrentState(player)); // Quay về trạng thái hiện tại
        }
       
    }

   

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.4f); // Thời gian chờ trước khi chuyển về trạng thái hiện tại
        isMove = true; // Đặt lại trạng thái di chuyển
       
    }

}
