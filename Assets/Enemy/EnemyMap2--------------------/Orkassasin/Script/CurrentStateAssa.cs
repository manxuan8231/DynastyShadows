using Pathfinding;
using UnityEngine;

public class CurrentStateAssa : AssasinState
{
    public CurrentStateAssa(ControllerStateAssa enemy) : base(enemy) { }

    public override void Enter()
    {
       
    }
    public override void Update()
    {
       float dis = Vector3.Distance(enemy.transform.position,enemy.player.transform.position);

        if (dis < enemy.stopRange)
        {
           
            enemy.aiPath.destination = enemy.transform.position; // Dừng lại
            enemy.animator.SetBool("isRunForward", false);
            enemy.animator.SetBool("isWalkForward", false);
        }
        else if (dis < 40f)
        {
           
            enemy.aiPath.maxSpeed = 5f;
            enemy.aiPath.destination = enemy.player.transform.position;
            enemy.animator.SetBool("isRunForward", false);
            enemy.animator.SetBool("isWalkForward", true);
        }
        else // >= 40f
        {
            
            enemy.aiPath.maxSpeed = 20f;
            enemy.aiPath.destination = enemy.player.transform.position;
            enemy.animator.SetBool("isRunForward", true);
            enemy.animator.SetBool("isWalkForward", false);
        }
    }

    public override void Exit()
    {
       
    }

  

    
}
