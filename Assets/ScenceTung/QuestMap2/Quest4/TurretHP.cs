using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretHP : MonoBehaviour
{
    //xử lý máu
    [SerializeField] public Slider sliderHp;
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth = 2000f;
    public GameObject model;
    public GameObject effect;
    public BoxCollider boxCollider;
    public Turret Turret;

    void Start()
    {
        Turret = GameObject.Find("tru").GetComponent<Turret>();
        effect.SetActive(false);
        currentHealth = maxHealth;
        sliderHp.maxValue = currentHealth;
        sliderHp.value = currentHealth;
    }

   public void TakeDame(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        sliderHp.value = currentHealth;
        if (currentHealth <= 0)
        {
          model.SetActive(false); // Ẩn mô hình khi máu về 0
         StartCoroutine(Dead()); // Bắt đầu coroutine để xử lý khi chết
         


        }
    }
    IEnumerator Dead()
    {
        Turret.enabled = false; // Đánh dấu turret đã chết
        boxCollider.enabled = false; // Vô hiệu hóa BoxCollider khi máu về 0    
        sliderHp.gameObject.SetActive(false); // Ẩn thanh máu
        effect.SetActive(true); // Hiện hiệu ứng khi máu về 0
        yield return new WaitForSeconds(2f); // Chờ 2 giây trước khi hủy đối tượng
        effect.SetActive(false); // Tắt hiệu ứng
        RunTimeLineQuest4 timeLine = FindAnyObjectByType<RunTimeLineQuest4>();
        timeLine.Active(); // Kích hoạt timeline khi máu về 0
        Destroy(gameObject); // Hủy đối tượng

    }
}