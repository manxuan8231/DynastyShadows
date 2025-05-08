using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraTransform;

    void Update()
    {
        // Nhận input người chơi
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Tính hướng di chuyển theo camera
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;

            // Di chuyển
            controller.Move(moveDir * speed * Time.deltaTime);

            // Xoay nhân vật theo hướng di chuyển
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
    }
}
