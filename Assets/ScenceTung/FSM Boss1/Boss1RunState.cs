using UnityEngine;

public class Boss1RunState : Boss1State
{
    public Boss1RunState(Boss1Controller enemy) : base(enemy) { }
 

    public override void Enter()
    {
        Debug.Log("Run");
    }
    public override void Update()
    {
        
        float distance = Vector3.Distance(enemy.transform.position,enemy.player.transform.position);
        if(distance <= enemy.radius)
        {

            enemy.anmt.SetTrigger("Run");
            enemy.agent.SetDestination(enemy.player.transform.position);
            enemy.agent.isStopped = false;

        }
        if (distance <= enemy.attackRange)
        {
            enemy.agent.isStopped = true;

            enemy.ChangState(new Boss1AttackState(enemy));
        }
        
    }
    public override void Exit()
    {
        enemy.anmt.ResetTrigger("Run");
    }

   
}
