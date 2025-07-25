using UnityEngine;
using UnityEngine.UI;

public class SliderDelay : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth;
    public float currentHealth;
    public float lerpSpeed = 0.05f;
    void Start()
    {
        currentHealth = maxHealth;
      
    }

   
    void Update()
    {
        if(healthSlider.value != currentHealth)
        {
          
            healthSlider.value = currentHealth;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
           TakeDame(30f);

        }
        if(healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }
    }
    public void TakeDame(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
}
