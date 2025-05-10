using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
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
        currentManaSkill = maxManaSkill;
        sliderManaSkill.maxValue = currentManaSkill;
        //
        currentMana = maxMana;
        sliderMana.maxValue = currentMana;

      
    }

    
    void Update()
    {
        RegenerateMana();
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
        sliderMana.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
      
    }
    public void AddMana(float amount)
    {
        currentMana += amount;
        sliderMana.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }
    public void RegenerateMana()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            TakeMana(100 * Time.deltaTime); // trừ dần theo thời gian
        }
        else {
            AddMana(100 * Time.deltaTime); // cộng dần theo thời gian
           
        }
    }
}
