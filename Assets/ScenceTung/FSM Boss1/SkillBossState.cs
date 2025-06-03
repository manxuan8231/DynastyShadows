using System.Collections;
using UnityEngine;

public class SkillBossState : Boss1State
{
    public SkillBossState(Boss1Controller enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Skill State Entered");
    }

    public override void Update()
    {
        if (enemy.player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Execute skill if conditions are met
        if (distance <= enemy.skillRange &&
            distance > enemy.attackRange &&
            Time.time >= enemy.skillTimer + enemy.skillCooldown &&
            !enemy.isUsingSkill)
        {
            ExecuteRandomSkill();
        }
        // State transitions
        else if (distance <= enemy.attackRange && !enemy.isAttacking)
        {
            enemy.ChangState(new Boss1AttackState(enemy));
        }
        else if (distance > enemy.skillRange)
        {
            enemy.ChangState(new Boss1RunState(enemy));
        }
    }

    private void ExecuteRandomSkill()
    {
        enemy.isUsingSkill = true;
        enemy.agent.isStopped = true;
        enemy.skillTimer = Time.time;

        int random = Random.Range(0, 4);

        switch (random)
        {
            case 0:
                Debug.Log("Executing Skill1");
                enemy.StartCoroutine(Skill1Manager());
                break;
            case 1:
                Debug.Log("Executing Skill2");
                enemy.anmt.SetTrigger("Skill2");
                break;
            case 2:
                Debug.Log("Executing Skill3");
                enemy.anmt.SetTrigger("Skill3");
                break;
            case 3:
                Debug.Log("Executing Skill4");
                enemy.anmt.SetTrigger("Skill4");
                break;
        }
    }

    public override void Exit()
    {
        // Reset any skill triggers
        enemy.anmt.ResetTrigger("Skill1");
        enemy.anmt.ResetTrigger("Skill2");
        enemy.anmt.ResetTrigger("Skill3");
        enemy.anmt.ResetTrigger("Skill4");
    }

    private IEnumerator Skill1Manager()
    {
        enemy.anmt.SetTrigger("Skill1");

        if (enemy.skill1Prefabs != null)
        {
            enemy.skill1Prefabs.SetActive(true);
        }

        enemy.transform.LookAt(enemy.player.transform.position);

        yield return new WaitForSeconds(5.5f);

        if (enemy.skill1Prefabs != null)
        {
            enemy.skill1Prefabs.SetActive(false);
        }

        enemy.anmt.ResetTrigger("Skill1");
        enemy.isUsingSkill = false;
        enemy.agent.isStopped = false;
    }
}
