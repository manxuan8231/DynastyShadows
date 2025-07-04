using Unity.VisualScripting;
using UnityEngine;

public class DameZoneSkill3PL : MonoBehaviour
{
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    public float finalDamage;
    //hien dame effect
    public GameObject textDame;
    public Transform textTransform;
    //tham chieu
    public PlayerStatus playerStatus;
    Skill3Manager skillManager;
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        skillManager = FindAnyObjectByType<Skill3Manager>();
    }

   
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (skillManager.isDamaged == false)
            {
                finalDamage = playerStatus.CalculateFinalDamage();//dame
            }
            else if (skillManager.isDamaged == true) {
                finalDamage = playerStatus.CalculateFinalDamage() * 2;//dame
            }

            // Tìm script EnemyHP 
            EnemyHP enemyHP = other.GetComponent<EnemyHP>();
            if (enemyHP != null)
            {
              
                ShowTextDame(finalDamage);
                enemyHP.TakeDamage(finalDamage);
                return;
            }

            EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
            if (enemyHP2 != null)
            {
              
                ShowTextDame(finalDamage);
                enemyHP2.TakeDamage(finalDamage);
                return;
            }

            EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
            if (enemyHP3 != null)
            {
              
                ShowTextDame(finalDamage);
                enemyHP3.TakeDamage(finalDamage);
                return;
            }

            EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
            if (enemyHP4 != null)
            {
              
                ShowTextDame(finalDamage);
                enemyHP4.TakeDamage(finalDamage);
                return;
            }
            //boss drakonit
            DrakonitController drakonitController = other.GetComponent<DrakonitController>();
            if (drakonitController != null)
            {
              
                ShowTextDame(finalDamage);
                drakonitController.TakeDame(finalDamage);
                return;
            }
            //boss ork
            BossHP bossHP = other.GetComponent<BossHP>();
            if (bossHP != null)
            {
               
                ShowTextDame(finalDamage);
                bossHP.TakeDamage(finalDamage);
                return;
            }
            //boss sa mac
            NecController necController = other.GetComponent<NecController>();
            if (necController != null)
            {
                Debug.Log("Đã trúng NecController");
               
                ShowTextDame(finalDamage);
                necController.TakeDame(finalDamage);
                return;
            }
            //boss chinh map 1
            Boss1Controller boss1HP = other.GetComponent<Boss1Controller>();
            if (boss1HP != null)
            {
               
                ShowTextDame(finalDamage);
                boss1HP.TakeDame((int)finalDamage);
                return;
            }
            //enemy map 2 1 + 2
            EnemyMap2_HP enemyMap2_1 = other.GetComponent<EnemyMap2_HP>();
            if (enemyMap2_1 != null)
            {
              
                ShowTextDame(finalDamage);
                enemyMap2_1.TakeDamage(finalDamage);
                return;
            }
            //Minotaur
            MinotaurEnemy minotaurController = other.GetComponent<MinotaurEnemy>();
            if (minotaurController != null)
            {

                ShowTextDame(finalDamage);
                minotaurController.TakeDamage(finalDamage);
                return;
            }
            DragonRedHP dragonRedHP = other.GetComponent<DragonRedHP>();
            if (dragonRedHP != null)
            {

                ShowTextDame(finalDamage);
                dragonRedHP.TakeDamage(finalDamage);
                return;
            }
        }
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

    public void beginDame()
    {
        Debug.Log("bat box");
        boxCollider.enabled = true;
    }

    public void endDame()
    {
        boxCollider.enabled = false;
    }
}
