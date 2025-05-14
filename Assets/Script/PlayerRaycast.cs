using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    public float rayDistance = 1f;                // Độ dài ray
    public LayerMask healthLayer;                 // Chỉ raycast vào Layer "Health"
    //goi ham
    private Animator animator;

    AudioSource audioSource;
    public AudioClip pickUpSound;

    public GameObject buttonF;

    //ke thua
    private LevelAvatar itemHealth;
    private PlayerStatus playerStatus;
    private ComboAttack comboAttack;
    private PlayerController playerController;
    void Start()
    {
        comboAttack = FindAnyObjectByType<ComboAttack>();
        itemHealth = FindAnyObjectByType<LevelAvatar>();
        playerController = FindAnyObjectByType<PlayerController>();
        playerStatus = FindAnyObjectByType<PlayerStatus>();

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        buttonF.SetActive(false);
    }

    void Update()
    {
        Vector3 origin = transform.position + transform.forward * 0.1f; // Đặt ray phía trước player
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, rayDistance, healthLayer))
        {
            buttonF.SetActive(true);
            if (Input.GetKey(KeyCode.F))
            {
                buttonF.SetActive(false);
                
                //dg pick up thi ko cho di chuyen vs attack
                playerController.isController = false;
                comboAttack.isAttack = false;
                //
                Debug.Log("Hit Health Object: " + hit.collider.name);
                Debug.DrawRay(origin, direction * rayDistance, Color.green);
                if (itemHealth != null)
                {
                    animator.SetTrigger("PickUp");
                    float randomHealth = Random.Range(500, 2000);
                    int randomValueAT = Random.Range(1, 5);
                    playerStatus.AddHealth(randomHealth);
                    itemHealth.AddValueAvatar(randomValueAT);
                    //destroy vat pham
                    Destroy(hit.collider.gameObject);
                }
            }                     
        }
        else
        {
            
            buttonF.SetActive(false);
            Debug.DrawRay(origin, direction * rayDistance, Color.red);
        }
    }
    public void EndPickUp()
    {
        playerController.isController = true;
        comboAttack.isAttack = true;
    }
    public void PlaySound()
    {
        audioSource.PlayOneShot(pickUpSound);
    }
}
