using UnityEngine;

public class ThirdPersonOrbitCamera : MonoBehaviour
{
    public Transform target;        // Camera LookAt target (thường là player)
    public Transform player;        // Player cần flip
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivityX = 4f;
    public float sensitivityY = 2f;
    public float minYAngle = -35f;
    public float maxYAngle = 60f;

    private float currentX = 0f;
    private float currentY = 10f;
    private bool cursorVisible = false;

    public bool flipCamera = false; // Cho phép flip liên tục khi true

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Toggle chuột bằng phím L
        if (Input.GetKeyDown(KeyCode.L))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Xử lý xoay camera bằng chuột
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        // Tính toán vị trí và hướng nhìn camera
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);

       
        if (flipCamera && player != null)
        {
            player.rotation = Quaternion.Euler(0, currentX, 0); // Xoay cả X và Y
        }
       
    }
}
