using UnityEngine;

public class DameZoneHit : MonoBehaviour
{
    //effect hit
    [SerializeField] private GameObject effectHit;
    [SerializeField] private Transform tranFormHit;
    public int minDame = 200;
    public int maxDame = 500;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect, 1f);
            float randomDame = Random.Range(minDame, maxDame);
            // Tìm script EnemyHP (hoặc EnemyHP2, 3, 4) trực tiếp trên object bị va chạm
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
                Debug.Log("da lay mau");
                enemyHP.TakeDamageHit(randomDame);
                return;
            }

            EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {
                enemyHP2.TakeDamageHit(randomDame);
                return;
            }

            EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {
                enemyHP3.TakeDamageHit(randomDame);
                return;
            }

            EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {
                enemyHP4.TakeDamageHit(randomDame);
                return;
            }
        }
    }
}
