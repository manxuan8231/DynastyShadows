using UnityEngine;

public abstract class INecState
{


    protected NecController enemy;

    // Constructor: truyền vào enemy để các trạng thái có thể điều khiển được nó
    public INecState(NecController enemy)
    {
        this.enemy = enemy;
    }

    // Hàm trừu tượng bắt buộc các lớp con phải override
    // Gọi khi bắt đầu chuyển sang trạng thái này
    public abstract void Enter();

    // Gọi mỗi frame khi enemy đang ở trạng thái này
    public abstract void Update();

    // Gọi khi rời khỏi trạng thái này
    public abstract void Exit();

}



