using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Tham số ------------------------------")]
    [SerializeField] private float walkSpeed = 5f;
                     private float runSpeed;
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Kiểm tra mặt đất -----------------------")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Thành phần -------------------------------")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CharacterController controller;
    public Animator animator;
    [SerializeField] private AudioSource audioSource;
   
    [Header("Leo tường -----------------------------")]
    
    //cham dat
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool isRunning = false;
    public bool isController = true;
    //cooldown roll
    private float rollColdownTime = 0f;
    //audio
    [SerializeField] private AudioClip audioJump;
    [SerializeField] private AudioClip audioRoll;
    [SerializeField] private AudioClip audioMovemen;
    //goi ham
    PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        audioSource = GetComponent<AudioSource>();
        runSpeed = playerStatus.speedRun;// tốc độ chạy
    }
    void Update()
    {
        if (isController == true)
        {
            Move();
            Jump();
            Roll();
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);

        }
    }

    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift) && playerStatus.currentMana > 0;
            float speed = isRunning ? runSpeed : walkSpeed;
            
            
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isWalking", !isRunning);

            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * inputDirection.z + camRight * inputDirection.x;
            controller.Move(moveDir * speed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), 0.15f);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
       
    }

    public void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        //
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && playerStatus.currentMana > 50)
        {
            playerStatus.TakeMana(50);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioSource.PlayOneShot(audioJump);
            animator.SetTrigger("jump");
        }
        //
        animator.SetBool("jumpLand", isGrounded && !wasGroundedLastFrame == true);

        wasGroundedLastFrame = isGrounded;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Roll()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && isGrounded && playerStatus.currentMana > 100 && Time.time >= rollColdownTime + 2f)
        {
            playerStatus.TakeMana(100);
            audioSource.PlayOneShot(audioRoll);
            animator.SetTrigger("Roll");
            rollColdownTime = Time.time;
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    //sound even 
    public void SoundMovemen()
    {
        audioSource.PlayOneShot(audioMovemen);
    }
}
