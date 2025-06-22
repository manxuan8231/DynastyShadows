using UnityEngine;

public class Skill3NecState : INecState
{
    public Skill3NecState(NecController enemy): base(enemy) { }
    public float skill3Timer = -12f;
    public float skill3Cooldown = 12f;
    public override void Enter()
    {

       
    }
    public override void Update()
    {
        enemy.necHp.textHp.text = $"{(int)enemy.necHp.curhp}/{(int)enemy.necHp.maxhp}";



        if (enemy.isSkill1 == true)
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);
            if (Time.time >= skill3Timer + skill3Cooldown && distance <= 100f)
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
