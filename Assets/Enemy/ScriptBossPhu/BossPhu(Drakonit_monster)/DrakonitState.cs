using UnityEngine;

// Lớp cơ sở trừu tượng đại diện cho một trạng thái của enemy (Drakonit)
// Các trạng thái cụ thể như Idle, Chase, Attack sẽ kế thừa lớp này
public abstract class DrakonitState
{
    // Biến tham chiếu đến enemy đang sử dụng state này 
    protected DrakonitController enemy;

    // Constructor: truyền vào enemy để các trạng thái có thể điều khiển được nó
    public DrakonitState(DrakonitController enemy)
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
