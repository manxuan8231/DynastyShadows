using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DrakonitDeathState : DrakonitState
{
    private bool hasStartedDeathSequence = false; // Đảm bảo coroutine chỉ chạy một lần
    CinemachineBrain brain;
    public DrakonitDeathState(DrakonitController enemy) : base(enemy) { }
    // tham chiếu
    private PlayerControllerState characterController;
    private ComboAttack comboAttack;
    private QuestMainBacLam questMainBacLam;

    private DrakonitAudioManager audioManager;
    public override void Enter()
    {
        audioManager = GameObject.FindAnyObjectByType<DrakonitAudioManager>();
        characterController = GameObject.FindAnyObjectByType<PlayerControllerState>();
        comboAttack = GameObject.FindAnyObjectByType<ComboAttack>();
       questMainBacLam = GameObject.FindAnyObjectByType<QuestMainBacLam>();
        characterController.enabled = false; // Vô hiệu hóa CharacterController
        characterController.animator.SetBool("isWalking", false);
        characterController.animator.SetBool("isRunning", false);
        // 
        audioManager.audioBackGround.enabled = false;    
        enemy.agent.isStopped = true;
        enemy.cutScene3.Priority = 20;
        //  Hiện chuột
        brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        }

        //tắt các trạng thái 
        enemy.isSkill = false; // Tắt trạng thái skill
        enemy.isAttack = false; // Tắt trạng thái tấn công
        enemy.isRunning = false; // Tắt trạng thái chạy
        enemy.isWalking = false; // Tắt trạng thái đi bộ
        enemy.effectHandR.SetActive(false); // tắt hiệu ứng tay phải
        enemy.effectHandL.SetActive(false); // tắt hiệu ứng tay trái
        enemy.animator.SetBool("Walking", false); // Dừng animation đi bộ
        enemy.animator.SetBool("Running", false); // Dừng animation chạy
        enemy.animator.SetBool("Attack2", false); // Dừng animation tấn công
        enemy.animator.SetBool("Attack1", false); // Dừng animation tấn công
        enemy.animator.SetBool("Attack3", false); // Dừng animation tấn công
        enemy.colliderBox.enabled = false; // Vô hiệu hóa collider
        enemy.agent.isStopped = true; // Dừng di chuyển

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
        comboAttack.enabled = false; // Vô hiệu hóa ComboAttack
        // 1. Dừng lại và nói câu đầu tiên
        enemy.textConten.enabled = true;
        enemy.textConten.text = "Ah...";
        enemy.animator.SetTrigger("Hit");
        enemy.colliderBox.enabled = false; // Vô hiệu hóa collider
        yield return new WaitForSeconds(4f);

        // 2. Câu thứ hai
        enemy.textConten.text = "Người hãy đợi đấy!";
        yield return new WaitForSeconds(4f);

        // 3. Câu cuối cùng
        enemy.textConten.text = "Ta sẽ báo thù";
        yield return new WaitForSeconds(4f);

        // 4. Ẩn thoại và chết
        enemy.textConten.enabled = false;
        enemy.animator.SetTrigger("Death");

        yield return new WaitForSeconds(2f);
        brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;// chuyển cam lại thành easeinout
        comboAttack.enabled = true; // Kích hoạt lại ComboAttack cua player
        characterController.enabled = true; // Kích hoạt lại CharacterController
        //  Hiện chuột                                                                     
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        if (questMainBacLam != null) { 
            questMainBacLam.UpdateKillEnemy(1);// cập nhật số lượng kẻ thù đã giết
    }
        GameObject.Destroy(enemy.gameObject,2f);
    }
}
