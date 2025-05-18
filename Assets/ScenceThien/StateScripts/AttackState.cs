using UnityEngine;

public class AttackState : BaseState
{
    private float attackCooldown = 2f;
    private float attackTimer = 0f;

    private string[] phase1Attacks = { "Attack 1", "Attack 2", "Attack 3" };
    private string[] phase2Attacks = { "Attack 4", "Attack 5" };

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
            // Chọn đòn tấn công phù hợp với Phase
            string[] attackPool = boss.isPhase2 ? phase2Attacks : phase1Attacks;
            string clip = attackPool[Random.Range(0, attackPool.Length)];

            boss.anim.SetTrigger(clip);
            attackTimer = 0f;

            AudioBossManager.instance?.PlaySFX("Attack"); // hoặc dùng clip nếu âm thanh tách riêng
        }
    }
}