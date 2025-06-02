using UnityEngine;

public class Boss1AttackState : Boss1State
{
    public Boss1AttackState(Boss1Controller enemy) : base(enemy) { }

    public float attackTimer = -9f;
    public float attackCooldown = 9;
    bool isAttacking = false;
    public override void Enter()
    {
        Debug.Log("Attack");
    }

    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Attack1");
        enemy.anmt.ResetTrigger("Attack2");
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position,enemy.player.transform.position);
        if (distance <= enemy.attackRange && Time.time >= attackTimer + attackCooldown && !isAttacking)
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Attack1");
                enemy.transform.LookAt(enemy.player);
                isAttacking = true;
            }
            else
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Attack3");
                enemy.transform.LookAt(enemy.player);
                isAttacking = true;

            }

            attackTimer = Time.time;
          
            isAttacking = false;

        }
        if(enemy.hp.currHp < 10000)
        {
            if (distance <= enemy.attackRange && Time.time >= attackTimer + attackCooldown && !isAttacking)
            {
                int random = Random.Range(0, 3);
                if (random == 0)
                {
                    enemy.agent.isStopped = true;
                    enemy.anmt.SetTrigger("Attack2");
                    enemy.transform.LookAt(enemy.player);
                    isAttacking = true;
                }
                else if(random == 1)
                {
                    enemy.agent.isStopped = true;
                    enemy.anmt.SetTrigger("Attack1");
                    enemy.transform.LookAt(enemy.player);
                    isAttacking = true;
                }
                else
                {
                    enemy.agent.isStopped = true;
                    enemy.anmt.SetTrigger("Attack4");
                    enemy.transform.LookAt(enemy.player);
                    isAttacking = true;

                }

                attackTimer = Time.time;

                isAttacking = false;
            }
        }
        
        if (distance > enemy.attackRange + 1f && !isAttacking)
        {
            enemy.ChangState(new Boss1RunState(enemy));
        }
    }
}
