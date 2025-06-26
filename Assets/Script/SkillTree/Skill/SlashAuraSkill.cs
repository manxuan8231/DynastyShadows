using UnityEngine;

public class SlashAuraSkill : MonoBehaviour
{
    public GameObject textDame;
    public Transform textTransform;
    //effect
    public GameObject effectHit;

    public float damage = 100f;
    void Start()
    {

    }


    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;

        // Nếu trúng Enemy
        if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("enemy da cham");
            float finalDamage = damage;
            // Kiểm tra và gọi TakeDamage nếu có
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
            GameObject effHit = Instantiate(effectHit, textTransform.position, Quaternion.identity);
            Destroy(effHit, 1f); // Hủy hiệu ứng sau 1 giây
           
        }
        else if(hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground da cham");
            GameObject effHit = Instantiate(effectHit, textTransform.position, Quaternion.identity);
            Destroy(effHit, 1f); // Hủy hiệu ứng sau 1 giây
           
        }

    }
    // Hàm generic xử lý gây damage
    void TryDealDamage(object target, float dmg)
    {
        if (target == null) return;

        ShowTextDame(dmg);

        switch (target)
        {
            case EnemyHP hp:
                hp.TakeDamage((int)dmg);
                break;
            case EnemyHP2 hp2:
                hp2.TakeDamage((int)dmg);
                break;
            case EnemyHP3 hp3:
                hp3.TakeDamage((int)dmg);
                break;
            case EnemyHP4 hp4:
                hp4.TakeDamage((int)dmg);
                break;
            case DrakonitController d:
                d.TakeDame((int)dmg);
                break;
            case BossHP b:
                b.TakeDamage((int)dmg);
                break;
            case NecController n:
                n.TakeDame((int)dmg);
                break;
            case Boss1Controller b1:
                b1.TakeDame((int)dmg);
                break;
            case EnemyMap2_HP em2:
                em2.TakeDamage((int)dmg);
                break;
            case MinotaurEnemy m:
                m.TakeDamage((int)dmg);
                break;
        }
    }
    public void ShowTextDame(float damage)
    {
        Vector3 posi = new Vector3(0, 2, 0);
        Vector3 enemyPo = textTransform.position + posi;
        GameObject effectText = Instantiate(textDame, enemyPo, Quaternion.identity);
        Destroy(effectText, 0.5f);

        TextDamePopup popup = effectText.GetComponent<TextDamePopup>();
        if (popup != null && popup.text != null)
        {
            popup.text.color = new Color(0.49f, 0.98f, 1f);
            popup.Setup(damage);
        }
    }
}
