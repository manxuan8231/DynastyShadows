using UnityEngine;
using UnityEngine.UI;

public class EnemyMap2_HP : MonoBehaviour
{
    //xử lý máu
    [SerializeField] public Slider sliderHp;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 2000f;
    //drop exp
    [SerializeField] public GameObject expPrefab;
    EnemyMap2_1 enemyMap2_1;
    public BoxCollider boxDame;
    public NPCQuest npc;

    void OnEnable()
    {
        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại
    }
    private void Awake()
    {
        npc = FindFirstObjectByType<NPCQuest>();
        enemyMap2_1 = GetComponent<EnemyMap2_1>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemyMap2_1 = GetComponent<EnemyMap2_1>();

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        if (enemyMap2_1.currentState == EnemyMap2_1.EnemyState.Death) return; // Nếu chết rồi thì bỏ qua

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {

            enemyMap2_1.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemyMap2_1.enabled = true; // Bật lại Enemy1 để có thể chơi animation chết
            enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Death);
            enemyMap2_1.agent.isStopped = true; // Dừng lại khi chết
            boxDame.enabled = false;
            GameObject exp = Instantiate(expPrefab, transform.position, Quaternion.identity);

            if (npc != null)
            {
                npc.UpdateKillQuest(); // Đánh dấu đã giết được enemy
            }
            ObjPoolingManager.Instance.ReturnToPool("EnemyMap2_2", gameObject);
        }
        if (currentHealth > 0)
        {
            enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }


    }
    void BackToChase()
    {
        if (enemyMap2_1.currentState != EnemyMap2_1.EnemyState.Death)
        {
            float dist = Vector3.Distance(transform.position, enemyMap2_1.player.position);
            if (dist <= enemyMap2_1.attackRange)
            {
                enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Attack);
            }
            else
            {
                enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Run);
            }
        }
    }

    void ResetEnemy()
    {

        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;

        boxDame.enabled = true;
        if (enemyMap2_1.animator != null)
        {
            enemyMap2_1.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemyMap2_1.animator.Update(0f);
        }

        if (enemyMap2_1.agent != null)
        {
            enemyMap2_1.agent.ResetPath();
            enemyMap2_1.agent.enabled = true;
        }
        enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Idle);
    }


}

