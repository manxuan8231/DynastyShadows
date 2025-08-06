using UnityEngine;

public class SlashAuraSkill : MonoBehaviour
{
    public GameObject textDame;
    public Transform textTransform;
    //effect
    public GameObject effectHit;

    public float damage = 100f;

    //tham chieu 
    CameraShake cameraShake;
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>(); // Tìm đối tượng CameraShake trong cảnh
    }


    void Update()
    {
        cameraShake.Shake(0.3f); // Gọi hàm Shake từ CameraShake để thực hiện rung camera
    }
    public void OnTriggerEnter(Collider other)
    {
        GameObject hitObj = other.gameObject;

        // Nếu trúng Enemy
        if (hitObj.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("enemy da cham");
            float finalDamage = damage;
           IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null) { 
             damageable.TakeDamage(finalDamage);
                ShowTextDame(finalDamage);
            }
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
