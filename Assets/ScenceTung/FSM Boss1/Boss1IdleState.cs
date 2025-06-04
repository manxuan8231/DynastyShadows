using Unity.VisualScripting;
using UnityEngine;

public class Boss1IdleState : Boss1State
{
    public Boss1IdleState(Boss1Controller enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Idle State Entered");
        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        // Clean up idle state
    }

    public override void Update()
    {
       
        if (enemy.player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);

        // Player detected within radius - start chasing
        if (distance <= enemy.radius)
        {
            enemy.ChangState(new Boss1RunState(enemy));
        }
    }
}
