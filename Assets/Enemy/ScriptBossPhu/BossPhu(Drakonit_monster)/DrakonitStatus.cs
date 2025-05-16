using UnityEngine;
using UnityEngine.UI;

public class DrakonitStatus : MonoBehaviour
{
    public Slider sliderHp; // Thanh máu
    public float maxHp = 1000; // Máu tối đa
    public float currentHp; // Máu hiện tại
    void Start()
    {
        currentHp = maxHp; // Khởi tạo máu hiện tại bằng máu tối đa
        sliderHp.maxValue = maxHp; // Đặt giá trị tối đa cho thanh máu
    }

   
    public void TakeDame(float amount)
    {
        currentHp -= amount; // Giảm máu hiện tại
        sliderHp.value = currentHp; // Cập nhật thanh máu
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Đảm bảo máu không âm và không vượt quá tối đa
        if (currentHp <= 0)
        {
            
            // Gọi hàm chết ở đây
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject); // Hủy đối tượng khi chết
    }
}
