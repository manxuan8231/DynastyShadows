using System.Collections;
using UnityEngine;

public class itemBuff : MonoBehaviour
{       
    public PlayerStatus status;
    void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
    }

   //buff attack
    public void PotionDameRage(int damage,float duration)
    {
        status.baseDamage += damage;
        
        StartCoroutine(PotionDameRageDuration(damage, duration));
    }

    IEnumerator PotionDameRageDuration(int damage, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage -= damage;
    }


    public void BUffCrit(int critDamage, float duration)
    {
        status.criticalDamage += critDamage;
        StartCoroutine(BuffCritDuration(critDamage, duration));
    }
    IEnumerator BuffCritDuration(int critDamage, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.criticalDamage -= critDamage;
    }

    public void PotionCritRate(int critRate, float duration)
    {
        status.criticalChance += critRate;
        StartCoroutine(PotionCritRateDuration(critRate, duration));
    }
    IEnumerator PotionCritRateDuration(int critRate, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.criticalChance -= critRate;
    }

    public void BuffDameAndCrit(int damage, int critRate, float duration)
    {
        status.baseDamage += damage;
        status.criticalChance += critRate;

        StartCoroutine(BuffDameAndCritDuration(damage, critRate, duration));
    }
    IEnumerator BuffDameAndCritDuration(int damage, int critRate, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage -= damage;
        status.criticalChance -= critRate;
    }

    //buff HP và mana
    public void BuffHP(int health)
    {
        status.currentHp += health;
        status.currentHp = Mathf.Clamp(status.currentHp, 0, status.sliderHp.maxValue); // Đảm bảo HP không vượt quá max HP
        status.currentHp = status.sliderHp.maxValue; // Đảm bảo HP không vượt quá max HP
    }

    public void BuffMana(int mana)
    {
        status.currentMana += mana;
        status.currentMana = Mathf.Clamp(status.currentMana, 0, status.sliderMana.maxValue); // Đảm bảo mana không vượt quá max mana
        status.currentMana = status.sliderMana.maxValue; // Đảm bảo mana không vượt quá max mana
    }


}
