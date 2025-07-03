using UnityEngine;
using UnityEngine.UI;

public class ShoterTargetDra : MonoBehaviour
{
    public Slider SliderHp;
    public float maxHp = 100f;
    public float currentHp;

    public GameObject bulletPrefab; // Prefab đạn sẽ bắn
    public Transform transforms;

    public GameObject effectBl; // Prefab đạn sẽ bắn
    public Transform tranEffect;
    void Start()
    {
        currentHp = maxHp;
        SliderHp.maxValue = currentHp;
        SliderHp.value = currentHp;
    }

    // Hàm này được gọi khi bị player đánh
    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        SliderHp.value = currentHp;
        if (currentHp <= 0)
        {
            currentHp = 0;
            GameObject instan = Instantiate(bulletPrefab ,transforms.position,Quaternion.identity);
            Destroy(instan,10f);
            GameObject effect = Instantiate(effectBl, tranEffect.position, Quaternion.identity);
            Destroy(effect, 1.5f);
            Destroy(gameObject); // Tuỳ chọn: Xoá object sau khi phản công
        }
    }

   
}
