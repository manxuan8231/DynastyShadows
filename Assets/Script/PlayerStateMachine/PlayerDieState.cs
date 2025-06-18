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
        yield return new WaitForSeconds(4);//------------------------
        player.canvasLoad.SetActive(true);  
        yield return new WaitForSeconds(6f); // Thời gian chờ trước khi chuyển trạng thái --------------------     
        player.playerStatus.currentHp = player.playerStatus.maxHp; // Đặt lại máu của người chơi về tối đa\
        player.playerStatus.sliderHp.value = player.playerStatus.currentHp; // Cập nhật thanh máu
        isDie = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.canvasLoad.SetActive(false);
        player.ChangeState(new PlayerCurrentState(player)); // Quay lại trạng thái hiện tại
    }

}
