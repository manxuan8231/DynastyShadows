using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 500;
    private Transform target;

    private PlayerStatus playerStatus;
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        
    }
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        if (playerStatus != null)
        {
            playerStatus.TakeHealth(damage,gameObject);
            playerStatus.TakeHealShield(20);
        }
        Destroy(gameObject);
    }
}
