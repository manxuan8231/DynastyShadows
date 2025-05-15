using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    //effect hit
    [SerializeField] private GameObject effectHit;
    [SerializeField] private Transform tranFormHit;

   //hien dame effect
    public GameObject textDame;
    public Transform textTransform;

    //ke thua
    PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") )
        {
            float finalDamage = playerStatus.CalculateFinalDamage();

            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect,1f);
            // Tìm script EnemyHP 
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
                ShowTextDame(finalDamage);
                enemyHP.TakeDamage(finalDamage);
                return;
            }

            EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {
                ShowTextDame(finalDamage);
                enemyHP2.TakeDamage(finalDamage);
                return;
            }

            EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {
                ShowTextDame(finalDamage);
                enemyHP3.TakeDamage(finalDamage);
                return;
            }

            EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {
                ShowTextDame(finalDamage);
                enemyHP4.TakeDamage(finalDamage);
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
    public void ShowTextDame(float damage)
    {
        GameObject effectText = Instantiate(textDame, textTransform.position, Quaternion.identity);
        Destroy(effectText, 0.5f);
        // Truyền dame vào prefab
        TextDamePopup popup = effectText.GetComponent<TextDamePopup>();
        if (popup != null)
        {
            popup.Setup(damage);
        }
    }

}
