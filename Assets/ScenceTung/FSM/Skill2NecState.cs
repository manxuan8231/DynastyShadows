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
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
            if (enemy.checkEnemyCount <= 10 && Time.time >= skillTimer + skillCooldown && distance <= 100)
            {  
                enemy.anmt.SetTrigger("Skill2");
                enemy.transform.LookAt(enemy.player);
                skillTimer = Time.time;
            }
        
        }

        if (enemy.checkEnemyCount >= 8)
        {
            enemy.isSkill2 = false;
            enemy.isSkill1 = true;
            if (enemy.isSkill2 == false)
            {
                enemy.StartCoroutine(OnNavMesh());
            }
        }
        if (enemy.isSkill1 == true && enemy.isSKill3 == true)
        {
            enemy.ChangState(new Skill3NecState(enemy));
        }
        

    }
    public override void Exit()
    {
        
    }
    
    IEnumerator OnNavMesh()
    {
        yield return null; // Đợi 1 frame để tránh bug
        enemy.agent.enabled = true;

        // Warp đảm bảo agent đồng bộ vị trí mới
        enemy.agent.Warp(enemy.transform.position);
        enemy.isSKill3 = true;
    }
   


   
}
