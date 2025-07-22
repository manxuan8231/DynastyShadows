using UnityEngine;

public class PetController : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;
    public float moveSpeed = 3f;
    public BuffManager buffManager;

    private float buffCooldown = 5f;
    private float buffTimer;

    void Update()
    {
        FollowPlayer();

        buffTimer += Time.deltaTime;
        if (buffTimer >= buffCooldown)
        {
            buffTimer = 0f;
            TryBuffPlayer();
        }
    }

    void FollowPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > followDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void TryBuffPlayer()
    {
        // Gọi hàm buff nếu người chơi đang thiếu máu hoặc theo logic tùy chỉnh
        buffManager.BuffDamage(); // hoặc BuffArmor(), BuffHP()
    }
}
