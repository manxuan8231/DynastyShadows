using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Boss1HP : MonoBehaviour
{
    public int currHp;
    public int maxHp;
    public float lerpSpeed = 0.05f; // Tốc độ lerp cho thanh máu
    public TMP_Text textHP;
    public Slider sliderHP;
    public Slider easeSliderHp;
    public Boss1Controller boss1Controller; // Tham chiếu đến controller của boss
    private void Start()
    {
        currHp = maxHp;
        UpdateUI();
        boss1Controller = FindAnyObjectByType<Boss1Controller>(); // Tìm kiếm đối tượng Boss1Controller trong scene
    }
    private void Update()
    {
        if (sliderHP.value != easeSliderHp.value)
        {
            easeSliderHp.value = Mathf.Lerp(easeSliderHp.value, currHp, lerpSpeed); // Lerp giá trị thanh máu
        }
        if(currHp <= 0)
        {
            boss1Controller.enabled = true;
            boss1Controller.anmt.enabled = true;
           
        }
    }
    public void UpdateUI()
    {
        currHp = maxHp;
        sliderHP.maxValue = currHp; // Đặt giá trị tối đa cho thanh máu
        sliderHP.value = currHp; // Đặt giá trị hiện tại cho thanh máu
        easeSliderHp.maxValue = currHp; // Đặt giá trị tối đa cho thanh máu với hiệu ứng lerp
        easeSliderHp.value = currHp; // Đặt giá trị hiện tại cho thanh máu với hiệu ứng lerp
        textHP.text = $"{currHp}/{maxHp}"; // Cập nhật text hiển thị máu
    }
   
}
