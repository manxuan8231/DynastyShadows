using UnityEngine;

public class OpenCursor : MonoBehaviour
{
    private bool isCursorVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isCursorVisible = !isCursorVisible; // Toggle trạng thái

            if (isCursorVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
