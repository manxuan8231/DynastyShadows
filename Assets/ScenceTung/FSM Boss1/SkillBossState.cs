using System.Collections;
using UnityEngine;

public class SkillBossState : Boss1State
{
    private float maxWaitTime = 1f; // Thời gian tối đa chờ hồi skill
    private float waitTimer = 0f;

    public SkillBossState(Boss1Controller enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Skill State Entered");
        waitTimer = 0f;
    }

    public override void Update()
    {
        if (enemy.player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Nếu đang sử dụng skill, không làm gì cả
        if (enemy.isUsingSkill)
        {
            return;
        }

        // Nếu trong tầm đánh thường, ưu tiên tấn công
        if (distance <= enemy.attackRange)
        {
            enemy.ChangState(new Boss1AttackState(enemy));
            return;
        }

        // Nếu trong tầm skill và skill đã hết cooldown
        if (distance <= enemy.skillRange && 
            distance > enemy.attackRange &&
            Time.time >= enemy.skillTimer + enemy.skillCooldown)
        {
            ExecuteRandomSkill();
            waitTimer = 0f; // Reset timer khi sử dụng skill
        }
        // Nếu đang chờ hồi skill
        else if (distance <= enemy.skillRange && 
                 distance > enemy.attackRange &&
                 Time.time < enemy.skillTimer + enemy.skillCooldown)
        {
            waitTimer += Time.deltaTime;
            
            // Nếu chờ quá lâu, chuyển sang run để đuổi theo player
            if (waitTimer >= maxWaitTime)
            {
                enemy.ChangState(new Boss1RunState(enemy));
                return;
            }

            // Di chuyển để giữ khoảng cách với player
            Vector3 directionToPlayer = (enemy.player.position - enemy.transform.position).normalized;
            Vector3 targetPosition = enemy.player.position - directionToPlayer * (enemy.skillRange * 0.8f);
            enemy.agent.SetDestination(targetPosition);
        }
        // Nếu player quá xa
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
        if (enemy.hp.currHp <= 0)
        {
            enemy.skill2Prefabs.SetActive(false);
            enemy.enabled = false;
            yield break;
        }

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
