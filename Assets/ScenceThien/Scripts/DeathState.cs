using UnityEngine;

public class DeathState : BaseState
{
    public DeathState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.isDead = true;
        boss.anim.SetTrigger("death");
        boss.agent.isStopped = true;
        GameObject.Destroy(boss.gameObject, 5f);
    }

    public override void UpdateState()
    {
        Debug.Log("death");
    }
}