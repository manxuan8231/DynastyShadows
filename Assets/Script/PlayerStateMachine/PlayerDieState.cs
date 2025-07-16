using System.Collections;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(PlayerControllerState player): base(player) { }
    private bool isDie = false; // Biến để kiểm tra xem đã chết hay chưa
    public override void Enter()
    {
        isDie = true; // Đặt biến isDie thành true khi vào trạng thái chết
       
    }

    public override void Exit()
    {
       
        player.canvasLoad.SetActive(false);
        player.animator.SetTrigger("Hit"); // Đặt lại trạng thái hoạt hình về 
       
    }

    public override void Update()
    {
        if(isDie == true)
        {
           
            player.StartCoroutine(WaitChangeState()); // Bắt đầu đợi để chuyển trạng thái
        }
    }
    private IEnumerator WaitChangeState()
    {
        isDie = false;
        player.animator.SetTrigger("die");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(4);

        player.canvasLoad.SetActive(true);
        yield return new WaitForSeconds(2);

        // Hồi máu
        player.playerStatus.currentHp = player.playerStatus.maxHp;
        player.playerStatus.sliderHp.value = player.playerStatus.currentHp;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.canvasLoad.SetActive(false);
        // Hồi sinh tại checkpoint
       /* player.controller.enabled = false; // Tắt controller để tránh va chạm khi di chuyển
        player.transform.position = player.checkpointPosition;
        player.controller.enabled = true; // Bật lại controller*/
        // Quay lại trạng thái bth
        player.ChangeState(new PlayerCurrentState(player)); 
    }


}
