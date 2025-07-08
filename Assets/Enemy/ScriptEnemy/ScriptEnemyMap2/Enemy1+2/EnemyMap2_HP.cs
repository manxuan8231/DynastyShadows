using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMap2_HP : MonoBehaviour, IDamageable
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
    public ActiveStartTimeLine6 activeStartTimeLine6;
    void OnEnable()
    {

        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại

    }
    private void Awake()
    {

        npc = FindFirstObjectByType<NPCQuest>();
        activeStartTimeLine6 = FindFirstObjectByType<ActiveStartTimeLine6>();
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
           
                  
            boxDame.enabled = false;
            enemyMap2_1.ai.canSearch = false;
            enemyMap2_1.ai.canMove = false; // Tắt di chuyển khi chết
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
            enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Death);

        }
        if (currentHealth > 0)
        {
            enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 0.2f);
        }


    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(5f); // Chờ 1.5 giây trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("EnemyMap2_1", gameObject);
        enemyMap2_1.hasFirstPos = false; // Đặt lại hasFirstPos để có thể spawn lại
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

        enemyMap2_1.ChangeState(EnemyMap2_1.EnemyState.Idle);
    }


}

