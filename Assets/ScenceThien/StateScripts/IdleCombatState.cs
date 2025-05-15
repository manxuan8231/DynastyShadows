using UnityEngine;

public class IdleCombatState : BaseState
{
    public IdleCombatState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.ResetAllTriggers(); // xoá trigger cũ để tránh bug
        boss.anim.SetTrigger("IdleCombat");
    }

    public override void UpdateState()
    {
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);

        if (dist <= boss.attackRange)
        {
            boss.TransitionToState(boss.attackState);
        }
        else if (dist <= boss.attackRange + 5f) // phạm vi chase rộng hơn
        {
            boss.agent.isStopped = false;
            boss.TransitionToState(boss.chaseState);
        }
    }

    public override void ExitState()
    {
        boss.anim.ResetAllTriggers(); // tránh bị kẹt trigger khi ra khỏi Idle
    }
}