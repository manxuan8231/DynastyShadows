using System.Collections;
using UnityEngine;

// Trạng thái Drakonit đang rượt đuổi Player
public class DrakonitChaseState : DrakonitState
{
    private float lastWalkTime = 0f; // Thời gian gần nhất Drakonit đi bộ
   
    //thoi gian cho
    private float exitSkillTime = -1f;//thời gian cho khi pl ra khỏi tầm đánh
    private float exitRunTime = -1f;//thời gian cho khi pl ra khỏi tầm đánh
   

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
        if (distance <= enemy.attackRange)
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
            if (Time.time >= exitSkillTime + 7f)
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
        
        if(Time.time >= lastWalkTime + 5f && enemy.isWalking)
        {
            int randomWalk = Random.Range(0, 3);
            if (randomWalk == 0)
            {
                enemy.agent.SetDestination(enemy.player.position);// Di chuyển đến vị trí của Player
                enemy.animator.SetBool("WalkingLeft", false);
                enemy.animator.SetBool("WalkingRight", false);
                enemy.animator.SetBool("Walking", true);
            }
            else if (randomWalk == 1)
            {
                enemy.animator.SetBool("WalkingLeft", true);
                enemy.animator.SetBool("WalkingRight", false);
                enemy.animator.SetBool("Walking", false);
                // Tính hướng trái theo enemy (dựa vào transform.right)
                Vector3 leftDirection = -enemy.transform.right;

                // Xác định vị trí mới cách vị trí hiện tại một khoảng sang trái
                Vector3 targetPosition = enemy.transform.position + leftDirection * 30f;

                // Đặt điểm đích cho NavMeshAgent
                enemy.agent.SetDestination(targetPosition);
            }
            else if (randomWalk == 2)
            {
                enemy.animator.SetBool("WalkingLeft", false);
                enemy.animator.SetBool("WalkingRight", true);
                enemy.animator.SetBool("Walking", false);
                // Tính hướng phải theo enemy (dựa vào transform.right)
                Vector3 rightDirection = enemy.transform.right;
                // Xác định vị trí mới cách vị trí hiện tại một khoảng sang phải
                Vector3 targetPosition = enemy.transform.position + rightDirection * 30f;
                // Đặt điểm đích cho NavMeshAgent
                enemy.agent.SetDestination(targetPosition);
            }
                lastWalkTime = Time.time;
        }

        //chạy tới player khi ở xa quá lâu 
        if(distance <= enemy.skillRange && enemy.isRunning)
        {
            if(exitRunTime < 0f)
            {
                exitRunTime = Time.time;
            }
            if (Time.time >= exitRunTime + 10f)//đợi 30f thì chạy tới player
            {
                enemy.StartCoroutine(WaitRun());
            }
        }
       

        enemy.transform.LookAt(enemy.player);// quay mat ve player
    }

    // Hàm gọi khi rời khỏi trạng thái này
    public override void Exit()
    {
        enemy.animator.SetBool("Walking", false);
        enemy.animator.SetBool("WalkingLeft", false);
        enemy.animator.SetBool("WalkingRight", false);
        // Ngừng đường di chuyển hiện tại
        enemy.agent.ResetPath();
    }

    private IEnumerator WaitRun()
    {
        enemy.isWalking = false;
        enemy.isSkill = false;
        enemy.animator.SetBool("Run", true);
        enemy.agent.SetDestination(enemy.player.position);
        enemy.agent.speed = 20f;
        yield return new WaitForSeconds(3f);

        enemy.agent.ResetPath();
        enemy.isRunning = false;
        enemy.animator.SetBool("Run", false);
        enemy.agent.speed = 3.5f;
        enemy.isWalking = true;
        enemy.isSkill = true;
        enemy.animator.SetBool("Walking", false);
        enemy.animator.SetBool("WalkingLeft", false);
        enemy.animator.SetBool("WalkingRight", false);

    }

}