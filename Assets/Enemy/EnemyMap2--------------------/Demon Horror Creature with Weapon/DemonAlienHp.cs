using Pathfinding;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.FlowStateWidget;

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
    public Collider colliTakeDame;
    private DemonAlien demonAlien;
    private EvenAlien evenAlien;
    void Start()
    {
        demonAlien = FindAnyObjectByType<DemonAlien>();
        evenAlien = FindAnyObjectByType<EvenAlien>();  
        colliTakeDame = GetComponent<Collider>();
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
        if (currentHp <= 0f)//t muon goi lien tuc ok
        {

            demonAlien.ChangerState(DemonAlien.EnemyState.Die);
        }
    }
    public void TakeDamage(float damage)
    {
        if (currentHp <= 0 && !isTakeDamage) return;
        
        demonAlien.scoreTele++;
        if(currentArmor > 0)
        {
            currentArmor -= damage;
            currentArmor = Mathf.Clamp(currentArmor, 0f, maxArmor);
        }
        else
        {
           
            currentHp -= damage;
            currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
            demonAlien.animator.SetTrigger("getDamage");
            if (currentHp <= 0f) 
            {
               
                demonAlien.ChangerState(DemonAlien.EnemyState.Die);
            }
        }
        UpdateUI();
        evenAlien.EndEffectShort();//tat effect short khi bi danh
        evenAlien.EndTelePathic();
        evenAlien.EndEffectBall();
    }
    public void UpdateUI()
    {
        //hp
        hpSlider.value = currentHp;
        textHp.text = $"{(int)currentHp}/{maxHp}";
       
       
        //Mana
        manaSlider.value = currentMana;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
        //armor
        armorSlider.value = currentArmor;
       
    }

    public void ResetArmor()
    {
        currentArmor = maxArmor;
        isArmorBroken = false; // reset lại trạng thái
        UpdateUI(); 
    }
}
