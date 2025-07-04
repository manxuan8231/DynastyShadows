using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHorseManHP : MonoBehaviour
{
    //xử lý máu
    [SerializeField] public Slider sliderHp;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 2000f;
    //drop exp
    [SerializeField] public GameObject expPrefab;
    EnemyMap2_horseman enemyMap2_horseman;
    public BoxCollider boxDame;
    public NPCQuest npc;
    public ActiveStartTimeLine6 activeStartTimeLine6;
    void OnEnable()
    {

        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại

    }
    private void Awake()
    {

        npc = FindFirstObjectByType<NPCQuest>();
        activeStartTimeLine6 = FindFirstObjectByType<ActiveStartTimeLine6>();
        enemyMap2_horseman = GetComponent<EnemyMap2_horseman>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemyMap2_horseman = GetComponent<EnemyMap2_horseman>();

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        if (enemyMap2_horseman.currentState == EnemyMap2_horseman.EnemyState.Die) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {

            enemyMap2_horseman.aiPath.enabled = true; // Bật lại NavMeshAgent để có thể chơi animation chết
            enemyMap2_horseman.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemyMap2_horseman.enabled = true; // Bật lại Enemy1 để có thể chơi animation chết
            enemyMap2_horseman.ChangeState(EnemyMap2_horseman.EnemyState.Die);

            enemyMap2_horseman.aiPath.isStopped = true; // Dừng lại khi chết

            boxDame.enabled = false;
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);

            if (npc != null)
            {
                npc.UpdateKillQuest(); // Đánh dấu đã giết được enemy
            }
            if (activeStartTimeLine6 != null)
            {
                activeStartTimeLine6.Count(); // Bật timeline 6
            }
            StartCoroutine(WaitDeath());
        }
        if (currentHealth > 0)
        {
            enemyMap2_horseman.ChangeState(EnemyMap2_horseman.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 1f);
        }


    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(1.5f); // Chờ 1.5 giây trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("EnemyMap2_horseman", gameObject);
    }
    void BackToChase()
    {
        if (enemyMap2_horseman.currentState != EnemyMap2_horseman.EnemyState.Die)
        {
            float dist = Vector3.Distance(transform.position, enemyMap2_horseman.player.position);
            if (dist <= enemyMap2_horseman.attackRange)
            {
                enemyMap2_horseman.ChangeState(EnemyMap2_horseman.EnemyState.Attack);
            }
            else
            {
                enemyMap2_horseman.ChangeState(EnemyMap2_horseman.EnemyState.Walk);
            }
        }
    }

    void ResetEnemy()
    {

        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;

        boxDame.enabled = true;
        if (enemyMap2_horseman.animator != null)
        {
            enemyMap2_horseman.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemyMap2_horseman.animator.Update(0f);
        }

        if (enemyMap2_horseman.aiPath != null)
        {
            enemyMap2_horseman.aiPath.canMove = false;
            enemyMap2_horseman.aiPath.enabled = true;
        }
        enemyMap2_horseman.ChangeState(EnemyMap2_horseman.EnemyState.Idle);
    }


}

