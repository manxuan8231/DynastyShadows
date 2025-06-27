using UnityEngine;

public class Turret : MonoBehaviour
{
    public float attackRange = 10f;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    public GameObject bulletPrefab;
    public Transform firePoint;

    private Transform currentTarget;

    void Update()
    {
        if (fireCountdown <= 0f)
        {
            FindTarget();
            if (currentTarget != null)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
        }
        fireCountdown -= Time.deltaTime;
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy1");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < shortestDistance && dist <= attackRange)
            {
                shortestDistance = dist;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
            currentTarget = nearestEnemy.transform;
        else
            currentTarget = null;
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Seek(currentTarget);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
