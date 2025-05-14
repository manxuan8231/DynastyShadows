using UnityEngine;
using System.Collections;
public class GetHitState : BaseState
{
    public GetHitState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.anim.SetTrigger("gethit");
        boss.agent.isStopped = true;
        boss.StartCoroutine(WaitAndReturn());
    }

    private IEnumerator WaitAndReturn()
    {
        yield return new WaitForSeconds(0.6f);
        if (boss.isDead) yield break;

        float dist = Vector3.Distance(boss.transform.position, boss.player.position);
        if (dist <= boss.attackRange)
            boss.TransitionToState(boss.attackState);
        else
            boss.TransitionToState(boss.chaseState);
    }

    public override void UpdateState()
    {
        Debug.Log("gethit state");
    }
}