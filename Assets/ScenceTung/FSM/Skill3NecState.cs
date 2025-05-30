using UnityEngine;

public class Skill3NecState : INecState
{
    public Skill3NecState(NecController enemy): base(enemy) { }
    public float skill3Timer = -7;
    public float skill3Cooldown = 7;
    public override void Enter()
    {
       
    }
    public override void Update()
    {
       
        if (enemy.isSkill1 == true)
        {
            if (Time.time >= skill3Timer + skill3Cooldown)
            {
                enemy.anmt.SetTrigger("Skill2");
                enemy.transform.LookAt(enemy.player);
                skill3Timer = Time.time;
            }
        }

      
    }
    public override void Exit()
    {
       
    }


}
