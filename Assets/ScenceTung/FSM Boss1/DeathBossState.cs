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
            enemy.agent.isStopped = true;
            enemy.agent.enabled = false;
            GameObject.DontDestroyOnLoad(enemy.playerObjScence1);
            enemy.StartCoroutine(dontDestroy(5));



        }
    }
    IEnumerator dontDestroy(float duration)
    {
        enemy.anmt.SetTrigger("Death");
        GameObject.Destroy(enemy.bossMap1,5f);  
        yield return new WaitForSeconds(duration);
       


    }

}
