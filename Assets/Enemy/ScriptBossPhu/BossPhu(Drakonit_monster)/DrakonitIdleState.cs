using UnityEngine;

public class DrakonitIdleState : DrakonitState
{
    public DrakonitIdleState(DrakonitController enemy) : base(enemy) { }

    private float exitChase = -1f; // Thời gian gần nhất Drakonit đi bộ
    public override void Enter()
    {
        enemy.animator.Play("Idle");
        enemy.agent.ResetPath();
        Debug.Log("trang thai idle");
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        
        if (distance < enemy.chaseRange )
        {
            if(exitChase < 0f)
            {
                exitChase = Time.time;
            }
            if(Time.time >= exitChase + 2f)
            {
                enemy.ChangeState(new DrakonitChaseState(enemy));
                return;
            }
           
        }
    }

    public override void Exit() { }
}
