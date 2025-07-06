using UnityEngine;

public class FireBallSkill4 : MonoBehaviour
{
    public GameObject impactEffect;       // Hiệu ứng va chạm
    public Transform posi;
    public float destroyDelay = 2f;       // Thời gian hủy hiệu ứng
    public int damage = 5;                // Sát thương gốc

    public GameObject textDame;           // Prefab số damage
    public Transform textTransform;       // Vị trí hiển thị damage text

    PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        if (playerStatus != null)
        {
            damage *= playerStatus.baseDamage;//nhan len 5 lan
        }
        else
        {
            Debug.LogWarning("Không tìm thấy PlayerStatus!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;

        if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy đã bị trúng");

            float finalDamage = damage;

            // Gọi hàm gây damage
           IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) { 
            damageable.TakeDamage(finalDamage);
            }
            SpawnImpact(); 
            Destroy(gameObject);
        }
        else if (hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            SpawnImpact();
            Destroy(gameObject);
        }
    }

    void SpawnImpact()
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, posi.position, Quaternion.identity);
            Destroy(effect, destroyDelay);
        }
    }

    public void ShowTextDame(float damage)
    {
        GameObject effectText = Instantiate(textDame, textTransform.position, Quaternion.identity);
        Destroy(effectText, 0.5f);

        TextDamePopup popup = effectText.GetComponent<TextDamePopup>();
        if (popup != null)
        {
            popup.Setup(damage);
        }
    }

   
}
