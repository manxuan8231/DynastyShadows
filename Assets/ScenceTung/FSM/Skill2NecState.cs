using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Skill2NecState : INecState
{
    public Skill2NecState(NecController enemy) : base(enemy) { }
    public float skillTimer = -18;
    public float skillCooldown = 18;
    public override void Enter()
    {

    }

    public override void Update()
    {  
        if (enemy.isSkill2 == true)
        {
            if (enemy.checkEnemyCount <= 10 && Time.time >= skillTimer + skillCooldown)
            {  
                enemy.anmt.SetTrigger("Skill2");
                
                skillTimer = Time.time;
            }
            else
            {
                enemy.isSkill2 = false;
            }
        }   

    

    }
    public override void Exit()
    {
        
    }

   


   
}
