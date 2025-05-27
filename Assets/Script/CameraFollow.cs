using UnityEngine;
using System.Linq;

public class ThirdPersonOrbitCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float sensitivityX = 4f;
    public float sensitivityY = 2f;
    public float minYAngle = -35f;
    public float maxYAngle = 60f;

    private float currentX = 0f;
    private float currentY = 10f;

    private bool cursorVisible = false;

    [Header("Lock-on --------------------------")]
    public float lockOnRange = 50f;
    public string enemyTag = "Enemy";
    private Transform lockTarget;
    private bool isLocking = false;



    void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Ẩn/hiện chuột khi bấm L
        if (Input.GetKeyDown(KeyCode.L))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Ghim enemy khi click chuột giữa
        if (Input.GetMouseButtonDown(2))
        {
            if (!isLocking)
            {
                FindNearestEnemy();
                if (lockTarget != null)
                {
                    isLocking = true;


                    // Cập nhật currentX để camera nhìn ngay enemy
                    Vector3 toEnemy = lockTarget.position - target.position;
                    currentX = Quaternion.LookRotation(toEnemy).eulerAngles.y;
                }
            }
            else
            {
                isLocking = false;
                lockTarget = null;

            }
        }

        // Xử lý input xoay camera
        if (Cursor.lockState == CursorLockMode.Locked && !isLocking)
        {
            currentX += Input.GetAxis("Mouse X") * sensitivityX;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY;
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }

        // Tính vị trí camera theo góc xoay
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = desiredPosition;
        transform.LookAt(target.position + Vector3.up * 1.5f);

        // Nếu đang ghim enemy
        if (isLocking && lockTarget != null)
        {
            // Xoay player nhìn về enemy
            Vector3 dir = lockTarget.position - target.position;
            dir.y = 0f;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            target.rotation = Quaternion.Slerp(target.rotation, lookRot, Time.deltaTime * 10f);

            // Cập nhật góc camera để nhìn đúng enemy
            Vector3 toEnemy = lockTarget.position - target.position;
            currentX = Quaternion.LookRotation(toEnemy).eulerAngles.y;

            // Cập nhật vị trí UI indicator



        }
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0) return;

        lockTarget = enemies
            .Select(e => e.transform)
            .OrderBy(t => Vector3.Distance(target.position, t.position))
            .FirstOrDefault(t => Vector3.Distance(target.position, t.position) <= lockOnRange);
    }



    public bool IsLockingEnemy()
    {
        return isLocking;
    }
}
