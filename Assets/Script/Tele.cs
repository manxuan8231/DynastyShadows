using UnityEngine;

public class Tele : MonoBehaviour
{
    public Vector3 targetPosition = new Vector3(0, 110, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger zone!");

            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false; // Tắt CharacterController trước khi dịch chuyển
                other.transform.position = targetPosition;
                cc.enabled = true;  // Bật lại CharacterController
            }
            else
            {
                other.transform.position = targetPosition; // Nếu không dùng CC thì cứ dịch chuyển bình thường
            }
        }
    }
}
