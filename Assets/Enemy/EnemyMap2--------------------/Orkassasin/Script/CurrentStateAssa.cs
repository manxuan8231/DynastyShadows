
using System.Collections;
using UnityEngine;

public class CurrentStateAssa : AssasinState
{
    public CurrentStateAssa(ControllerStateAssa enemy) : base(enemy) { }
   
    public override void Enter()
    {
        enemy.aiPath.maxSpeed = 5f;
    }
    public override void Exit()
    {

    }
    public override void Update()
    {
       float dis = Vector3.Distance(enemy.transform.position,enemy.player.transform.position);

        if (dis < enemy.stopRange)//khoan cach dừng và attack
        {           
            enemy.aiPath.destination = enemy.transform.position;
            enemy.animator.SetBool("isRunForward", false);
            enemy.animator.SetBool("isWalkForward", false);
            enemy.ChangeState(new AttackStateAssa(enemy));//chuyen trang thai attack
                                
        }
        else if (dis < 40f)//be hon 40 di bo
        {
           
            enemy.aiPath.maxSpeed = 5f;
            enemy.aiPath.destination = enemy.player.transform.position;
            enemy.animator.SetBool("isRunForward", false);
            enemy.animator.SetBool("isWalkForward", true);
            SkillKnife();//bat skill knife
        }
        else//lon hon 40 run
        {
            
            enemy.aiPath.maxSpeed = 20f;
            enemy.aiPath.destination = enemy.player.transform.position;
            enemy.animator.SetBool("isRunForward", true);
            enemy.animator.SetBool("isWalkForward", false);
        }
     

    }

    public void SkillKnife()
    {
        // Kích hoạt kỹ năng 
        if (Time.time >= enemy.lastAutoSkillTime + enemy.autoSkillCooldown)
        {
            enemy.animator.SetTrigger("Knife");
            enemy.StartCoroutine(ActivateAutoSkill());
            enemy.lastAutoSkillTime = Time.time;
        }

    }
    private IEnumerator ActivateAutoSkill()//kich hoat skill dao
    {
        if (enemy.autoSkillKnife != null)
        {
            enemy.autoSkillKnife.SetActive(true);
            yield return new WaitForSeconds(enemy.autoSkillDuration);
            enemy.autoSkillKnife.SetActive(false);
        }
    }




}
