using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrakonitCameraState : DrakonitState
{
   
    CinemachineBrain brain ;

    private bool isWaiting = true;
    public DrakonitCameraState(DrakonitController enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("trang thai camera");
        brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        }
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance < enemy.chaseRange && isWaiting)
        {
            isWaiting = false;
            enemy.StartCoroutine(PlayCutscene(3f));//thời gian chuyển camera
        }
    }

    public override void Exit() { }

    public IEnumerator PlayCutscene(float seconds)
    {
        //qua camera 1
        enemy.cutScene1.Priority = 20;
        enemy.ChangeState(new DrakonitChaseState(enemy));
        yield return new WaitForSeconds(seconds);

        //qua camera 2   
        enemy.cutScene1.Priority = 0;
        enemy.cutScene2.Priority = 20;

        //qua camera 3
        yield return new WaitForSeconds(seconds);   
        enemy.cutScene2.Priority = 0;
        enemy.cutScene3.Priority = 20;
        yield return new WaitForSeconds(seconds);
        if (brain != null)
        {
            brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;// chuyển cam lại thành easeinout
        }
        enemy.cutScene3.Priority = 0;
       
    }
}
