using System.Collections;
using UnityEngine;

public class Skill1NecState : INecState
{
    public Skill1NecState(NecController enemy) : base(enemy) { }
    public bool isTele;
    
    public override void Enter()
    {
        isTele = false;
        
    }

    public override void Update()
    {

         if (enemy.necHp.curhp <= 350 && !isTele  && enemy.isSkill1==false)
        {
          
            enemy.agent.enabled = false;
            isTele = true;
            // 2. Tính hướng lùi
            Vector3 directionAway = (enemy.transform.position - enemy.player.transform.position).normalized;
            float retreatDistance = 25f;
            Vector3 retreatPosition = enemy.transform.position + directionAway * retreatDistance;

            // 3. Nâng trục Y lên 3–4 đơn vị
            retreatPosition.y = enemy.transform.position.y + Random.Range(3f, 4f);

            // 4. Dịch chuyển enemy tới vị trí mới
            enemy.transform.position = retreatPosition;
            enemy.necHp.curhp = 500f;
            

            // Trigger skill hoặc animation
            enemy.anmt.SetTrigger("Skill1");
            enemy.transform.LookAt(enemy.player);
            enemy.StartCoroutine(Open());
            if(enemy.checkEnemyCount <= 10)
            {
                enemy.ChangState(new Skill2NecState(enemy));
            } 
            
        }
         
    }
    public override void Exit()
    {
       
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(7);
        if (enemy.hasSpawned == false)
        {
            enemy.hasSpawned = true;
            enemy.SpawnEnemiesInstantly();
        }
        yield return new WaitForSeconds(2.5f);
        enemy.isSkill2 = true;
    }
    
    
    
}
