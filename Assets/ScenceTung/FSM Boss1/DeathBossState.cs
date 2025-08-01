using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBossState : Boss1State
{
    public DeathBossState(Boss1Controller enemy) : base(enemy)
    {
    }
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        if (enemy.hp.currHp <= 0)
        {
            enemy.enabled = false;

             enemy.StartCoroutine(DontDestroy(1));
            
        }
    }
    IEnumerator DontDestroy(float duration)
    {
        enemy.boxCollider.enabled = false;
        enemy.anmt.SetTrigger("Death");
        yield return new WaitForSeconds(duration);
        enemy.timeLine.SetActive(true);
        enemy.boss1.SetActive(false);
       

    }

}
