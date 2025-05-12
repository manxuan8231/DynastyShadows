using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    public float rayDistance = 1f;                // Độ dài ray
    public LayerMask healthLayer;                 // Chỉ raycast vào Layer "Health"
    //goi ham
    private Animator animator;
    private PlayerController playerController;
    private AvatarHealth itemHealth;
    private ComboAttack comboAttack;

    AudioSource audioSource;
    public AudioClip pickUpSound;

    public GameObject buttonF;
   
    void Start()
    {
        comboAttack = FindAnyObjectByType<ComboAttack>();
        itemHealth = FindAnyObjectByType<AvatarHealth>();
        playerController = FindAnyObjectByType<PlayerController>();
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
            if (Input.GetKeyDown(KeyCode.F))
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

                    itemHealth.AddValueAvatar(1);
                    Destroy(hit.collider.gameObject, 1f);
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
