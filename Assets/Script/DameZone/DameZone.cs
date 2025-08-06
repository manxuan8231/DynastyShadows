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
    CameraShake cameraShake;
    private void Start()
    {
        
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        cameraShake =FindAnyObjectByType<CameraShake>();   
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
            cameraShake.Shake(0.1f);
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
