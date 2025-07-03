using UnityEngine;
using UnityEngine.UI;

public class ShoterTargetDra : MonoBehaviour
{
    public Slider SliderHp;
    public float maxHp = 100f;
    public float currentHp;

    public GameObject bulletPrefab; // Prefab đạn sẽ bắn
    public Transform transforms;
    public float bulletSpeed = 20f;
    public string targetTag = "Enemy"; // Tag của đối tượng sẽ bị bắn

    void Start()
    {
        currentHp = maxHp;
        SliderHp.maxValue = currentHp;
        SliderHp.value = currentHp;
    }

    // Hàm này được gọi khi bị player đánh
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        SliderHp.value = currentHp;
        if (currentHp <= 0)
        {
            currentHp = 0;
            CounterAttack(); // Gọi đòn phản công
            Destroy(gameObject); // Tuỳ chọn: Xoá object sau khi phản công
        }
    }

    void CounterAttack()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy == null) return;

        // Tạo đạn
        GameObject bullet = Instantiate(bulletPrefab, transforms.position, Quaternion.identity);

        // Tính hướng bay đến enemy
        Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;

        // Gắn vận tốc cho đạn
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;

        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}
