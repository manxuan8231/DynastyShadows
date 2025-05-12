using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    private Animator animator;

    [Header("Thời gian cooldown cho mỗi đòn combo")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.6f;
    [SerializeField] private float attack3Cooldown = 0.7f;

    private int comboStep = 0;
    private float nextAttackTime = 0f;
    public bool isAttack = true;
    private float coolDownAttackFly = 0f;

    //effect slash
    [SerializeField] private GameObject effectAttack1;
    //[SerializeField] private GameObject effectAttack2;
    [SerializeField] private GameObject effectAttack3;
    [SerializeField] private GameObject effectAttackFly3;

   

    //audio
    private AudioSource audioSource;
    public AudioClip slashSound1;
    public AudioClip slashSound2;
    public AudioClip slashSound3;
    public AudioClip slashSoundFly;

    //Goi ham
    PlayerStatus playerStatus;
    PlayerController playerController;

    //damezone
    public BoxCollider boxColliderDameZone;
    public BoxCollider boxColliderDameZoneHit;
    void Start()
    {
        animator = GetComponent<Animator>();
        effectAttack1.SetActive(false);
       
        effectAttack3.SetActive(false);
        effectAttackFly3.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerController = FindAnyObjectByType<PlayerController>();

        boxColliderDameZone.enabled = false;
        boxColliderDameZoneHit.enabled = false;
    }

    void Update()
    {
        // Chỉ cho phép tấn công nếu đã hết thời gian cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime 
            && isAttack && playerStatus.currentMana > 100 && playerController.IsGrounded())
        {
            playerStatus.TakeMana(100);
            OnAttack();
        }
        //tấn công khi ko chạm đất
        if (Input.GetMouseButtonDown(0)
            && isAttack && playerStatus.currentMana > 100 && playerController.IsGrounded() == false && Time.time >= coolDownAttackFly + 1f)
        {
            OnAttackFly();
            coolDownAttackFly = Time.time;
        }
        // Nếu người chơi không nhấn trong 1.2s thì reset combo
        if (Time.time >= nextAttackTime + 1.2f)
        {
            comboStep = 0;
        }
    }

    void OnAttack()
    {
        // Tìm và quay về hướng enemy gần nhất
        GameObject closestEnemy = FindClosestEnemy(7f);
        if (closestEnemy != null)
        {
            Vector3 lookDirection = closestEnemy.transform.position - transform.position;
            lookDirection.y = 0; // Giữ player không ngẩng lên/ngửa xuống
            transform.forward = lookDirection.normalized;
        }

        comboStep++;

        if (comboStep == 1)
        {
            animator.SetTrigger("Attack1");
            nextAttackTime = Time.time + attack1Cooldown;
        }
        else if (comboStep == 2)
        {
            animator.SetTrigger("Attack2");
            nextAttackTime = Time.time + attack2Cooldown;
        }
        else if (comboStep == 3)
        {
            animator.SetTrigger("Attack3");
            nextAttackTime = Time.time + attack3Cooldown;
            comboStep = 0;
        }
        else
        {
            comboStep = 0;
        }
    }
    void OnAttackFly()
    {
        // Tìm và quay về hướng enemy gần nhất
        GameObject closestEnemy = FindClosestEnemy(10f);
        if (closestEnemy != null)
        {
            Vector3 lookDirection = closestEnemy.transform.position - transform.position;
            lookDirection.y = 0; // Giữ player không ngẩng lên/ngửa xuống
            transform.forward = lookDirection.normalized;
        }
        animator.SetTrigger("FlyAttack");
    }
    private GameObject FindClosestEnemy(float range)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(currentPosition, enemy.transform.position);
            if (distance < minDistance && distance <= range)
            {
                minDistance = distance;
                closest = enemy;
            }
        }

        return closest;
    }


    //effect even
    public void StartEffectAttack1()
    {
        effectAttack1.SetActive(true);
    }
    public void EndEffectAttack1()
    {
        effectAttack1.SetActive(false);
    }
   
   /* public void StartEffectAttack2()
    {
        effectAttack2.SetActive(true);
    }
    public void EndEffectAttack2()
    {
        effectAttack2.SetActive(false);
    }*/
 
    public void StartEffectAttack3()
    {
        effectAttack3.SetActive(true);
    }
    public void EndEffectAttack3()
    {
        effectAttack3.SetActive(false);
    }
   
    public void StartEffectAttackFly3()
    {
        effectAttackFly3.SetActive(true);
    }
    public void EndEffectAttackFly3()
    {
        effectAttackFly3.SetActive(false);
    }

    //sounds even
    public void PlaySlashSound1()
    {
        audioSource.PlayOneShot(slashSound1);
    }
    public void PlaySlashSound2()
    {
        audioSource.PlayOneShot(slashSound2);
    }
    public void PlaySlashSound3()
    {
        audioSource.PlayOneShot(slashSound3);
    }
    public void PlaySlashSoundFly()
    {
        audioSource.PlayOneShot(slashSoundFly);
    }

    //damezone box
    public void StartDameZone()
    {
        boxColliderDameZone.enabled = true;
    }
    public void EndDameZone()
    {
        boxColliderDameZone.enabled = false;
    }
    public void StartDameZoneHit()
    {
        boxColliderDameZoneHit.enabled = true;
    }
    public void EndDameZoneHit()
    {
        boxColliderDameZoneHit.enabled = false;
    }
}
