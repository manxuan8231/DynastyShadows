using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CastRostbindSoul : MonoBehaviour
{
    public float duration = 3f; // Thời gian đóng băng
    public GameObject effectPrefab; // Hiệu ứng đóng băng
    public string[] enemyTag; // Tag của enemy cần đóng băng
    private GameObject targetEnemy;


    private void Start()
    {
        // Tìm enemy gần nhất trong phạm vi 20
        targetEnemy = FindNearestEnemy(50);
        if (targetEnemy != null)
        {
           
            // Dịch chuyển skill đến vị trí enemy (ngực)
            transform.position = targetEnemy.transform.position + new Vector3(0f, 2.4f, 0f);

            // Thực hiện đóng băng
            FreezeEnemy(targetEnemy);

            // Bắt đầu đếm thời gian đóng băng
            StartCoroutine(UnfreezeAfterDelay(targetEnemy,duration));
        }
        else
        {
            Debug.Log("Không tìm thấy enemy gần.");
            Destroy(gameObject);
        }
    }

    private GameObject FindNearestEnemy(float maxRange)
    {
        GameObject nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        for (int i = 0; i < enemyTag.Length; i++)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag[i]);

            foreach (var enemy in enemies)
            {
                float dist = Vector3.Distance(currentPos, enemy.transform.position);
                if (dist < minDist && dist <= maxRange)
                {
                    minDist = dist;
                    nearest = enemy;
                }
            }
        }

        return nearest;
    }


    private void FreezeEnemy(GameObject enemy)
    {
        Debug.Log("Enemy bị đóng băng: " + enemy.name);

        // Hiệu ứng đóng băng
        if (effectPrefab != null)
        {
            Vector3 instanPos = enemy.transform.position + new Vector3(0f, 2.4f, 0f);
            Instantiate(effectPrefab, instanPos, Quaternion.identity, enemy.transform);
            Destroy(effectPrefab, 5); // Giả sử hiệu ứng tồn tại trong 2 giây
        }

        // Tắt NavMeshAgent
        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        // Tắt Animator
        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        // Tắt script AI (thay bằng tên thật nếu có)
        MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>();
        if (ai != null) ai.enabled = false;
    }

    private void UnfreezeEnemy(GameObject enemy)//hoạt động lại enemy sau khi đóng băng
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

    private IEnumerator UnfreezeAfterDelay(GameObject enemy,float delay)//Đợi thời gian đóng băng
    {
        yield return new WaitForSeconds(delay);
        UnfreezeEnemy(targetEnemy);
        Destroy(gameObject);
        Vector3 instanPos = enemy.transform.position + new Vector3(0f, 2.4f, 0f);
        Instantiate(effectPrefab, instanPos, Quaternion.identity, enemy.transform);
        Destroy(effectPrefab, 5); // Giả sử hiệu ứng tồn tại trong 2 giây
    }
}
