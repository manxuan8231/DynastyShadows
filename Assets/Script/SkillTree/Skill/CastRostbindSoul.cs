using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CastRostbindSoul : MonoBehaviour
{
    public float duration = 3f; // Thời gian đóng băng
    public GameObject effectPrefab; // Hiệu ứng đóng băng
    public float moveSpeed = 10f; // Tốc độ bay đến target

    private GameObject targetEnemy;

    private void Start()
    {
        // Khi skill tạo ra, tìm enemy gần nhất
        targetEnemy = FindNearestEnemy(20);
        if (targetEnemy != null)
        {
            Debug.Log("Target enemy:");
            // Bắt đầu bay đến target
            StartCoroutine(MoveToTarget());
        }
        else
        {
            Debug.Log("Không tìm thấy enemy gần.");
            Destroy(gameObject);
        }
    }

    private GameObject FindNearestEnemy(float maxRange)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(currentPos, enemy.transform.position);
            if (dist < minDist && dist <= maxRange)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }


    private IEnumerator MoveToTarget()
    {
        Vector3 targetPos = targetEnemy.transform.position + new Vector3(0f, 2.4f, 0f); // Vị trí ngực
        while (targetEnemy != null && Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            // Di chuyển thẳng tới vị trí ngực
            Vector3 dir = (targetPos - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;

            yield return null;
        }

        if (targetEnemy != null)
        {
            FreezeEnemy(targetEnemy);
            yield return new WaitForSeconds(duration);
            UnfreezeEnemy(targetEnemy);
        }

        Destroy(gameObject);
    }



    private void FreezeEnemy(GameObject enemy)
    {
        Debug.Log("Enemy bị đóng băng: " + enemy.name);

        // Hiệu ứng đóng băng
        if (effectPrefab != null)
        {
            Vector3 instanPos = enemy.transform.position + new Vector3(0f, 2.4f, 0f); // Vị trí ngực
            Instantiate(effectPrefab, instanPos, Quaternion.identity, enemy.transform);
        }

        // Tắt NavMeshAgent
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // Tắt Animator
        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = false;
        }

        // Tắt script điều khiển
        MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>(); // Thay thế bằng script AI cụ thể nếu có
        if (ai != null)
        {
            ai.enabled = false;
        }
    }

    private void UnfreezeEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
            {
                agent.enabled = true;
            }

            Animator anim = enemy.GetComponent<Animator>();
            if (anim != null)
            {
                anim.enabled = true;
            }

            MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>();
            if (ai != null)
            {
                ai.enabled = true;
            }

            Debug.Log("Enemy hoạt động lại");
        }
    }
}
