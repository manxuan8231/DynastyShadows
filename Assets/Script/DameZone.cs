using System.Collections.Generic;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    //effect hit
    [SerializeField] private GameObject effectHit;
    [SerializeField] private Transform tranFormHit;
    public int maxDame = 200;
    public int minDame = 100;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") )
        {
            float randomDame = Random.Range(minDame,maxDame);
            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect,1f);
            // Tìm script EnemyHP 
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
              
                Debug.Log("da lay mau");
                enemyHP.TakeDamage(randomDame);
                return;
            }

            EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {
                enemyHP2.TakeDamage(randomDame);
                return;
            }

            EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {
                enemyHP3.TakeDamage(randomDame);
                return;
            }

            EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {
                enemyHP4.TakeDamage(randomDame);
                return;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            
        }
    }
}
