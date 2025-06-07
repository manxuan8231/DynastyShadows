using System;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class PlayerControllerState : MonoBehaviour
{
    [Header("Tham số ------------------------------")]
    public float walkSpeed = 5f;
    public float runSpeed;
    public float jumpHeight = 5f;
    public float gravity = -9.81f;

    [Header("Kiểm tra mặt đất -----------------------")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Thành phần -------------------------------")]
    public Transform cameraTransform;
    public CharacterController controller;
    public Animator animator;
    public AudioSource audioSource;
    public RigBuilder rigBuilder;

    //cham dat
    public Vector3 velocity;
    public bool isGrounded;
    public bool wasGroundedLastFrame;
    public bool isRunning = false;
    public bool isController = true;
    
    //cooldown roll
    public float rollColdownTime = -2f;
    //audio
    public AudioClip audioJump;
    public AudioClip audioRoll;
    public AudioClip audioMovemen;

    //goi ham tham chieu
    public PlayerStatus playerStatus;
    public ComboAttack comboAttack;

    //skill2 tham chieu
    public Skill2Manager skill2Manager;
    public bool isRemoveClone = false;
    public Skill3Manager skill3Manager; //skill3 tham chieu
    public Skill1Manager skill1Manager; //skill1 tham chieu
    public Skill4Manager skill4Manager;// skill 4 tham chieu

    // Trạng thái hiện tại
    public RuntimeAnimatorController animatorDefauld;//trang thai mac định
    public RuntimeAnimatorController animatorSkill2;//trang thai skill2
    public RuntimeAnimatorController animatorSkill4;//trang thai skill4
    private PlayerState currentState;

    public GameObject weaponSword; //weapon
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorDefauld; // Gán bộ điều khiển hoạt hình mặc định
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        skill1Manager = FindAnyObjectByType<Skill1Manager>();
        skill2Manager = FindAnyObjectByType<Skill2Manager>();
        skill3Manager = FindAnyObjectByType<Skill3Manager>();
        skill4Manager = FindAnyObjectByType<Skill4Manager>();

        weaponSword.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        runSpeed = playerStatus.speedRun;// tốc độ chạy
        rigBuilder = GetComponent<RigBuilder>();
        rigBuilder.enabled = false; // 
        
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
    //sound even 
    public void SoundMovemen()
    {
        audioSource.PlayOneShot(audioMovemen);
    }
}
