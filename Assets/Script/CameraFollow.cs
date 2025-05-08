using UnityEngine;

public class ThirdPersonOrbitCamera : MonoBehaviour
{
    public Transform target;          
    public Vector3 offset = new Vector3(0, 2, -5); // Khoảng cách camera so với player
    public float sensitivityX = 4f;    // Nhạy trục ngang (xoay quanh player)
    public float sensitivityY = 2f;    // Nhạy trục dọc (ngẩng lên/ngửa xuống)
    public float minYAngle = -35f;     // Góc nhìn xuống tối đa
    public float maxYAngle = 60f;      // Góc nhìn lên tối đa

    private float currentX = 0f;       // Góc xoay ngang
    private float currentY = 10f;      // Góc xoay dọc

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột trong màn hình
    }

    void LateUpdate()
    {
        // Nhận input từ chuột
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Tính vị trí mới của camera
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f); // Nhìn về đầu nhân vật
    }
}
