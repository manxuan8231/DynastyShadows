using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DameZone : MonoBehaviour
{
    //effect hit
    [SerializeField] private GameObject effectHit;
    [SerializeField] private Transform tranFormHit;

   //hien dame effect
    public GameObject textDame;
    public Transform textTransform;
    //
    public Collider dameZoneCollider;
    public string tagEnemy;
    public List<Collider> listDame = new List<Collider>();

    //ke thua
    PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tagEnemy) && !listDame.Contains(other))
        {
            float finalDamage = playerStatus.CalculateFinalDamage();

            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect,1f);
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
            if(bossHP != null)
            {
                listDame.Add(other);
                ShowTextDame(finalDamage);
                bossHP.TakeDamage(finalDamage);
                return;
            }

        }
    }
    private void OnTriggerStay(Collider other)//nếu ontrigger xử lấy ko kịp thì nó dô đây xử lý tiếp
    {
        if (other.gameObject.CompareTag(tagEnemy) && !listDame.Contains(other))
        {
            float finalDamage = playerStatus.CalculateFinalDamage();
            GameObject hitEffect = Instantiate(effectHit, tranFormHit.position, transform.rotation);
            Destroy(hitEffect, 1f);
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

            DrakonitController drakonitController = other.GetComponent<DrakonitController>();
            if (drakonitController != null)
            {
                listDame.Add(other);// thêm collider vào danh sách
                ShowTextDame(finalDamage);
                drakonitController.TakeDame(finalDamage);
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
