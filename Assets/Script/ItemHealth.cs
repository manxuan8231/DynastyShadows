using UnityEngine;

public class ItemHealth : MonoBehaviour
{
    public Transform player;
    public float speed = 5f; // Tốc độ di chuyển về phía người chơi
    public float desiredHeight = 1.5f; // Độ cao mong muốn của item

    [SerializeField] private float timeDestroy;

    private PlayerStatus playerStatus;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        GameObject statsObj = GameObject.Find("Stats");
        if (statsObj != null)
        {
            playerStatus = statsObj.GetComponent<PlayerStatus>();
        }

        Destroy(gameObject, timeDestroy);
    }

    void FixedUpdate()
    {
        // Di chuyển item về phía người chơi
        if (player != null)
        {
            Vector3 targetPosition = new Vector3(player.position.x, desiredHeight, player.position.z);
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerStatus != null)
            {
                playerStatus.AddExp(50); // Tăng exp cho người chơi
                Destroy(gameObject); // Xóa item sau khi nhặt
            }
        }
    }
}
