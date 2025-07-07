using Pathfinding;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CastRostbindSoul : MonoBehaviour
{
   
    private GameObject effectInstance;
    private bool isDestroyed = false;

    public GameObject effectPrefab; // Hiệu ứng đóng băng
    public LayerMask enemyLayer; // Layer của enemy
    private GameObject targetEnemy;
    
    //hien dame effect
    public GameObject textDame;
    public Transform textTransform;

    //tham chieu
    public PlayerStatus status;
    public Skill1Manager skill1Manager;
    private void Start()
    {
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        status = FindAnyObjectByType<PlayerStatus>();
        targetEnemy = FindNearestEnemy(50f);
        if (targetEnemy != null)
        {
            

            FreezeEnemy(targetEnemy);
            StartCoroutine(UnfreezeAfterDelay(targetEnemy, skill1Manager.timeSkill1));
        }
        else
        {
            Debug.Log("Không tìm thấy enemy gần.");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Nếu enemy bị tắt thì huỷ hiệu ứng và skill
        if (!isDestroyed && targetEnemy != null && !targetEnemy.activeInHierarchy)
        {
            isDestroyed = true;

            if (effectInstance != null)
                Destroy(effectInstance);

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
            effectInstance = Instantiate(effectPrefab, instanPos, Quaternion.identity, enemy.transform);
            Destroy(effectInstance,5f);
        }


        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        Animator anim = enemy.GetComponent<Animator>();
        if (anim != null) anim.enabled = false;

        MonoBehaviour ai = enemy.GetComponent<MonoBehaviour>();
        if (ai != null) ai.enabled = false;

        AIPath aIPath = enemy.GetComponent<AIPath>();
        if(aIPath != null) aIPath.enabled = false;


        // láy mau enemy sau khi mở khóa
        if (skill1Manager.isDamaged == true)
        {
            float dame = 2000f;
            IDamageable damageable = enemy.GetComponent<IDamageable>();//goi interface
            if (damageable != null) 
            {
              
                damageable.TakeDamage(dame);
               
            }
        }
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

        AIPath aIPath = enemy.GetComponent<AIPath>();
        if (aIPath != null) aIPath.enabled = true;
    }

    private IEnumerator UnfreezeAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnfreezeEnemy(enemy);
        Destroy(gameObject);
     
    }

   
}
