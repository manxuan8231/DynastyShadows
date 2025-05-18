using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrakonitDeathState : DrakonitState
{
    private bool hasStartedDeathSequence = false; // Đảm bảo coroutine chỉ chạy một lần
    CinemachineBrain brain;
    public DrakonitDeathState(DrakonitController enemy) : base(enemy) { }

    private PlayerController characterController;
    private DrakonitAudioManager audioManager;
    public override void Enter()
    {
        audioManager = GameObject.FindAnyObjectByType<DrakonitAudioManager>();
        characterController = GameObject.FindAnyObjectByType<PlayerController>();
        characterController.enabled = false; // Vô hiệu hóa CharacterController
        characterController.animator.SetBool("isWalking", false);
        characterController.animator.SetBool("isRunning", false);
        //
       
        audioManager.audioSource.Stop();    
        enemy.agent.isStopped = true;
        enemy.cutScene3.Priority = 20;
        brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        }

    }

    public override void Update()
    {
        if (!hasStartedDeathSequence)
        {
            hasStartedDeathSequence = true; // Đánh dấu đã bắt đầu
            enemy.StartCoroutine(WaitDead());
        }
    }

    public override void Exit()
    {
       
    }

    public IEnumerator WaitDead()
    {
        //  Hiện chuột
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        // 1. Dừng lại và nói câu đầu tiên
        enemy.textConten.enabled = true;
        enemy.textConten.text = "Ah...";
        enemy.animator.SetTrigger("Hit");

        yield return new WaitForSeconds(4f);

        // 2. Câu thứ hai
        enemy.textConten.text = "Người hãy đợi đấy!";
        yield return new WaitForSeconds(4f);

        // 3. Câu cuối cùng
        enemy.textConten.text = "... sẽ báo thù cho ta";
        yield return new WaitForSeconds(4f);

        // 4. Ẩn thoại và chết
        enemy.textConten.enabled = false;
        enemy.animator.SetTrigger("Death");

        yield return new WaitForSeconds(2f);
        brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;// chuyển cam lại thành easeinout

        characterController.enabled = true; // Kích hoạt lại CharacterController
                                            //  Hiện chuột
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        GameObject.Destroy(enemy.gameObject,2f);
    }
}
