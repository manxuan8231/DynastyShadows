using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DameZone : MonoBehaviour
{
 

   //hien dame effect
    public GameObject textDame;
    public Transform textTransform;
    //
    public Collider dameZoneCollider;
    public string[] tagEnemy;
    public List<Collider> listDame = new List<Collider>();

    //ke thua
    PlayerStatus playerStatus;
   
    private void Start()
    {
        
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;
        if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
          {
            if (listDame.Contains(other)) return;
            float finalDamage = playerStatus.CalculateFinalDamage();

               
               
                // Tìm script EnemyHP 
                EnemyHP enemyHP = other.GetComponent<EnemyHP>();
                if (enemyHP != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
                if (enemyHP2 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP2.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
                if (enemyHP3 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP3.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
                if (enemyHP4 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP4.TakeDamage(finalDamage);
                    return;
                }
                //boss drakonit
                DrakonitController drakonitController = other.GetComponent<DrakonitController>();
                if (drakonitController != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    drakonitController.TakeDame(finalDamage);
                    return;
                }
                //boss ork
                BossHP bossHP = other.GetComponent<BossHP>();
                if (bossHP != null)
                {
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    bossHP.TakeDamage(finalDamage);
                    return;
                }
                //boss sa mac
                NecController necController = other.GetComponent<NecController>();
                if (necController != null)
                {
                    Debug.Log("Đã trúng NecController");
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    necController.TakeDame(finalDamage);
                    return;
                }
                //boss chinh map 1
                Boss1Controller boss1HP = other.GetComponent<Boss1Controller>();
                if (boss1HP != null)
                {
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    boss1HP.TakeDame((int)finalDamage);
                    return;
                }
                //enemy map 2 1 + 2
                EnemyMap2_HP enemyMap2_1 = other.GetComponent<EnemyMap2_HP>();
                if (enemyMap2_1 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyMap2_1.TakeDamage(finalDamage);
                    return;
                }
                //Minotaur
                MinotaurEnemy minotaurController = other.GetComponent<MinotaurEnemy>();
                if (minotaurController != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    minotaurController.TakeDamage(finalDamage);
                    return;
                }
                TurretHP turretHP = other.GetComponent<TurretHP>();
                if (turretHP != null)
                {
                listDame.Add(other);// thêm collider vào danh sách
                ShowTextDame(finalDamage);
                turretHP.TakeDame(finalDamage);
                return;
                }
                DragonRed dragonRed = other.GetComponent<DragonRed>();
                if (dragonRed != null)
                {
                listDame.Add(other);// thêm collider vào danh sách
                ShowTextDame(finalDamage);
                dragonRed.TakeDame(finalDamage);
                return;
                }   
        }
        
    }
    private void OnTriggerStay(Collider other)//nếu ontrigger xử lấy ko kịp thì nó dô đây xử lý tiếp
    {
        GameObject hitObj = other.gameObject;
        if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
            {
            if (listDame.Contains(other)) return;
            float finalDamage = playerStatus.CalculateFinalDamage();

             
                // Tìm script EnemyHP 
                EnemyHP enemyHP = other.GetComponent<EnemyHP>();
                if (enemyHP != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP2 enemyHP2 = other.GetComponent<EnemyHP2>();
                if (enemyHP2 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP2.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP3 enemyHP3 = other.GetComponent<EnemyHP3>();
                if (enemyHP3 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP3.TakeDamage(finalDamage);
                    return;
                }

                EnemyHP4 enemyHP4 = other.GetComponent<EnemyHP4>();
                if (enemyHP4 != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    enemyHP4.TakeDamage(finalDamage);
                    return;
                }
                //boss drakonit
                DrakonitController drakonitController = other.GetComponent<DrakonitController>();
                if (drakonitController != null)
                {
                    listDame.Add(other);// thêm collider vào danh sách
                    ShowTextDame(finalDamage);
                    drakonitController.TakeDame(finalDamage);
                    return;
                }
                //boss ork
                BossHP bossHP = other.GetComponent<BossHP>();
                if (bossHP != null)
                {
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    bossHP.TakeDamage(finalDamage);
                    return;
                }
                //boss sa mac
                NecController necController = other.GetComponent<NecController>();
                if (necController != null)
                {
                    Debug.Log("Đã trúng NecController");
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    necController.TakeDame(finalDamage);
                    return;
                }
                //boss chinh map 1
                Boss1Controller boss1HP = other.GetComponent<Boss1Controller>();
                if (boss1HP != null)
                {
                    listDame.Add(other);
                    ShowTextDame(finalDamage);
                    boss1HP.TakeDame((int)finalDamage);
                    return;
                }
            TurretHP turretHP = other.GetComponent<TurretHP>();
            if (turretHP != null)
            {
                listDame.Add(other);// thêm collider vào danh sách
                ShowTextDame(finalDamage);
                turretHP.TakeDame(finalDamage);
                return;
            }
            DragonRed dragonRed = other.GetComponent<DragonRed>();
            if (dragonRed != null)
            {
                listDame.Add(other);// thêm collider vào danh sách
                ShowTextDame(finalDamage);
                dragonRed.TakeDame(finalDamage);
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
        listDame.Clear();
        dameZoneCollider.enabled = true;
    }
    public void endDame()
    {
        listDame.Clear();//xóa
        dameZoneCollider.enabled = false;
    }
}
