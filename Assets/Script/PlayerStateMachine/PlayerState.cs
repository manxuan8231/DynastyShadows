using UnityEngine;

public abstract class PlayerState
{
    protected PlayerControllerState player;
    public PlayerState(PlayerControllerState player)
    {
      this.player = player;
    }

    // Hàm trừu tượng bắt buộc các lớp con phải override
    // Gọi khi bắt đầu chuyển sang trạng thái này
    public abstract void Enter();

    // Gọi mỗi frame khi enemy đang ở trạng thái này
    public abstract void Update();

    // Gọi khi rời khỏi trạng thái này
    public abstract void Exit();
}
