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
        status.statDame.text = damage.ToString();
        StartCoroutine(PotionDameRageDuration(damage, duration));
    }

    IEnumerator PotionDameRageDuration(int damage, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage -= damage;
        status.statDame.text = damage.ToString();

    }


    public void BUffCrit(int critDamage, float duration)
    {
        status.criticalDamage += critDamage;
        status.statCrit.text = critDamage.ToString();
        StartCoroutine(BuffCritDuration(critDamage, duration));
    }
    IEnumerator BuffCritDuration(int critDamage, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.criticalDamage -= critDamage;
        status.statCrit.text = critDamage.ToString();
    }

    public void PotionCritRate(int critRate, float duration)
    {
        status.criticalChance += critRate;
        status.statCritChance.text = critRate.ToString();
        StartCoroutine(PotionCritRateDuration(critRate, duration));
    }
    IEnumerator PotionCritRateDuration(int critRate, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.criticalChance -= critRate;
        status.statCritChance.text = critRate.ToString();

    }

    public void BuffDameAndCrit(int damage, int critRate, float duration)
    {
        status.baseDamage += damage;
        status.statDame.text = damage.ToString();
        status.criticalChance += critRate;
        status.statCritChance.text = critRate.ToString();

        StartCoroutine(BuffDameAndCritDuration(damage, critRate, duration));
    }
    IEnumerator BuffDameAndCritDuration(int damage, int critRate, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage -= damage;
        status.statDame.text = damage.ToString();

        status.criticalChance -= critRate;
        status.statCritChance.text = critRate.ToString();

    }

    //buff HP và mana
    public void BuffHP(int health)
    {
        status.currentHp += health;
        status.sliderHp.value = status.currentHp;
        status.currentHp = Mathf.Clamp(status.currentHp, 0, status.sliderHp.maxValue); // Đảm bảo HP không vượt quá max HP
        status.currentHp = status.sliderHp.maxValue; // Đảm bảo HP không vượt quá max HP
    }

    public void BuffMana(int mana)
    {
        status.currentMana += mana;
        status.sliderMana.value = mana;
        status.currentMana = Mathf.Clamp(status.currentMana, 0, status.sliderMana.maxValue); // Đảm bảo mana không vượt quá max mana
        status.currentMana = status.sliderMana.maxValue; // Đảm bảo mana không vượt quá max mana
    }

    public void BuffAllStats(int stats,int crit,int critChance,int dame,float duration)
    {
        //tăng máu
        status.maxHp += stats;
        status.sliderHp.maxValue = status.maxHp;
        status.sliderHp.value = status.maxHp;

        //tăng mana
        status.maxMana += stats;
        status.sliderMana.maxValue = status.maxMana;
        status.sliderMana.value = status.maxMana;


        //tăng crit
        status.criticalDamage += crit;
        status.statCrit.text = crit.ToString();
        //tăng critchance
        status.criticalChance += critChance;
        status.statCritChance.text = critChance.ToString();
        //tang8 dame
        status.baseDamage += dame;
        status.statDame.text = dame.ToString();
        StartCoroutine(StopBuffAllStats(stats, crit, critChance, dame, duration));
        

    }
    IEnumerator StopBuffAllStats(int stats, int crit, int critChance, int dame, float duration)
    {

        yield return new WaitForSeconds(duration);


        //tăng máu
        status.maxHp -= stats;
        status.sliderHp.maxValue = status.maxHp;
        status.sliderHp.value = status.maxHp;

        //tăng mana
        status.maxMana -= stats;
        status.sliderMana.maxValue = status.maxMana;
        status.sliderMana.value = status.maxMana;


        //tăng crit
        status.criticalDamage -= crit;
        status.statCrit.text = crit.ToString();
        //tăng critchance
        status.criticalChance -= critChance;
        status.statCritChance.text = critChance.ToString();
        //tang8 dame
        status.baseDamage -= dame;
        status.statDame.text = dame.ToString();
    }

}
