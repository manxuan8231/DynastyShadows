using Pathfinding;
using UnityEngine;

public class DameBallTarget : MonoBehaviour, IDamageable
{
    public GameObject player;
    public float speed;

    public float counter;
    //tham chieu
    public PlayerStatus status;
    
    void Start()
    {
        
        status = FindAnyObjectByType<PlayerStatus>();
        player = GameObject.FindWithTag("PlayerTarget");
    }


    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist > 100f)
        {
            Destroy(gameObject);
            return;
        }
        // Tính hướng bay
        Vector3 direction = (player.transform.position - transform.position).normalized;

        // Nếu bị đánh thì bay ngược lại
        if (counter >= 1)
        {
            direction *= -1f;
        }

       
        transform.position += direction * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
                status.TakeHealth(100,gameObject);
                status.TakeHealShield(20);
                Destroy(gameObject);

        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject,8f);
        }

    }

    public void TakeDamage(float amon)
    {
        counter++;
    }
}
