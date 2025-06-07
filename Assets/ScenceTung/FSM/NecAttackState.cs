using UnityEngine;

public class NecAttackState : INecState
{
    public NecAttackState(NecController enemy) : base(enemy) { }

    public float attackTimer = -7;
    public float attackCooldown = 7;
   
    public override void Enter()
    {
        Debug.Log("Đang đấm chết mẹ m");
        Debug.Log("Thời gian chuẩn bị đòn đánh");
     

    }
    public override void Update()
    {
       
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        
        if (distance <= enemy.attackRange && Time.time >= attackTimer + attackCooldown )
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                enemy.agent.isStopped = true;
                enemy.anmt.SetTrigger("Attack1");
                enemy.transform.LookAt(enemy.player);

            }
            else
            {
                enemy.agent.isStopped = true;

                enemy.anmt.SetTrigger("Attack2");
                enemy.transform.LookAt(enemy.player);

            }

            attackTimer = Time.time;


        }
       else if (distance > enemy.attackRange + 1f)
        {
           
            enemy.ChangState(new NecWalkState(enemy));
            
        }
        if (enemy.necHp.curhp <= 7000)
        {
            enemy.ChangState(new Skill1NecState(enemy));
        }
       
    }
    public override void Exit()
    {

    }
 
}
