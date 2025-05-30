using UnityEngine;

public class DeathNecState : INecState
{
   public DeathNecState(NecController enemy)  : base(enemy) { }

    public override void Enter()
    {

    }
    public override void Update()
    {
        if (enemy.necHp.curhp <= 0)

        {
            enemy.audioManager.audioSource.Stop();
            enemy.audioManager.audioSource.clip = enemy.audioManager.deathClip;
            enemy.audioManager.audioSource.Play();
            enemy.offDame.enabled = false;
            enemy.anmt.SetTrigger("Death");
            GameObject.Destroy(enemy.gameObject, 5f);
        }
    }
    public override void Exit()
    {
        enemy.enabled = false;
        enemy.agent.enabled = false ;
    }

  
}
