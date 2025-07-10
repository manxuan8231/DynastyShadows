using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCruTankHP : MonoBehaviour,IDamageable
{
    //xử lý máu
    [SerializeField] public Slider sliderHp;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 2000f;
    public bool isTakedame;
    //drop exp
    [SerializeField] public GameObject expPrefab;
    EnemyCruTank enemyCruTank;
    public BoxCollider boxDame;
    public NPCQuest npc;
    public ActiveStartTimeLine6 activeStartTimeLine6;
    public List<ItemDrop> itemDrops = new List<ItemDrop>();
    void OnEnable()
    {

        ResetEnemy(); // Mỗi lần lấy từ pool ra thì reset lại

    }
    private void Awake()
    {

        npc = FindFirstObjectByType<NPCQuest>();
        activeStartTimeLine6 = FindFirstObjectByType<ActiveStartTimeLine6>();
        enemyCruTank = GetComponent<EnemyCruTank>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
        enemyCruTank = GetComponent<EnemyCruTank>();
        isTakedame = true;
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void DropItem()
    {
        foreach (ItemDrop item in itemDrops)
        {
            float chance = item.dropRate / 100f;
            if (Random.value <= chance) // randon ra số từ 0 đến 1
            {
                Instantiate(item.itemPrefabs, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (enemyCruTank.currentState == EnemyCruTank.EnemyState.Die) return; // Nếu chết rồi thì bỏ qua
        if (!isTakedame) return;
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
            DropItem();
            enemyCruTank.aiPath.enabled = true; // Bật lại NavMeshAgent để có thể chơi animation chết
            enemyCruTank.animator.enabled = true; // Bật animator để có thể chơi animation chết
            enemyCruTank.enabled = true; // Bật lại Enemy1 để có thể chơi animation chết
            enemyCruTank.ChangeState(EnemyCruTank.EnemyState.Die);

            enemyCruTank.aiPath.isStopped = true; // Dừng lại khi chết

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
            enemyCruTank.ChangeState(EnemyCruTank.EnemyState.GetHit);
            // Sau một thời gian nhỏ thì quay lại Run/Attack
            Invoke(nameof(BackToChase), 2f);
        }


    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(1.5f); // Chờ 1.5 giây trước khi trả về pool
        ObjPoolingManager.Instance.ReturnToPool("EnemyCruTank", gameObject);
    }
    void BackToChase()
    {
        if (enemyCruTank.currentState != EnemyCruTank.EnemyState.Die)
        {
            float dist = Vector3.Distance(transform.position, enemyCruTank.player.position);
            if (dist <= enemyCruTank.attackRange)
            {
                enemyCruTank.ChangeState(EnemyCruTank.EnemyState.Attack);
            }
            else
            {
                enemyCruTank.ChangeState(EnemyCruTank.EnemyState.Walk);
            }
        }
    }

    void ResetEnemy()
    {

        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;

        boxDame.enabled = true;
        if (enemyCruTank.animator != null)
        {
            enemyCruTank.animator.Rebind();        // Khôi phục tất cả trạng thái mặc định ban đầu
            enemyCruTank.animator.Update(0f);
        }

        if (enemyCruTank.aiPath != null)
        {
            enemyCruTank.aiPath.canMove = false;
            enemyCruTank.aiPath.enabled = true;
        }
        enemyCruTank.ChangeState(EnemyCruTank.EnemyState.Idle);
    }


}

