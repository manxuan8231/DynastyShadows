using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.SetTrigger("Idle");
    }

    public override void UpdateState()
    {
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);
        if (dist <= boss.attackRange + 1.5f)
        {
            boss.TransitionToState(boss.chaseState);
        }
    }
}
