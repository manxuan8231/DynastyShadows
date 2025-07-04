using UnityEngine;

public class ThirdPersonOrbitCamera : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public Vector3 offset = new Vector3(0, 2, -5);
    private Vector3 defaultOffset;
    private Vector3 currentOffset;

    public float sensitivityX = 4f;
    public float sensitivityY = 2f;
    public float minYAngle = -35f;
    public float maxYAngle = 60f;

    private float currentX = 0f;
    private float currentY = 10f;
    private bool cursorVisible = false;

    public bool flipCamera = false;

    public LayerMask groundLayer;
    public float cameraCollisionBuffer = 0.2f;
    public float smoothSpeed = 10f; // tốc độ mượt

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        defaultOffset = offset;
        currentOffset = offset;
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Vector3 desiredCameraPos = target.position + rotation * defaultOffset;
        Vector3 direction = desiredCameraPos - target.position;
        float maxDistance = defaultOffset.magnitude;

        // Kiểm tra va chạm
        if (Physics.Raycast(target.position, direction.normalized, out RaycastHit hit, maxDistance + cameraCollisionBuffer, groundLayer))
        {
            float hitDistance = Mathf.Clamp(hit.distance - cameraCollisionBuffer, 0.5f, maxDistance);
            Vector3 hitOffset = direction.normalized * hitDistance;

            // Lerp từ currentOffset đến hitOffset (mượt)
            currentOffset = Vector3.Lerp(currentOffset, hitOffset, Time.deltaTime * smoothSpeed);
        }
        else
        {
            // Lerp từ currentOffset về offset gốc
            currentOffset = Vector3.Lerp(currentOffset, defaultOffset, Time.deltaTime * smoothSpeed);
        }

        transform.position = target.position + rotation * currentOffset;
        transform.LookAt(target.position + Vector3.up * 1.5f);

        if (flipCamera && player != null)
        {
            player.rotation = Quaternion.Euler(0, currentX, 0);
        }
    }
}
