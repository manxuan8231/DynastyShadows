using System.Collections;
using UnityEngine;

public class PhaseChangeState: BaseState
{
    public PhaseChangeState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.SetTrigger("phase2");
        boss.StartCoroutine(EnterPhase2());
    }

    private IEnumerator EnterPhase2()
    {
        yield return new WaitForSeconds(2f);
        boss.TransitionToState(boss.chaseState);
    }
}