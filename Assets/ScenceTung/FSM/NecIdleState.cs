using UnityEngine;

public class NecIdleState : INecState
{
    public NecIdleState(NecController enemy): base(enemy) { }


    public override void Enter()
    {

    }

    public override void Update()
    {

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if (distance <= enemy.radius) 
        { 
            
        enemy.ChangState(new NecWalkState(enemy));
    
        }
        enemy.anmt.SetTrigger("Idle");
        if (enemy.necHp.curhp <= 350)
        {
            enemy.ChangState(new Skill1NecState(enemy));
        }

    }

    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Idle"); 
    }

}
