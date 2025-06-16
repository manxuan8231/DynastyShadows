using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestPointer : MonoBehaviour
{
    public Transform targetQuest;                 // Vị trí mục tiêu
    public Camera mainCamera;                     // Camera chính
    public RectTransform arrowImage;              // Mũi tên chỉ hướng
    public TextMeshProUGUI distanceText;          // Hiển thị khoảng cách
    public float borderPadding = 50f;             // Viền an toàn màn hình

    public GameObject canvas; // Canvas chứa mũi tên và khoảng cách
    private void Start()
    {
        if (canvas != null)
        {
            canvas.SetActive(true); // Hiển thị canvas khi bắt đầu
        }
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        if (targetQuest == null || mainCamera == null) return;

        // Vị trí mục tiêu trên màn hình
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetQuest.position);

        // Tính khoảng cách
        float distance = Vector3.Distance(mainCamera.transform.position, targetQuest.position);
        distanceText.text = Mathf.RoundToInt(distance) + "m";

        // Nếu mục tiêu nằm sau camera (không thấy được)
        if (screenPos.z < 0)
        {
            screenPos *= -1; // Đảo hướng
        }

        // Giới hạn trong màn hình
        screenPos.x = Mathf.Clamp(screenPos.x, borderPadding, Screen.width - borderPadding);
        screenPos.y = Mathf.Clamp(screenPos.y, borderPadding, Screen.height - borderPadding);

        // Di chuyển mũi tên UI
        arrowImage.position = screenPos;     
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false); // Hiển thị canvas khi người chơi vào vùng trigger
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true); // Ẩn canvas khi người chơi ra khỏi vùng trigger
        }
    }
}
