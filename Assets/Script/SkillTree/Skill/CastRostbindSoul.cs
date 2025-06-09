using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CastRostbindSoul : MonoBehaviour
{
    public float duration = 3f; // Thời gian đóng băng
    public GameObject effectPrefab; // Hiệu ứng đóng băng
    public LayerMask enemyLayer; // Layer của enemy
    private GameObject targetEnemy;

    private void Start()
    {
        targetEnemy = FindNearestEnemy(50f);
        if (targetEnemy != null)
        {
            transform.position = targetEnemy.transform.position + new Vector3(0f, 2.4f, 0f);
            FreezeEnemy(targetEnemy);
            StartCoroutine(UnfreezeAfterDelay(targetEnemy, duration));
        }
        else
        {
            Debug.Log("Không tìm thấy enemy gần.");
            Destroy(gameObject);
        }
    }

    private GameObject FindNearestEnemy(float maxRange)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, maxRange, enemyLayer);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = col.gameObject;
            }
        }

        return nearest;
    }

    private void FreezeEnemy(GameObject enemy)
    {
        Debug.Log("Enemy bị đóng băng: " + enemy.name);

        if (effectPrefab != null)
        {
            Vector3 instanPos = enemy.transform.position + new Vector3(0f, 2.4f, 0f);
            Instantiate(effectPrefab, instanPos, Quaternion.identity, enemy.transform);
        }

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>();
        if (ai != null) ai.enabled = false;
    }

    private void UnfreezeEnemy(GameObject enemy)
    {
        if (enemy == null) return;

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = true;

        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null) anim.enabled = true;

        MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>();
        if (ai != null) ai.enabled = true;

        Debug.Log("Enemy hoạt động lại");
    }

    private IEnumerator UnfreezeAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnfreezeEnemy(enemy);
        Destroy(gameObject);
    }
}
