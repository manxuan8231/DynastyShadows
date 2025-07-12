using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssasinHp : MonoBehaviour,IDamageable
{
    [Header("Hp")]
    public Slider sliderHp;
    public TextMeshProUGUI textHp;
    public float curentHp;
    public float maxHp = 10000f;
    [Header("tranh ne ")]
    public float scoreDodge = 0f;
    //tham chieu
    public ControllerStateAssa controllerStateAssa;
  

    void Start()
    {
        curentHp = maxHp;
        sliderHp.maxValue = maxHp;
        sliderHp.value = curentHp;
        textHp.text =$"{curentHp}/{maxHp}";
        controllerStateAssa = FindAnyObjectByType<ControllerStateAssa>();
    }

    
    void Update()
    {
        if (curentHp <= maxHp * 0.8)
        {
            controllerStateAssa.ChangeState(new SkillKnifeStateAssa(controllerStateAssa));//trang thai skill dash    
        }

    }
    public void TakeDamage(float damage)
    {
        curentHp -= damage;
        curentHp = Mathf.Clamp(curentHp, 0, maxHp);
        UpdateUI();
    }
    void UpdateUI()
    {
        sliderHp.value = curentHp;
        textHp.text = $"{curentHp}/{maxHp}";
    }
}
