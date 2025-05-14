using UnityEngine;
using System.Collections;
public class PhaseChangeState : BaseState
{
    public PhaseChangeState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.anim.SetTrigger("phaseChange"); 
        boss.agent.isStopped = true;
        boss.StartCoroutine(WaitToChangePhase());
    }

    private IEnumerator WaitToChangePhase()
    {
        yield return new WaitForSeconds(2f);
        boss.isPhase2 = true;
        boss.TransitionToState(boss.chaseState);
    }
}