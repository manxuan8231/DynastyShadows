using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemonAlienHp : MonoBehaviour,IDamageable
{
    //hp
  
    public Slider hpSlider;
    public TextMeshProUGUI textHp;
    public float currentHp = 0f;
    public float maxHp = 1000f;
    //armor
    public Slider armorSlider;
    public float currentArmor = 0f;
    public float maxArmor = 1000f;
    //mana
    public Slider manaSlider;
    public float currentMana = 0f;
    public float maxMana = 1000f;

    public bool isTakeDamage = true;
    private bool isArmorBroken = false;
    //tham chieu
    private DemonAlien demonAlien;
    void Start()
    {
        demonAlien = FindAnyObjectByType<DemonAlien>();
        //hp
         currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        textHp.text = $"{currentHp}/{maxHp}";
        //mana
        currentMana = maxMana;
        manaSlider.maxValue = maxMana;
        manaSlider.value = currentMana;
        //armor
        currentArmor = maxArmor;    
        armorSlider.maxValue = currentArmor;
        armorSlider.value = currentArmor;
        isTakeDamage = true;
        isArmorBroken = false;
}

    
    void Update()
    {
        if (currentArmor <= 0 && !isArmorBroken)
        {       
            isArmorBroken = true;
            Invoke(nameof(ResetArmor), 10);
        }
       
    }
    public void TakeDamage(float damage)
    {
        if (!isTakeDamage) return;
        demonAlien.scoreTele++;
        if(currentArmor > 0)
        {
            currentArmor -= damage;
        }
        else
        {         
            currentHp -= damage;
            demonAlien.animator.SetTrigger("getDamage");
        }
        UpdateUI();
    }
    public void UpdateUI()
    {
        //hp
        hpSlider.value = currentHp;
        textHp.text = $"{currentHp}/{maxHp}";
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
       
        //Mana
        manaSlider.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        //armor
        armorSlider.value = currentArmor;
        currentArmor = Mathf.Clamp(currentArmor, 0f,maxArmor);
    }

    public void ResetArmor()
    {
        currentArmor = maxArmor;
        isArmorBroken = false; // reset lại trạng thái
        UpdateUI(); 
    }
}
