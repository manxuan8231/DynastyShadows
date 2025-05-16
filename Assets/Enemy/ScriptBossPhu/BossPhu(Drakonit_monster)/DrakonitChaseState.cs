using System.Collections;
using UnityEngine;

// Trạng thái Drakonit đang rượt đuổi Player
public class DrakonitChaseState : DrakonitState
{
    // Hàm khởi tạo: nhận vào controller của Drakonit
    public DrakonitChaseState(DrakonitController enemy) : base(enemy) { }

    // Hàm gọi khi mới vào trạng thái này
    public override void Enter()
    {
        enemy.animator.SetBool("Walking", true);  // Phát animation chạy
        enemy.agent.isStopped = false;   // Cho phép agent di chuyển
        enemy.slider.SetActive(true); // Ẩn thanh máu
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
            enemy.ChangeState(new DrakonitSkillState(enemy));
            return;
        }

        // Nếu Player đã chạy xa vượt khỏi tầm rượt đuổi -> chuyển sang idle
        if (distance > enemy.chaseRange)
        {
            enemy.ChangeState(new DrakonitIdleState(enemy));
            return;
        }

        // Nếu vẫn đang trong tầm rượt, tiếp tục di chuyển đến vị trí Player
        enemy.agent.SetDestination(enemy.player.position);
    }

    // Hàm gọi khi rời khỏi trạng thái này
    public override void Exit()
    {
        enemy.animator.SetBool("Walking", false); 
        // Ngừng đường di chuyển hiện tại
        enemy.agent.ResetPath();
    }

}
