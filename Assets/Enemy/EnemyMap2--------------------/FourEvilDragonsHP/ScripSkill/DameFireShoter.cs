using UnityEngine;

public class DameFireShoter : MonoBehaviour
{
    public float dame = 500f;
    PlayerStatus playerStatus;
    CameraShake cameraShake; // Biến để lưu trữ CameraShake
    public GameObject fireEffect; // Hiệu ứng lửa

    void Start()
    {
        GameObject gameObject = GameObject.Find("Stats");
        if (gameObject != null)
        {
            playerStatus = gameObject.GetComponent<PlayerStatus>();

        }
        cameraShake = FindAnyObjectByType<CameraShake>(); // Tìm đối tượng CameraShake trong scene
    }


    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerStatus != null)
            {
                playerStatus.TakeHealth(dame, gameObject, "HitBack", 1);
                // Hiển thị hiệu ứng lửa
                if (fireEffect != null)
                {
                    GameObject effectInstance = Instantiate(fireEffect, transform.position, Quaternion.identity);
                    Destroy(effectInstance, 2f); // Hủy hiệu ứng sau 2 giây
                }
                cameraShake.Shake();
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground") /*|| other.gameObject.layer == LayerMask.NameToLayer("Obstacle")*/)
        {
            // Hiển thị hiệu ứng lửa khi chạm đất
            if (fireEffect != null)
            {
                GameObject effectInstance = Instantiate(fireEffect, transform.position, Quaternion.identity);
                Destroy(effectInstance, 2f); // Hủy hiệu ứng sau 2 giây
            }
            cameraShake.Shake();
            Destroy(gameObject); // Hủy đối tượng này sau khi chạm đất
        }


    }
}
