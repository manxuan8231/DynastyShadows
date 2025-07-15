using Pathfinding.Util;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class StatueQuest8 : MonoBehaviour
{
    [Header("Laser Settings")]
    public float attackRange = 10f;
    public float damagePerSecond = 50f;
  

    public LineRenderer laserLine;
    public Transform firePoint;

    private Transform currentTarget;
    private float currentDamage;
    private PlayerStatus playerStatus;
    public float cooldownHit;

    void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
        currentDamage = damagePerSecond;

        playerStatus = GameObject.Find("Stats").GetComponent<PlayerStatus>();
    }

    void Update()
    {
        FindTarget();

        if (currentTarget != null)
        {
            FireLaser();
        }
        else
        {
            StopLaser();
        }
    }

    void FindTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        float shortestDistance = Mathf.Infinity;
        GameObject nearest = null;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < shortestDistance && dist <= attackRange)
            {
                shortestDistance = dist;
                nearest = player;
            }
        }

        currentTarget = nearest != null ? nearest.transform : null;
    }

    void FireLaser()
    {
        if (currentTarget == null) return;

        // Bật tia
        if (!laserLine.enabled)
        {
            laserLine.enabled = true;
            currentDamage = damagePerSecond; // reset nếu mới bắt đầu
        }

        // Vẽ tia
        laserLine.SetPosition(0, firePoint.position);
        laserLine.SetPosition(1, currentTarget.position);

      
        // Gây damage
        if (playerStatus != null)
        {
            float actualDamage = currentDamage * Time.deltaTime;
         playerStatus.TakeHealth(actualDamage,gameObject,"Hit", cooldownHit);
            playerStatus.TakeHealShield(0.1f);
        }
    }

    void StopLaser()
    {
        if (laserLine.enabled)
        {
            laserLine.enabled = false;
            currentDamage = damagePerSecond;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
