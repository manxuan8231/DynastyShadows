using UnityEngine;

public class Boss1RunState : Boss1State
{
    public Boss1RunState(Boss1Controller enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Run State Entered");
        enemy.anmt.SetTrigger("Run");
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        if (enemy.player == null) return;
      
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Continue chasing if within radius
        if (distance <= enemy.radius)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
        }

        // State transitions (priority order matters!)
        if (distance <= enemy.attackRange)
        {
            enemy.ChangState(new Boss1AttackState(enemy));
        }
        else if (distance <= enemy.skillRange &&
                 distance > enemy.attackRange &&
                 Time.time >= enemy.skillTimer + enemy.skillCooldown &&
                 !enemy.isUsingSkill)
        {
            enemy.ChangState(new SkillBossState(enemy));
        }
        else if (distance > enemy.radius)
        {
            // Player too far away - return to idle
            enemy.ChangState(new Boss1IdleState(enemy));
        }
    }

    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Run");
    }


}
