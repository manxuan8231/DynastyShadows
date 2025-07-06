using UnityEngine;

public class Skill2Explo : MonoBehaviour
{
    public GameObject textDame;           // Prefab hiệu ứng số damage
    public Transform textTransform;       // Vị trí hiển thị damage text
   

    public float damage = 1000f; 

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
            IDamageable iDame = other.GetComponent<IDamageable>();
            if (iDame != null) {
                ShowTextDame(finalDamage);
            iDame.TakeDamage(finalDamage);
            }
           
            Destroy(gameObject,1f);
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
