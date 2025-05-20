using System.Collections;
using UnityEngine;

// Trạng thái Drakonit đang rượt đuổi Player
public class DrakonitChaseState : DrakonitState
{
    private float lastWalkTime = 0f; // Thời gian gần nhất Drakonit đi bộ
   
    //thoi gian cho
    private float exitSkillTime = -1f;//thời gian cho khi pl ra khỏi tầm đánh

    // Hàm khởi tạo: nhận vào controller của Drakonit
    public DrakonitChaseState(DrakonitController enemy) : base(enemy) { }

    // Hàm gọi khi mới vào trạng thái này
    public override void Enter()
    {
        enemy.animator.SetBool("Walking", true);  // Phát animation chạy
        enemy.agent.isStopped = false;   // Cho phép agent di chuyển
        enemy.slider.SetActive(true); // bat thanh máu
        Debug.Log("trang thai ruot duoii");
    }

    // Hàm cập nhật liên tục khi đang ở trạng thái Chase
    public override void Update()
    {
        // Tính khoảng cách giữa Drakonit và Player
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // Nếu đã đến gần trong khoảng cách tấn công -> chuyển sang trạng thái Attack
        if (distance <= enemy.attackRange && enemy.isAttack)
        {
          
            enemy.ChangeState(new DrakonitAttackState(enemy));
            return;
        }

        // Nếu đã đến gần trong khoảng cách skill -> chuyển sang trạng thái skill
        if (distance <= enemy.skillRange && enemy.isSkill)
        {
            if (exitSkillTime < 0f)
            {
                exitSkillTime = Time.time;
            }
            if (Time.time >= exitSkillTime + 5f)
            {               
                enemy.ChangeState(new DrakonitSkillState(enemy));
                return;
            }
        }


        // Nếu Player đã chạy xa vượt khỏi tầm rượt đuổi -> chuyển sang idle
        if (distance > enemy.chaseRange)
        {
            enemy.ChangeState(new DrakonitIdleState(enemy));
            return;
        }

        //target toi player walk random
        
        if(Time.time >= lastWalkTime + 6f)
        {
            int randomWalk = Random.Range(0, 4);
            if (randomWalk == 0)
            {
                enemy.agent.speed = 4f; // Tăng tốc độ di chuyển của enemy             
                enemy.StartCoroutine(WaitRunAndWalk()); // Di chuyển đến vị trí của Player
                enemy.animator.SetBool("WalkingLeft", false);
                enemy.animator.SetBool("WalkingRight", false);
                enemy.animator.SetBool("Walking", true);
                enemy.animator.SetBool("Run", false);
            }
           
            else if (randomWalk == 1)
            {
                enemy.animator.SetBool("WalkingLeft", true);
                enemy.animator.SetBool("WalkingRight", false);
                enemy.animator.SetBool("Walking", false);
                enemy.animator.SetBool("Run", false);
              
                enemy.StartCoroutine(WaitLeftWalk()); // Di chuyển sang trái
                enemy.agent.speed = 3f; // Tăng tốc độ di chuyển của enemy
               
            }
            else if (randomWalk == 2)
            {
                enemy.animator.SetBool("WalkingLeft", false);
                enemy.animator.SetBool("WalkingRight", true);
                enemy.animator.SetBool("Walking", false);
                enemy.animator.SetBool("Run", false);
               
                enemy.StartCoroutine(WaitRightWalk()); // Di chuyển sang phải
                enemy.agent.speed = 3f; // Tăng tốc độ di chuyển của enemy
               
            }
            else if(randomWalk == 3)
            {
                enemy.animator.SetBool("WalkingLeft", false);
                enemy.animator.SetBool("WalkingRight", false);
                enemy.animator.SetBool("Walking", false);
                enemy.animator.SetBool("Run", true);
               
                enemy.agent.speed = 10f; // Tăng tốc độ di chuyển của enemy
                                        
                enemy.StartCoroutine(WaitRunAndWalk()); // Di chuyển đến vị trí của Player
            }
                lastWalkTime = Time.time;
        }
        if (enemy.isRunning)// Luôn cập nhật vị trí player liên tục mỗi frame walk and run
        { 
            enemy.agent.SetDestination(enemy.player.position);
        }    
        if(enemy.isWalkLeft)
        {
            // Tính hướng trái theo enemy (dựa vào transform.right)
            Vector3 leftDirection = -enemy.transform.right;

            // Xác định vị trí mới cách vị trí hiện tại một khoảng sang trái
            Vector3 targetPosition = enemy.transform.position + leftDirection * 30f;
            // Đặt điểm đích cho NavMeshAgent
            enemy.agent.SetDestination(targetPosition);
        }
        if (enemy.isWalkRight)
        {
            // Tính hướng phải theo enemy (dựa vào transform.right)
            Vector3 rightDirection = enemy.transform.right;
            // Xác định vị trí mới cách vị trí hiện tại một khoảng sang phải
            Vector3 targetPosition = enemy.transform.position + rightDirection * 30f;
            // Đặt điểm đích cho NavMeshAgent
            enemy.agent.SetDestination(targetPosition);
        }

        enemy.transform.LookAt(enemy.player);// quay mat ve player lien tuuc
    }

    // Hàm gọi khi rời khỏi trạng thái này
    public override void Exit()
    {
       enemy.agent.isStopped = true; // Dừng agent
        enemy.animator.SetBool("Walking", false); // Dừng animation đi bộ
        enemy.animator.SetBool("WalkingLeft", false); // Dừng animation đi bộ
        enemy.animator.SetBool("WalkingRight", false); // Dừng animation đi bộ
        enemy.animator.SetBool("Run", false); // Dừng animation đi bộ

    }

   private IEnumerator WaitRunAndWalk()
    {
       enemy.isWalkRight = false;
        enemy.isWalkLeft = false;
        enemy.isRunning = true; // Đặt trạng thái isRunning về true
        yield return new  WaitForSeconds(7f);
        enemy.isRunning = false; // Đặt lại trạng thái isRunning về false
      
    }
    private IEnumerator WaitLeftWalk()
    {
        enemy.isRunning = false;
        enemy.isWalkRight = false;
        enemy.isWalkLeft = true; // Đặt trạng thái isRunning về true
        yield return new WaitForSeconds(7f);
        enemy.isWalkLeft = false; // Đặt lại trạng thái isRunning về false
      
    }
    private IEnumerator WaitRightWalk()
    {
       enemy.isRunning = false;
        enemy.isWalkLeft = false;
        enemy.isWalkRight = true; // Đặt trạng thái isRunning về true
        yield return new WaitForSeconds(7f);
        enemy.isWalkRight = false; // Đặt lại trạng thái isRunning về false
      
    
    }
}