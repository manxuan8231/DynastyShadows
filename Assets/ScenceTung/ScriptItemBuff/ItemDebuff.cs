using System.Collections;
using UnityEngine;

public class ItemDebuff : MonoBehaviour
{
    [SerializeField] private PlayerStatus status;
        
    void Start()
    {
        status = FindAnyObjectByType<PlayerStatus>();
       
    }

    public void Debuff1(int critDame,  float duration)
    {
      
        status.criticalDamage *= critDame;
        status.criticalChance = 1;
        StartCoroutine(UnDebuff1(critDame, duration));
    }

    IEnumerator UnDebuff1(int critDame, float duration)
    {
       yield return new WaitForSeconds(duration);
        status.criticalDamage /= critDame;
        status.criticalChance = status.firstCritChance;
    }

    public void Debuff2(int dame, int hp, float duration) 
    { 
        status.baseDamage *= dame;
        status.currentHp -= hp;
        status.sliderHp.value -= hp;
        StartCoroutine(Undebuff2(dame,hp,duration));

    }

    IEnumerator Undebuff2(int dame, int hp, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage /= dame;
    }

    public void DeBuff3(int critRate,int dame,float duration)
    {
        status.criticalChance *= critRate;
        status.baseDamage -= dame;
        StartCoroutine(Undebuff3(critRate,dame,duration));
    }

    IEnumerator Undebuff3(int critRate, int dame, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.criticalChance /= critRate;
        status.baseDamage += dame;
    }

    public void Debuff4(int dame, int critDame, float duration)
    {
        status.baseDamage *= dame;
        status.criticalDamage += critDame;
        
        StartCoroutine(Undebuff4(dame, critDame, duration));
    }

    IEnumerator Undebuff4(int dame, int critDame, float duration)
    {
        yield return new WaitForSeconds(duration);
        status.baseDamage /= dame;
        status.criticalDamage -= critDame;
        status.criticalChance = status.firstCritChance;
    }
}
