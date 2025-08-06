using UnityEngine;

public class Skill4Explo : MonoBehaviour
{
    public GameObject textDame;           // Prefab hiệu ứng số damage
    public Transform textTransform;       // Vị trí hiển thị damage text


    public float damage = 1000f;
    CameraShake cameraShake; // Biến để lưu trữ đối tượng CameraShake
    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>(); // Tìm đối tượng CameraShake trong cảnh
    }


    void Update()
    {
        cameraShake.Shake(0.2f); // Gọi hàm Shake từ CameraShake để tạo hiệu ứng rung camera
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
                ShowTextDame(damage);
                damageable.TakeDamage(damage);
            }
          

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
