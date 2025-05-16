using UnityEngine;

public class ChaseState : BaseState
{
    public ChaseState(BossScript boss) : base(boss) { }
    public AudioManager ChaseAudio;
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
            
        }
    }
}
