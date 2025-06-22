using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = false;

        boss.anim.ResetAllTriggers();
        boss.anim.SetTrigger(boss.animationData.chase);
    }

    public override void UpdateState()
    {
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);

        // Di chuyển đến player
        boss.agent.SetDestination(boss.player.position);

        // Nếu gần đủ để tấn công
        if (dist <= boss.attackRange)
        {
            boss.agent.isStopped = true;
            boss.TransitionToState(boss.attackState);
        }
    }

    public override void ExitState()
    {

    }
}
