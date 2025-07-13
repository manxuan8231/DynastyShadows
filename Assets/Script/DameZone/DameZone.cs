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
    public List<Collider> listDame = new List<Collider>();

    //ke thua
    PlayerStatus playerStatus;
   
    private void Start()
    {
        
        playerStatus = FindAnyObjectByType<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        TryApplyDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryApplyDamage(other);
    }
    private void TryApplyDamage(Collider other)
    {
        GameObject hitObj = other.gameObject;
        LayerMask allowedLayers = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("InvisibleAssasin"));
        if ((allowedLayers & (1 << other.gameObject.layer)) == 0) return;


        if (listDame.Contains(other)) return;

        float finalDamage = playerStatus.CalculateFinalDamage();

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            listDame.Add(other);
            ShowTextDame(finalDamage);
            damageable.TakeDamage(finalDamage);
        }
        //cua phong 
        if (!other.CompareTag("EnemyHorseMan")) return;     
        if (other.TryGetComponent<EnemyMap2_horseman>(out var enemy))
        {           
            enemy.TryDodge(); // Gọi hành vi né đòn
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
