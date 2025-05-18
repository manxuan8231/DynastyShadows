using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(BossScript boss) : base(boss) { }
    private AudioBossManager audioBossManager;
    public override void EnterState()
    {
        boss.anim.SetTrigger("chase");
        boss.agent.isStopped = false;
    }

    public override void UpdateState()
    {
        boss.agent.SetDestination(boss.player.position);
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);

        if (dist <= boss.attackRange)
        {
            boss.TransitionToState(boss.idleCombatState);
            AudioBossManager.instance.PlaySFX("Chase");
        }
    }
}
