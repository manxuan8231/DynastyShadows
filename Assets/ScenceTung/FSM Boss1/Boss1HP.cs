using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Boss1HP : MonoBehaviour
{
    public int currHp;
    public int maxHp;
    public TMP_Text textHP;
    public Slider sliderHP;

    private void Start()
    {
        currHp = maxHp;
        UpdateUI();

    }
    public void UpdateUI()
    {
        currHp = maxHp;
        sliderHP.maxValue = currHp; // Đặt giá trị tối đa cho thanh máu
        sliderHP.value = currHp; // Đặt giá trị hiện tại cho thanh máu
        textHP.text = $"{currHp}/{maxHp}"; // Cập nhật text hiển thị máu
    }
   
}
