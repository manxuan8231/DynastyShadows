using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skill2State : PlayerState
{
    public Skill2State(PlayerControllerState player) : base(player) { }
    public LayerMask enemyLayer = LayerMask.GetMask("Enemy"); // Layer của enemy
    private bool isMove = false;
    public override void Enter()
    {
        player.animator.runtimeAnimatorController = player.animatorSkill2;
        player.skill2Manager.effectRun.SetActive(true);
        Debug.Log("Chạy trạng thái skill2");
      
        player.StartCoroutine(WaitForMove()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại
        player.StartCoroutine(WaitForSkill2()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại

       
    }

    public override void Update()
    {
        if (isMove == true)
        {
            Move(); 
        
        }   
        DashToNearestEnemyAndAttack();
      
    }

    public override void Exit()
    {
        player.skill2Manager.isChangeSkill2 = false;
        isMove = true; // Đặt lại trạng thái di chuyển
        player.animator.runtimeAnimatorController = player.animatorDefauld; // Quay về bộ điều khiển hoạt hình mặc định
        player.skill2Manager.effectRun.SetActive(false);

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
            player.controller.Move(moveDir * speed * Time.deltaTime);

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
        float searchRadius = 50f; // Tầm tìm enemy
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

        // Nếu có enemy gần thì mới cho bấm chuột trái để dash
        if (nearestEnemy != null && Input.GetMouseButtonDown(0))
        {
            isMove = false;
            player.controller.enabled = false; // Tắt controller để tránh va chạm

            Vector3 targetPos = nearestEnemy.transform.position;
            Vector3 dashDir = (targetPos - player.transform.position).normalized;
            
            player.StartCoroutine(DashToTarget(targetPos + dashDir * -2f, 0.3f));


            // Quay mặt về enemy
            player.transform.rotation = Quaternion.LookRotation(dashDir);

            // Chạy animation chém
            player.animator.SetTrigger("Attack");
            player.isRemoveClone = true; // Đặt cờ để loại bỏ phân thân nếu có
            player.controller.enabled = true; // Bật lại controller
            player.StartCoroutine(WaitForChangeState()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại
        }
        // Nếu ko co enemy gần thì cung cho attack
        if (Input.GetMouseButtonDown(0))
        {
           
            // Chạy animation chém
            player.animator.SetTrigger("Attack");
            player.isRemoveClone = true; // Đặt cờ để loại bỏ phân thân nếu có
          
            player.StartCoroutine(WaitForChangeState()); // Bắt đầu đợi thời gian chờ trước khi chuyển về trạng thái hiện tại
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
        player.ChangeState(new PlayerCurrentState(player)); // Quay về trạng thái hiện tại
    }
    // Đợi 10 giây để loại bỏ phân thân
    public IEnumerator WaitForSkill2()
    {
        yield return new WaitForSeconds(player.skill2Manager.timeSkill2); // Thời gian chờ trước khi chuyển về trạng thái hiện tại
        player.ChangeState(new PlayerCurrentState(player)); // Quay về trạng thái hiện tại
    }

   

    IEnumerator WaitForMove()
    {
        yield return new WaitForSeconds(0.4f); // Thời gian chờ trước khi chuyển về trạng thái hiện tại
        isMove = true; // Đặt lại trạng thái di chuyển
       
    }

}
