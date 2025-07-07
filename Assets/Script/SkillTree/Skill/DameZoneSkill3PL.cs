using Unity.VisualScripting;
using UnityEngine;

public class DameZoneSkill3PL : MonoBehaviour
{
    public BoxCollider boxCollider;
    public LayerMask layerMask;
    public float finalDamage;
    //hien dame effect
    public GameObject textDame;
    public Transform textTransform;
    //tham chieu
    public PlayerStatus playerStatus;
    Skill3Manager skillManager;
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        skillManager = FindAnyObjectByType<Skill3Manager>();
    }

   
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if ((layerMask.value & (1 << other.gameObject.layer)) != 0)
        {
            if (skillManager.isDamaged == false)
            {
                finalDamage = playerStatus.CalculateFinalDamage();//dame
            }
            else if (skillManager.isDamaged == true) {
                finalDamage = playerStatus.CalculateFinalDamage() * 2;//dame
            }

            // Tìm script EnemyHP 
            if (other.TryGetComponent<IDamageable>(out var damageable))
            { 
                damageable.TakeDamage(finalDamage);
                ShowTextDame(finalDamage);
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
        Debug.Log("bat box");
        boxCollider.enabled = true;
    }

    public void endDame()
    {
        boxCollider.enabled = false;
    }
}
