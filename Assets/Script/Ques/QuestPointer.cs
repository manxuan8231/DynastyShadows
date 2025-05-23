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

    private void Start()
    {
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

        // Tính góc xoay dựa trên hướng nhìn màn hình
       // Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
       // Vector3 direction = screenPos - screenCenter;

        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Xoay mũi tên chỉ về phía target trên màn hình
       // arrowImage.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
