using Unity.VisualScripting;
using UnityEngine;

public class Boss1IdleState : Boss1State
{
    public Boss1IdleState(Boss1Controller enemy) : base(enemy) { }
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
        if(distance <= enemy.radius)
        {
           enemy.ChangState(new Boss1RunState(enemy));
        }
        else if(distance > enemy.attackRange)
        {
            enemy.ChangState(new Boss1AttackState(enemy));
        }
    }
}
