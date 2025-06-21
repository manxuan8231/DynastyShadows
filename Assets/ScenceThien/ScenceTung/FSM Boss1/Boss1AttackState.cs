using UnityEngine;

public class Boss1AttackState : Boss1State
{
    public Boss1AttackState(Boss1Controller enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Attack State Entered");
    }

    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Attack1");
        enemy.anmt.ResetTrigger("Attack2");
        enemy.anmt.ResetTrigger("Attack3");
        enemy.anmt.ResetTrigger("Attack4");
    }

    public override void Update()
    {

       
            if (enemy.isUsingSkill) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Check if we should attack
        if (distance <= enemy.attackRange &&
            Time.time >= enemy.attackTimer + enemy.attackCooldown &&
            !enemy.isAttacking)
        {
            PerformAttack();
        }

        // State transitions
        if (distance > enemy.attackRange + 1f && !enemy.isAttacking)
        {
            enemy.isAttacking = false;
            enemy.agent.isStopped = false;
            enemy.ChangState(new Boss1RunState(enemy));
        }
        else if (distance <= enemy.skillRange &&
                 distance > enemy.attackRange &&
                 !enemy.isUsingSkill &&
                 Time.time >= enemy.skillTimer + enemy.skillCooldown)
        {
            enemy.ChangState(new SkillBossState(enemy));
        }
    }

    private void PerformAttack()
    {
        enemy.agent.isStopped = true;
        enemy.transform.LookAt(enemy.player.transform.position);
        enemy.isAttacking = true;
        enemy.attackTimer = Time.time;

        // Different attack patterns based on HP
        if (enemy.hp.currHp <= 10000)
        {
            // Phase 2 attacks (more variety)
            int random = Random.Range(0, 4);
            string[] attacks = { "Attack1", "Attack2", "Attack3", "Attack4" };
            enemy.anmt.SetTrigger(attacks[random]);
        }
        else
        {
            // Phase 1 attacks (simpler)
            int random = Random.Range(0, 2);
            string[] attacks = { "Attack1", "Attack3" };
            enemy.anmt.SetTrigger(attacks[random]);
        }
    }
}
