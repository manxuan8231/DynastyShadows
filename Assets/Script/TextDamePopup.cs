using TMPro;
using UnityEngine;

public class TextDamePopup : MonoBehaviour
{
    public TextMeshProUGUI text; // Ô hiển thị damage
    public float moveSpeed = 1f; // Tốc độ bay lên
    public float lifeTime = 1f; // Thời gian tồn tại

    private Camera mainCamera;

    public void Setup(float damage)
    {
        text.text = damage.ToString();

        Destroy(gameObject, lifeTime);
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Di chuyển lên
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime, Space.World);

        // Luôn quay mặt về camera
        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }
}
