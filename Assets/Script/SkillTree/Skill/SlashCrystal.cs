using UnityEngine;

public class SlashCrystal : MonoBehaviour
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
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) {
                ShowTextDame(finalDamage);
                damageable.TakeDamage(finalDamage);
            }
            
            GameObject effHit = Instantiate(effectHit, textTransform.position, Quaternion.identity);
            Destroy(effHit, 1f); // Hủy hiệu ứng sau 1 giây
            Destroy(gameObject,0.7f);
        }
        else if(hitObj.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log("Ground da cham");
            GameObject effHit = Instantiate(effectHit, textTransform.position, Quaternion.identity);
            Destroy(effHit, 1f); // Hủy hiệu ứng sau 1 giây
            Destroy(gameObject);
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
