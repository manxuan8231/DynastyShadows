using TMPro;
using UnityEngine;

public class DameZoneHit : MonoBehaviour
{
    //effect hit
    [SerializeField] private GameObject effectHit;
    [SerializeField] private Transform tranFormHit;
   //show dame  effect
    public GameObject textDame;
    public Transform textTransform;

    //ke thua
    PlayerStatus playerStatus;

    //dame
    private float maxDame;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect, 1f);
            maxDame = playerStatus.CalculateFinalDamage();
            // Tìm script EnemyHP (hoặc EnemyHP2, 3, 4) trực tiếp trên object bị va chạm
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
                ShowTextDame(maxDame);
               // enemyHP.TakeDamageHit(maxDame);
                return;
            }

            EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {
                ShowTextDame(maxDame);
                enemyHP2.TakeDamageHit(maxDame);
                return;
            }

            EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {
                ShowTextDame(maxDame);
                enemyHP3.TakeDamageHit(maxDame);
                return;
            }

            EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {
                ShowTextDame(maxDame);
                enemyHP4.TakeDamageHit(maxDame);
                return;
            }
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

    public void UpTrueDame(int amount)//up sat thuong chuan
    {
        maxDame += amount;
    }
}
