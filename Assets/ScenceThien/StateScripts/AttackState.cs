using UnityEngine;

public class AttackState : BaseState
{
    private float attackCooldown = 2f;
    private float attackTimer = 0f;
    private AudioBossManager audioBossManager;

    private string[] attackClips = { "Attack 1", "Attack 2", "Attack 3" };

    public AttackState(BossScript boss) : base(boss) { }

    public override void EnterState()
    {
        boss.agent.isStopped = true;
        attackTimer = attackCooldown;
    }

    public override void UpdateState()
    {
        float dist = Vector3.Distance(boss.transform.position, boss.player.position);

        if (dist > boss.attackRange + 1f)
        {
            boss.agent.isStopped = false;
            boss.TransitionToState(boss.chaseState);
            return;
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackCooldown)
        {
            string clip = attackClips[Random.Range(0, attackClips.Length)];
            boss.anim.SetTrigger(clip);
            attackTimer = 0f;
            audioBossManager.instance.PlaySFX("Attack");
        }
    }
}
