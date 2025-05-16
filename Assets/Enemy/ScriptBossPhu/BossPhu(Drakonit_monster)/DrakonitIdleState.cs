using UnityEngine;

public class DrakonitIdleState : DrakonitState
{
    public DrakonitIdleState(DrakonitController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.animator.Play("Idle");
        enemy.agent.ResetPath();
        Debug.Log("trang thai idle");
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance < enemy.chaseRange)
        {
            enemy.ChangeState(new DrakonitChaseState(enemy));
        }
    }

    public override void Exit() { }
}
