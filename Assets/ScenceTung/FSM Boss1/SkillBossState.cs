using Mono.Cecil.Cil;
using System.Collections;
using UnityEngine;

public class SkillBossState : Boss1State
{
    public SkillBossState(Boss1Controller enemy) : base(enemy) { }
  

    public override void Enter()
    {
        Debug.Log("Đã vào skil");
    }
    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position,enemy.player.transform.position);
        if(distance <= enemy.skillRange && distance > enemy.attackRange && Time.time >= enemy.skillTimer + enemy.skillCooldown && enemy.isUsingSkill == false)
        {
            enemy.isUsingSkill = true;
            int random = Random.Range(0,4);
            if (random == 0)
            {
                enemy.agent.isStopped = true;
                Debug.Log("Skill1");
                enemy.StartCoroutine(SKill1Manager());
            }
            else if(random == 1)
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Skill2");
            }
            else if (random == 2)
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Skill3");
                enemy.isUsingSkill = false;
                Debug.Log("Skill3");
            }
            else if (random == 3)
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Skill4");
                enemy.isUsingSkill = false;
                Debug.Log("Skill4");

            }
          
           
            enemy.skillTimer = Time.time;
            enemy.agent.isStopped = false ;
        }
       else if (distance <= enemy.attackRange && enemy.isAttacking == false)
        {
            enemy.ChangState(new Boss1AttackState(enemy));
        }
        else
        {
            enemy.ChangState(new Boss1RunState(enemy));
        }

        
    }

    
    public override void Exit()
    {

    }

    IEnumerator SKill1Manager()
    {
      
      enemy.anmt.SetTrigger("Skill1");
      enemy.skill1Prefabs.SetActive(true);
       enemy.transform.LookAt(enemy.player.transform.position);
        yield return new WaitForSeconds(5f);
       enemy.skill1Prefabs.SetActive(false);
       enemy.anmt.ResetTrigger("Skill1");
        enemy.isUsingSkill = false;
    }

}
