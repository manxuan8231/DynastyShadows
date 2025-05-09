using UnityEngine;
using UnityEngine.UI;

public class playerStatus : MonoBehaviour
{
    //xử lý máu
    public Slider sliderHp;
    public float currentHp ;
    public float maxHp = 2000f;

    //xử lý mana skill
    public Slider sliderManaSkill;
    public float currentManaSkill;
    public float maxManaSkill = 2000f;

    //xử lý mana
    public Slider sliderMana;
    public float currentMana;
    public float maxMana = 2000f;

   
    void Start()
    {
        //khởi tạo hp
        currentHp = maxHp;
        sliderHp.maxValue = currentHp;
        //
        currentMana = maxManaSkill;
        sliderMana.maxValue = currentManaSkill;
        //
        currentMana = maxMana;
        sliderMana.maxValue = currentMana;

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeHealth(float amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);
        sliderHp.value = currentHp;

        if(currentHp <= 0)
        {
            Destroy(gameObject,4f);

        }
    }

    public void TakeManaSkill(float amount)
    {
        currentManaSkill -= amount;
        currentManaSkill = Mathf.Clamp(currentManaSkill, 0, maxManaSkill);
        sliderManaSkill.value = currentManaSkill;

    }

    public void TakeMana(float amount)
    {
        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        sliderMana.value = currentMana;

    }
}
