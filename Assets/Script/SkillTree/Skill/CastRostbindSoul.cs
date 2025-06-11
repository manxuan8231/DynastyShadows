using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CastRostbindSoul : MonoBehaviour
{
   
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
            transform.position = targetEnemy.transform.position + new Vector3(0f, 2f, 0f);
            FreezeEnemy(targetEnemy);
            StartCoroutine(UnfreezeAfterDelay(targetEnemy, skill1Manager.timeSkill1));
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

        // Tìm script EnemyHP lay mau
        if (skill1Manager.isDamaged == true) {
            EnemyHP enemyHP = enemy.GetComponent<EnemyHP>();
            if (enemyHP != null)//mở khóa kỹ năng mới cho mất máu
            {

                ShowTextDame(status.baseDamage);
                enemyHP.TakeDamage(status.baseDamage);
                return;
            }
            EnemyHP2 enemyHP2 = enemy.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {

                ShowTextDame(status.baseDamage);
                enemyHP2.TakeDamage(status.baseDamage);
                return;
            }

            EnemyHP3 enemyHP3 = enemy.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {

                ShowTextDame(status.baseDamage);
                enemyHP3.TakeDamage(status.baseDamage);
                return;
            }

            EnemyHP4 enemyHP4 = enemy.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {

                ShowTextDame(status.baseDamage);
                enemyHP4.TakeDamage(status.baseDamage);
                return;
            }
            //boss drakonit
            DrakonitController drakonitController = enemy.GetComponent<DrakonitController>();
            if (drakonitController != null)
            {

                ShowTextDame(status.baseDamage);
                drakonitController.TakeDame(status.baseDamage);
                return;
            }
            //boss ork
            BossHP bossHP = enemy.GetComponent<BossHP>();
            if (bossHP != null)
            {

                ShowTextDame(status.baseDamage);
                bossHP.TakeDamage(status.baseDamage);
                return;
            }
            //boss sa mac
            NecController necController = enemy.GetComponent<NecController>();
            if (necController != null)
            {
                Debug.Log("Đã trúng NecController");

                ShowTextDame(status.baseDamage);
                necController.TakeDame(status.baseDamage);
                return;
            }
            //boss chinh map 1
            Boss1Controller boss1HP = enemy.GetComponent<Boss1Controller>();
            if (boss1HP != null)
            {

                ShowTextDame(status.baseDamage);
                boss1HP.TakeDame(status.baseDamage);
                return;
            }
            //enemy map 2 1 + 2
            EnemyMap2_HP enemyMap2_1 = enemy.GetComponent<EnemyMap2_HP>();
            if (enemyMap2_1 != null)
            {

                ShowTextDame(status.baseDamage);
                enemyMap2_1.TakeDamage(status.baseDamage);
                return;
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

        Debug.Log("Enemy hoạt động lại");
    }

    private IEnumerator UnfreezeAfterDelay(GameObject enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnfreezeEnemy(enemy);
        Destroy(gameObject);
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
