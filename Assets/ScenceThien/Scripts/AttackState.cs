using UnityEngine;

public class AttackState : BaseState
{
    private float cooldownTimer;

    public AttackState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        cooldownTimer = 0f;
        boss.agent.isStopped = true;
    }

    public override void UpdateState()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= boss.attackCooldown)
        {
            string atk = boss.GetRandomAttack();
            boss.anim.SetTrigger(atk);
            cooldownTimer = 0f;
            Debug.Log("attack state");
        }

        float dist = Vector3.Distance(boss.transform.position, boss.player.position);
        if (dist > boss.attackRange + 1f)
        {
            boss.agent.isStopped = false;
            boss.TransitionToState(boss.chaseState);
            Debug.Log("Chase state");
        }
    }
}