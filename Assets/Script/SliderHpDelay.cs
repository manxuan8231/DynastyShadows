using UnityEngine;
using UnityEngine.UI;

public class SliderHpDelay : MonoBehaviour
{
    public Slider realSlider;    // Thanh đỏ
    public Slider damageSlider;  // Thanh trắng tụt chậm
    public float lerpSpeed = 2f;

    private float targetHP;

    public void SetHP(float hpPercent)
    {
        targetHP = Mathf.Clamp01(hpPercent);          // Giới hạn 0-1
        realSlider.value = targetHP;                  // Thanh đỏ tụt ngay
    }

    void Update()
    {
        if (damageSlider.value > targetHP)
        {
            damageSlider.value = Mathf.Lerp(damageSlider.value, targetHP, Time.deltaTime * lerpSpeed);
        }
        else
        {
            damageSlider.value = targetHP;
        }
    }
}
