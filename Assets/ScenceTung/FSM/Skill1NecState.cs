using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Skill1NecState : INecState
{
    public Skill1NecState(NecController enemy) : base(enemy) { }
    public bool isTele;
    public override void Enter()
    {
        isTele = false;
        
        
    }

    public override void Update()
    {

         if (enemy.necHp.curhp <= 7000 && !isTele  && enemy.isSkill1==false)
        {
            enemy.audioManager.audioSource.PlayOneShot(enemy.audioManager.skill1Sound);
          
            enemy.agent.enabled = false;
            isTele = true;
            // 2. Tính hướng lùi
            Vector3 directionAway = (enemy.transform.position - enemy.player.transform.position).normalized;
            float retreatDistance = 25f;
            Vector3 retreatPosition = enemy.transform.position + directionAway * retreatDistance;

            // 3. Nâng trục Y lên 3–4 đơn vị
            retreatPosition.y = enemy.transform.position.y + Random.Range(3f, 4f);

            // 4. Dịch chuyển enemy tới vị trí mới
            enemy.transform.position = retreatPosition;
            //enemy.necHp.curhp = 10000f;
            //enemy.necHp.sliderHp.maxValue = enemy.necHp.curhp;
            //enemy.necHp.sliderHp.value = enemy.necHp.curhp;


            // Trigger skill hoặc animation
            enemy.anmt.SetTrigger("Skill1");
            enemy.transform.LookAt(enemy.player);
            enemy.necHp.sliderHp.maxValue = 10000f;
            enemy.StartCoroutine(HealAndSetMaxHp(10000f, 10f));
            enemy.StartCoroutine(Open());
            if(enemy.checkEnemyCount <= 10)
            {
                enemy.ChangState(new Skill2NecState(enemy));
            } 
            
        }
         
    }
    public override void Exit()
    {
       
    }

    IEnumerator Open()
    {
        yield return new WaitForSeconds(7);
        if (enemy.hasSpawned == false)
        {
            enemy.hasSpawned = true;
            enemy.SpawnEnemiesInstantly();
        }
        yield return new WaitForSeconds(2.5f);
        enemy.isSkill2 = true;
    }

    IEnumerator HealAndSetMaxHp(float newMaxHp, float duration)
    {
        float startHp = enemy.necHp.curhp;
        float startMax = enemy.necHp.sliderHp.maxValue;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            float currentHp = Mathf.Lerp(startHp, newMaxHp, t);
            float currentMax = Mathf.Lerp(startMax, newMaxHp, t);

            enemy.necHp.curhp = currentHp;
            enemy.necHp.sliderHp.maxValue = currentMax;
            enemy.necHp.sliderHp.value = currentHp;
            enemy.necHp.textHp.text = $"{(int)currentHp}/{(int)currentMax}";

            yield return null;
        }

        enemy.necHp.curhp = newMaxHp;
        enemy.necHp.sliderHp.maxValue = newMaxHp;
        enemy.necHp.sliderHp.value = newMaxHp;
        enemy.necHp.textHp.text = $"{(int)newMaxHp}/{(int)newMaxHp}";
    }




}
