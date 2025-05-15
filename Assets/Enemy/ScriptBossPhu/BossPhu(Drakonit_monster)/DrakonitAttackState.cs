using UnityEngine;

// Lớp trạng thái "Tấn công" kế thừa từ DrakonitState
public class DrakonitAttackState : DrakonitState
{
    // Thời gian chờ giữa mỗi lần tấn công
    private float attackCooldown = 3f;
    // Thời điểm lần tấn công gần nhất
    private float lastAttackTime;
    //thoi gian cho
    private float exitAttackTime = -1f;//thời gian cho khi pl ra khỏi tầm đánh
    private float exitFlipTime = -1f;//thời gian chờ khi xoay mặt

    // Constructor: nhận controller của enemy (DrakonitController)
    public DrakonitAttackState(DrakonitController enemy) : base(enemy) { }

    // Khi vào trạng thái tấn công
    public override void Enter()
    {
        enemy.agent.isStopped = true; // Dừng di chuyển khi tấn công
        
    }

    // Hàm Update sẽ được gọi mỗi frame
    public override void Update()
    {
        // Tính khoảng cách giữa enemy và người chơi
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);

        // Nếu người chơi đi xa hơn tầm đánh, chuyển sang trạng thái đuổi theo
        if (distance > enemy.attackRange)
        {       
            // Nếu chưa bắt đầu đếm thời gian
            if (exitAttackTime < 0f)
            {
                exitAttackTime = Time.time;
            }
            // Nếu đã vượt quá 2 giây kể từ khi ra khỏi tầm đánh
            if (Time.time >= exitAttackTime + 2f)
            {
                enemy.ChangeState(new DrakonitSkillState(enemy));
                return;
            }
                
        }
        // Kiểm tra nếu đủ thời gian để tấn công tiếp
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Gọi animation tấn công
            float randomAttack = Random.Range(0, 2);
            if (randomAttack == 0)
            {
                Debug.Log("Attack1");
                enemy.animator.SetTrigger("Attack1");
                enemy.transform.LookAt(enemy.player); // Quay mặt về phía người chơi
            }
            else if (randomAttack == 1) 
            {
                Debug.Log("Attack2");
                enemy.animator.SetTrigger("Attack2");
                enemy.transform.LookAt(enemy.player);
            }
            else if(randomAttack == 2)
            {
                Debug.Log("Attack3");
                enemy.animator.SetTrigger("Attack3");
                enemy.transform.LookAt(enemy.player);
            }
            lastAttackTime = Time.time; // Cập nhật lại thời gian tấn công

            // Gây sát thương cho player nếu có component PlayerStatus
           // enemy.player.GetComponent<PlayerStatus>()?.TakeHealth(enemy.damage);
        }
    }

    // Khi rời khỏi trạng thái này
    public override void Exit()
    {
        enemy.agent.isStopped = false; // Cho phép di chuyển trở lại
    }
}
