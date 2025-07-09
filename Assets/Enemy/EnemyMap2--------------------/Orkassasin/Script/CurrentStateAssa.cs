using UnityEngine;

public class CurrentStateAssa : AssasinState
{
    public CurrentStateAssa(ControllerStateAssa enemy) : base(enemy) { }

    public override void Enter()
    {
       
    }
    public override void Update()
    {
        if (enemy.distancePLAndEnemy <= 100f) 
        {
           
            enemy.animator.SetBool("isWalkForward", true);
            enemy.aiPath.destination = enemy.player.transform.position;
        }
    }
    public override void Exit()
    {
       
    }

  

    
}
