
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerControllerState : MonoBehaviour
{
    [Header("Tham số ------------------------------")]
    public float walkSpeed = 5f;
    public float runSpeed = 15f;
    public float jumpHeight = 5f;
    public float gravity = -9.81f;

    [Header("Kiểm tra mặt đất -----------------------")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public LayerMask enemyLayer;
    //cham dat
    public Vector3 velocity;
    public bool isGrounded;
    public bool wasGroundedLastFrame;// kiểm tra xem có chạm đất trong frame trước hay không
    [Header("Thành phần -------------------------------")]
    public Transform cameraTransform;
    public CharacterController controller;
    public Animator animator;
    public AudioSource audioSource;
    public RigBuilder rigBuilder;
    [Header("Tutorial -------------------------------")]
    public bool isRun;
    public bool isJump;
    public bool isRollBack;
    public bool isAttack;
   

    [Header("-------------------------------")]
    //bien kiem tra 
    public bool isRunning = false;
    public bool isController = true;
    
    //cooldown roll
    public float rollColdownTime = -2f;
   
    //goi ham tham chieu
    public PlayerStatus playerStatus;
    public ComboAttack comboAttack;
    public CameraFollow thirdPersonOrbitCamera;
    public EvenAnimator evenAnimator;
  
    //skill2 tham chieu
    public bool isRemoveClone = false;
    public bool isSkinSkill3Clone = false; //kiểm tra xem có sử dụng skin skill3 clone hay không
    public Skill2Manager skill2Manager;   
    public Skill3Manager skill3Manager; //skill3 tham chieu
    public Skill1Manager skill1Manager; //skill1 tham chieu
    public Skill4Manager skill4Manager;// skill 4 tham chieu
     
    // Biến để lưu vị trí checkpoint
    public Vector3 checkpointPosition;

    //trang thai animator
    public RuntimeAnimatorController animatorDefauld;//trang thai mac định
    public RuntimeAnimatorController animatorSkill2;//trang thai skill2
    public RuntimeAnimatorController animatorSkill4;//trang thai skill4
    // Trạng thái hiện tại                                              
    private PlayerState currentState;
    //weapon L
    public GameObject weaponSword;
    //Die
    public GameObject canvasLoad;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorDefauld; // Gán bộ điều khiển hoạt hình mặc định
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        skill4Manager = FindAnyObjectByType<Skill4Manager>();
       
        thirdPersonOrbitCamera = FindAnyObjectByType<CameraFollow>();
        evenAnimator = FindAnyObjectByType<EvenAnimator>();
        weaponSword.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        canvasLoad.SetActive(false);
        rigBuilder = GetComponent<RigBuilder>();
        rigBuilder.enabled = false;
        //vị trí checkpoint
        //if (PlayerPrefs.HasKey("CheckpointX"))
        //{
        //    Debug.Log("luu vi tri khi spawnpoint trc");
        //   float X = PlayerPrefs.GetFloat("CheckpointX");
        //    float Y = PlayerPrefs.GetFloat("CheckpointY");
        //   float Z = PlayerPrefs.GetFloat("CheckpointZ");
        //    checkpointPosition = new Vector3(X, Y, Z);
        //   transform.position = checkpointPosition; // Đặt vị trí của người chơi về vị trí checkpoint đã lưu
        //}
        //else
        //{
        //    Debug.Log("lay vi tri cu");
        //    checkpointPosition = transform.position; // Nếu không có checkpoint, sử dụng vị trí hiện tại
        //}

        // Gọi hàm ChangeState để chuyển sang trạng thái ban đầu
        ChangeState(new PlayerCurrentState(this));
    }

    void Update()
    {
        // Gọi hàm Updat của trạng thái hiện tại 
        currentState?.Update();
      
    }

    // Hàm chuyển trạng thái
    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();     // Thoát trạng thái cũ 
        currentState = newState;  // Gán trạng thái mới
        currentState.Enter();     // Kích hoạt trạng thái mới
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    // Gọi hàm này khi chạm checkpoint
    public void SetCheckpoint(Vector3 position)
    {
        Debug.Log("da luu vi tri " + position);
        checkpointPosition = position;
        PlayerPrefs.SetFloat("CheckpointX", position.x);
        PlayerPrefs.SetFloat("CheckpointY", position.y);
        PlayerPrefs.SetFloat("CheckpointZ", position.z);
        PlayerPrefs.Save();

    }

    //khi pl bi an thi chay
    void OnDisable()
    {  
        //skill4
        animator.runtimeAnimatorController = animatorDefauld; // Trở về animator mặc định
        animator.SetTrigger("Skill4State");
        skill4Manager.isHibitedIcon = false; // cam skill icon
        isSkinSkill3Clone = false;
        //skill2
        skill2Manager.isHibitedIcon = false; // Bỏ cấm sử dụng skill 2
        ChangeState(new PlayerCurrentState(this)); // Trở về trạng thái hiện tại
     
        
    }
   
    //tinh toan tim enemy gan nhat
    public Transform GetNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 10, enemyLayer);

        float minDist = Mathf.Infinity;
        Transform closest = null;

        foreach (Collider col in hits)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = col.transform;
            }
        }

        return closest;
    }

}
