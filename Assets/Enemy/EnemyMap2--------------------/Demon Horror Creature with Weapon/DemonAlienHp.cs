using UnityEngine;
using UnityEngine.UI;

public class DemonAlienHp : MonoBehaviour,IDamageable
{
    public Slider hpSlider;
    public float currentHp = 0f;
    public float maxHp = 1000f;
    void Start()
    {
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    
    void Update()
    {
        
    }
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        hpSlider.value = currentHp;
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
        if (currentHp <= 0)
        {
            
        }
    }
}
