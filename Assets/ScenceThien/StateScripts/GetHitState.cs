using UnityEngine;
using System.Collections;
using System.Collections;
using UnityEngine;

public class GetHitState : BaseState
{
    public GetHitState(BossScript boss) : base(boss) { }
    public AudioBossManager audioBoss;

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.SetTrigger(boss.animationData.gethit);
        boss.StartCoroutine(ReturnToFight());
    }

    private IEnumerator ReturnToFight()
    {
        AudioBossManager.instance.PlaySFX("GetHit");
        yield return new WaitForSeconds(0.5f);

        if (boss.isDead) yield break;

        float dist = Vector3.Distance(boss.transform.position, boss.player.position);
        if (dist <= boss.attackRange)
            boss.TransitionToState(boss.attackState);
        else
            boss.TransitionToState(boss.chaseState);
    }
}
