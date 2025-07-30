using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrakonitHp1 : MonoBehaviour, IDamageable
{
    // thanh máu
    public Slider sliderHp;
    public Slider easeSliderHp; // Thanh máu
    public float maxHp = 1000; // Máu tối đa
    public float currentHp; // Máu hiện tại
    public float easeSpeed = 0.05f; // Tốc độ thay đổi thanh máu
    public TextMeshProUGUI textHp; // Text hiển thị máu
    public Collider colliderBox; // Collider của enemy
    public GameObject slider; // GameObject chứa thanh máu
    //tham chieu
    public DrakonitController enemy; // Tham chiếu đến DrakonitController
    void Start()
    {
        //mau
        currentHp = maxHp; // Khởi tạo máu hiện tại
        sliderHp.maxValue = currentHp; // Đặt giá trị tối đa cho thanh máu
        sliderHp.value = currentHp; // Đặt giá trị hiện tại cho thanh máu
        textHp.text = $"{currentHp}/{maxHp}"; // Cập nhật text hiển thị máu
        easeSliderHp.maxValue = currentHp; // Đặt giá trị tối đa cho thanh máu easing
        easeSliderHp.value = currentHp; // Khởi tạo giá trị easing
        slider.SetActive(false); // Ẩn thanh máu
    }

    void Update()
    {
        if (sliderHp.value != easeSliderHp.value)
        {
            easeSliderHp.value = Mathf.Lerp(easeSliderHp.value, currentHp, easeSpeed);
        }

        if (!enemy.isCamera) return;
        float dis = Vector3.Distance(enemy.transform.position, enemy.player.transform.position);     
        if (dis > 50f )
        {
            slider.SetActive(false); // Ẩn thanh máu nếu khoảng cách lớn hơn 50
        }
        else
        {
            slider.SetActive(true); // Hiện thanh máu nếu khoảng cách nhỏ hơn hoặc bằng 50
        }
       
    }
    public void TakeDamage(float amount)
    {
        currentHp -= amount; // Giảm máu hiện tại
        sliderHp.value = currentHp; // Cập nhật thanh máu
        textHp.text = $"{currentHp}/{maxHp}"; // Cập nhật text hiển thị máu
        currentHp = Mathf.Clamp(currentHp, 0, maxHp); // Đảm bảo máu không âm và không vượt quá tối đa

        if (currentHp <= 0)
        {

            Destroy(gameObject);
            enemy.blockZone.SetActive(false); // voo hiệu hóa vùng chặn
            enemy.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemy.enabled = true; // Bật lại DrakonitController để có thể chơi animation chết
            colliderBox.enabled = false;
            slider.SetActive(false); // Ẩn thanh máu
                                     // Gọi hàm chết ở đây
            if (enemy.questMainBacLam != null)
            {
                enemy.questMainBacLam.UpdateKillEnemy(1);// cập nhật số lượng kẻ thù đã giết
            }
            enemy.animator.SetTrigger("Death");
            // ChangeState(new DrakonitDeathState(this));
        }

    }
}
