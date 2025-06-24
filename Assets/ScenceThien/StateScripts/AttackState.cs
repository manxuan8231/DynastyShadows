using UnityEngine;
using System.Collections;
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
            string attackAnim = boss.isPhase2
                ? boss.animationData.attackPhase2[Random.Range(0, boss.animationData.attackPhase2.Length)]
                : boss.animationData.attackPhase1[Random.Range(0, boss.animationData.attackPhase1.Length)];

            boss.anim.SetTrigger(attackAnim);
            AudioBossManager.instance?.PlaySFX("Attack");

            // Bật collider đúng frame impact
            boss.StartCoroutine(EnableColliderForHit(0.4f, 0.2f)); // ví dụ bật sau 0.4s, tắt sau 0.2s

            attackTimer = 0f;
        }
    }

    private IEnumerator EnableColliderForHit(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);

        boss.attackCollider.SetActive(true);  // bật

        yield return new WaitForSeconds(duration);

        boss.attackCollider.SetActive(false); // tắt
    }

}