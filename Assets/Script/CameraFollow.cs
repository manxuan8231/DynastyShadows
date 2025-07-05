using UnityEngine;

public class CameraFollow : MonoBehaviour
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

    public LayerMask enemyLayer;
    public float detectEnemyRange = 10f; // Phạm vi phát hiện enemy


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

       

        transform.position = target.position + rotation * currentOffset;
        transform.LookAt(target.position + Vector3.up * 1.5f);

        if (flipCamera && player != null)
        {
            player.rotation = Quaternion.Euler(0, currentX, 0);
        }
        DetectAndFaceClosestEnemy();

    }

    void DetectAndFaceClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(player.position, detectEnemyRange, enemyLayer);

        if (enemiesInRange.Length == 0) return;

        Transform closestEnemy = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider enemy in enemiesInRange)
        {
            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            // Xoay player về phía enemy
            Vector3 direction = closestEnemy.position - player.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                player.rotation = Quaternion.Slerp(player.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
    }

}
