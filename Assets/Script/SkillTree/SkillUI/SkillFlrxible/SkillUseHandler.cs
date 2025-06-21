using UnityEngine;

public class SkillUseHandler : MonoBehaviour
{
    public GameObject player;
   
    public float lastColldown = -10f;
    public float cooldownTime = 10f;

    //tham chieu
    public PlayerStatus playerStatus;
    public PlayerControllerState playerControllerState;
    
    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerControllerState = FindAnyObjectByType<PlayerControllerState>();

    }
    void Update()
    {
        string skillID = playerStatus.equipSkillID;
        switch (skillID)
        {
            case "CauLua":
                if (Input.GetKeyDown(KeyCode.R) && Time.time >= lastColldown + cooldownTime) 
                {
                   
                    GameObject enemy = FindEnemy(); // tìm enemy gần nhất để xoay người chơi
                    playerControllerState.animator.SetTrigger("Skill1FE");
                    // Trong SkillUseHandler, khi bắn FireBall
                    foreach (Skill3ClonePLayer clone in FindObjectsOfType<Skill3ClonePLayer>())
                    {
                        clone.PlaySkill1Anim();
                    }

                    lastColldown = Time.time;
                }
                break;
            case "DongCung2":
            case "DongCung3":
            case "DongCung4":

            default:
            Debug.Log("Chưa trang bị kỹ năng!");
                break;
            }
        }
    public GameObject FindEnemy()
    {
        float radius = 50f;
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider col in colliders)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = col.gameObject;
            }
        }

        // Nếu tìm được enemy, xoay player về phía enemy
        if (closestEnemy != null && player != null)
        {
            Vector3 direction = (closestEnemy.transform.position - player.transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                
                Quaternion targetRot = Quaternion.LookRotation(direction);// xoay player về hướng enemy
                player.transform.rotation = targetRot;
            }
        }

        return closestEnemy;
    }




}