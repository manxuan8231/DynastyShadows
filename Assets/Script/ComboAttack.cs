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
    public bool isAttack = false;

    [SerializeField] private GameObject effectAttack1;
    //[SerializeField] private GameObject effectAttack2;
    [SerializeField] private GameObject effectAttack3;
    [SerializeField] private GameObject effectAttackFly3;
    //
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private Transform hitPosition;

    //audio
    private AudioSource audioSource;
    public AudioClip slashSound1;
    public AudioClip slashSound2;
    public AudioClip slashSound3;
    public AudioClip slashSoundFly;

    //Goi ham
    PlayerStatus playerStatus;
    PlayerController playerController;
    void Start()
    {
        animator = GetComponent<Animator>();
        effectAttack1.SetActive(false);
       
        effectAttack3.SetActive(false);
        effectAttackFly3.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    void Update()
    {
        // Chỉ cho phép tấn công nếu đã hết thời gian cooldown
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime 
            && !isAttack && playerStatus.currentMana > 100 && playerController.IsGrounded())
        {
            playerStatus.TakeMana(100);
            OnAttack();
        }
        //tấn công khi ko chạm đất
        if (Input.GetMouseButtonDown(0)
            && !isAttack && playerStatus.currentMana > 100 && playerController.IsGrounded() == false)
        {
            OnAttackFly();
        }
        // Nếu người chơi không nhấn trong 1.2s thì reset combo
        if (Time.time >= nextAttackTime + 1.2f)
        {
            comboStep = 0;
        }
    }

    void OnAttack()
    {
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
            comboStep = 0; // Reset combo sau đòn 3
        }
        else
        {
            comboStep = 0;
        }
    }

    void OnAttackFly()
    {
        animator.SetTrigger("FlyAttack");
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
    //
   /* public void StartEffectAttack2()
    {
        effectAttack2.SetActive(true);
    }
    public void EndEffectAttack2()
    {
        effectAttack2.SetActive(false);
    }*/
   //
    public void StartEffectAttack3()
    {
        effectAttack3.SetActive(true);
    }
    public void EndEffectAttack3()
    {
        effectAttack3.SetActive(false);
    }
    //
    public void StartEffectAttackFly3()
    {
        effectAttackFly3.SetActive(true);
    }
    public void EndEffectAttackFly3()
    {
        effectAttackFly3.SetActive(false);
    }

    //sound even
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
}
