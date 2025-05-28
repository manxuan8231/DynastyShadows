using System.Collections;
using UnityEngine;

public class PhaseChangeState : BaseState
{
    public PhaseChangeState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.SetTrigger(boss.animationData.phaseChangeState);
        AudioBossManager.instance?.PlaySFX("Roar");
        boss.StartCoroutine(EnterPhase2());
    }

    private IEnumerator EnterPhase2()
    {
        yield return new WaitForSeconds(2f);
        boss.agent.isStopped = false;
        boss.TransitionToState(boss.chaseState);
    }

    public override void UpdateState()
    {

    }

    public override void ExitState()
    {

    }
}