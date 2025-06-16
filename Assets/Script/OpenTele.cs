using UnityEngine;
using UnityEngine.Audio;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;         // Panel Teleport
    public GameObject buttonF;      // UI nhấn F
    public string teleportID = "teleport_1"; // Đặt ID khác nhau nếu có nhiều cái


    private bool isInRange = false;
    public Collider colliderOpen; // Collider để kiểm tra va chạm

    public AudioSource audioSource;
    public AudioClip soundF; // Âm thanh khi nhấn nút F
    void Start()
    {
      
        imgTele.SetActive(false);
        buttonF.SetActive(false);
        colliderOpen.enabled = true; // Bật collider để nhận va chạm
        audioSource = GetComponent<AudioSource>();
        // Kiểm tra nếu đã lưu trạng thái mở
        if (PlayerPrefs.GetInt(teleportID, 0) == 1)
        {
            imgTele.SetActive(true);
            colliderOpen.enabled = false;
        }
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            isInRange = false;
            audioSource.PlayOneShot(soundF);
            buttonF.SetActive(false);
            colliderOpen.enabled = false;
            imgTele.SetActive(true);

            // Lưu trạng thái đã mở teleport
            PlayerPrefs.SetInt(teleportID, 1);
            PlayerPrefs.Save();
        }
        // Bấm R để reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey(teleportID);
            PlayerPrefs.Save();
            Debug.Log("Đã xóa dữ liệu teleport.");
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
