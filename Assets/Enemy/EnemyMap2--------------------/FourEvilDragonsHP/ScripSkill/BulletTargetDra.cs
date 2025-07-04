using Unity.VisualScripting;
using UnityEngine;

public class BulletTargetDra : MonoBehaviour
{
    public float bulletSpeed = 20f;
    public string targetTag = "Enemy"; // Tag của đối tượng sẽ bị bắn
    public GameObject effectEx;
    public Transform posiEffect;
    private Vector3 moveDirection; // Hướng bay
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject target = FindNearestEnemy();
        if (target != null)
        {
            // Tính hướng bay tới enemy gần nhất
            moveDirection = (target.transform.position - transform.position).normalized;

            // Gắn vận tốc bay cho Rigidbody
            if (rb != null)
            {
                rb.linearVelocity = moveDirection * bulletSpeed;
                rb.useGravity = false; // tuỳ chọn, nếu muốn bay thẳng
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy enemy nào!");
        }
    }
    private void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target != null)
        {
            // Tính hướng bay tới enemy gần nhất
            moveDirection = (target.transform.position - transform.position).normalized;

            // Gắn vận tốc bay cho Rigidbody
            if (rb != null)
            {
                rb.linearVelocity = moveDirection * bulletSpeed;
                rb.useGravity = false; // tuỳ chọn, nếu muốn bay thẳng
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy enemy nào!");
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
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            GameObject inst = Instantiate(effectEx, posiEffect.transform.position, Quaternion.identity);
            Destroy(inst,2f);

            DragonRedHP dr = other.GetComponent<DragonRedHP>();
            if (dr != null)
            {
                Debug.Log("lay mau");
                dr.TakeDamage(1000);
                Destroy(gameObject);
            }
           
        }
    }
}
