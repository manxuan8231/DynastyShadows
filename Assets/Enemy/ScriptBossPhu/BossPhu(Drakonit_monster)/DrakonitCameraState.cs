using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class DrakonitCameraState : DrakonitState
{
   
    CinemachineBrain brain; 

    private bool isWaiting = true;
    public DrakonitCameraState(DrakonitController enemy) : base(enemy) { }
    //tham chieu
    public DrakonitAudioManager audioManager;
    private PlayerControllerState characterController;
    private ComboAttack comboAttack;
    public override void Enter()
    {
        characterController = GameObject.FindAnyObjectByType<PlayerControllerState>();
        comboAttack = GameObject.FindAnyObjectByType<ComboAttack>();
        audioManager = GameObject.FindAnyObjectByType<DrakonitAudioManager>();
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

    public override void Exit() {
       // enemy.ChangeState(new DrakonitChaseState(enemy)); // chuyển sang trạng thái chase
    }

    public IEnumerator PlayCutscene(float seconds)
    {  
        enemy.blockZone.SetActive(true); // vùng chặn
        comboAttack.enabled = false; // Vô hiệu hóa ComboAttack
        characterController.enabled = false; // Vô hiệu hóa CharacterController
        characterController. animator.SetBool("isWalking", false);
        characterController.animator.SetBool("isRunning", false);
        audioManager.audioBackGround.enabled = true; // âm thanh nền
        characterController.animator.enabled = false; // Vô hiệu hóa Animator của player
        //  Hiện chuột
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        //qua camera 1
        audioManager.audioSource.enabled = true; // bật âm thanh
        enemy.cutScene1.Priority = 20;
        enemy.animator.SetBool("Walking", true); // animation đi bộ
        enemy.agent.isStopped = false;   // Cho phép agent di chuyển
        enemy.agent.SetDestination(enemy.player.position);
        enemy.textConten.enabled = true; // hiện text
        enemy.textConten.text = "Người!";
        yield return new WaitForSeconds(seconds);
        //qua camera 2   
        enemy.cutScene1.Priority = 0;
        enemy.cutScene2.Priority = 20;
        enemy.textConten.text = "Cúc khỏi đây!";
        yield return new WaitForSeconds(seconds);
        //qua camera 3
        enemy.imgBietDanh.SetActive(true);// hiện text biêt danh
        enemy.textConten.enabled = false; // ẩn text
        enemy.animator.SetBool("Walking", false); // dung animation đi bộ
        if (enemy.agent.enabled)
        {
            enemy.agent.isStopped = true;
        }
        enemy.animator.SetTrigger("Spell"); // gam
        audioManager.audioSource.PlayOneShot(audioManager.roar); // phát âm thanh gầm
        enemy.cutScene2.Priority = 0;
        enemy.cutScene3.Priority = 20;
        yield return new WaitForSeconds(seconds);
        //
        enemy.ChangeState(new DrakonitIdleState(enemy)); // chuyển sang trạng thái chase
        if (brain != null)// chuyển cam lại thành easeinout
        {
            brain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
        }
        enemy.cutScene3.Priority = 0;
        enemy.imgBietDanh.SetActive(false);// ẩn text biêt danh
        characterController.enabled = true; // Bật lại CharacterController
        comboAttack.enabled = true; // Kích hoạt lại ComboAttack cua player
        characterController.animator.enabled = true; // Vô hiệu hóa Animator của player
        // khoa Hiện chuột
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        enemy.isAttack = true; // bật trạng thái tấn công
        enemy.isSkill = true;
    }
}
