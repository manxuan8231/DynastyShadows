using UnityEngine;

public class FireBallSkill4 : MonoBehaviour
{
    public GameObject impactEffect;       // Hiệu ứng va chạm
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
            damage *= playerStatus.baseDamage;
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
            TryDealDamage(hitObj.GetComponent<EnemyHP>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<EnemyHP2>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<EnemyHP3>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<EnemyHP4>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<DrakonitController>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<BossHP>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<NecController>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<Boss1Controller>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<EnemyMap2_HP>(), finalDamage);
            TryDealDamage(hitObj.GetComponent<MinotaurEnemy>(), finalDamage);

            SpawnImpact(other.transform.position); 
            Destroy(gameObject);
        }
        else if (hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            SpawnImpact(other.transform.position);
            Destroy(gameObject);
        }
    }

    void SpawnImpact(Vector3 position)
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, position, Quaternion.identity);
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

    void TryDealDamage(object target, float dmg)
    {
        if (target == null) return;

        ShowTextDame(dmg);

        switch (target)
        {
            case EnemyHP hp: hp.TakeDamage((int)dmg); break;
            case EnemyHP2 hp2: hp2.TakeDamage((int)dmg); break;
            case EnemyHP3 hp3: hp3.TakeDamage((int)dmg); break;
            case EnemyHP4 hp4: hp4.TakeDamage((int)dmg); break;
            case DrakonitController d: d.TakeDame((int)dmg); break;
            case BossHP b: b.TakeDamage((int)dmg); break;
            case NecController n: n.TakeDame((int)dmg); break;
            case Boss1Controller b1: b1.TakeDame((int)dmg); break;
            case EnemyMap2_HP em: em.TakeDamage((int)dmg); break;
            case MinotaurEnemy me: me.TakeDamage((int)dmg); break;
        }
    }
}
