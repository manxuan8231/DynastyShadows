using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        boss.anim.SetTrigger("death");
        boss.isDead = true;
        GameObject.Destroy(boss.gameObject, 5f);
    }
}
