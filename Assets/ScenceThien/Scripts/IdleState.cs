using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.anim.SetTrigger("Idle");
        boss.agent.isStopped = true;
    }

    public override void UpdateState()
    {
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);
        if (dist < boss.detectionRange)
        {
            boss.TransitionToState(boss.chaseState);
            Debug.Log("run state");
        }
    }
}