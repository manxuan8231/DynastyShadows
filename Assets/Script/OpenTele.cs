using UnityEngine;
using UnityEngine.Audio;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;         // Panel Teleport
    public GameObject buttonF;      // UI nhấn F

    private bool isInRange = false;
    public Collider colliderOpen; // Collider để kiểm tra va chạm

    public AudioSource audioSource;
    // Âm thanh khi nhấn nút F
    public AudioClip soundF; // Âm thanh khi nhấn nút F
    void Start()
    {
        imgTele.SetActive(false);
        buttonF.SetActive(false);
        colliderOpen.enabled = true; // Bật collider để nhận va chạm
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            isInRange = false; // Đặt lại trạng thái để không thể nhấn F liên tục
            audioSource.PlayOneShot(soundF);
            buttonF.SetActive(false);
            colliderOpen.enabled = false;
            imgTele.SetActive(true);

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(true);
            isInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(false);
            isInRange = false;
        }
    }
}
