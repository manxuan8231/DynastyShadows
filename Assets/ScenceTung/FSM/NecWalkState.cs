using UnityEngine;

public class NecWalkState : INecState
{
    public NecWalkState(NecController enemy) : base(enemy) { }


    public override void Enter()
    {

    }

    public override void Update()
    {


        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance < enemy.radius)
        {
            enemy.agent.SetDestination(enemy.player.transform.position);
            enemy.anmt.SetTrigger("Walk");
        }
        //khoảng cách để nó đấm lộn 

        if (distance <= enemy.attackRange)
        {
            enemy.ChangState(new NecAttackState(enemy));

        }

        if (enemy.necHp.curhp <= 7000)
        {
            enemy.ChangState(new Skill1NecState(enemy));
        }
       
    }

    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Walk");
    }

}
