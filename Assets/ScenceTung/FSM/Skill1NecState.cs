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

         if (enemy.necHp.curhp <= 350 && !isTele)
        {
           
            isTele = true;
            Vector3 directionAway = (enemy.transform.position - enemy.player.transform.position).normalized;
            float retreatDistance = 25f;
            Vector3 retreatPosition = enemy.transform.position + directionAway * retreatDistance;

            // Đảm bảo không thay đổi độ cao
            retreatPosition.y = enemy.transform.position.y;

            // Tắt agent để có thể set vị trí thủ công
            enemy.agent.enabled = false;

            // Dịch chuyển ngay lập tức
            enemy.transform.position = retreatPosition;

            // Bật lại agent
            enemy.agent.enabled = true;
            enemy.agent.isStopped = true;

            // Trigger skill hoặc animation
            enemy.anmt.SetTrigger("Skill1");
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
