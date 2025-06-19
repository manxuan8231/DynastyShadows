using UnityEngine;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;       // Panel Teleport
    public GameObject buttonF;       // UI nhấn F
  

    public string teleportID = "teleport_1"; // ID riêng cho mỗi teleport

    private bool isInRange = false;
    public Collider colliderOpen;
    public AudioSource audioSource;
    public AudioClip soundF;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
       
        // Kiểm tra trạng thái đã lưu
        if (PlayerPrefs.GetInt(teleportID, 0) == 1)
        {
            imgTele.SetActive(true);
            buttonF.SetActive(false);
            colliderOpen.enabled = false;
        }
        else
        {
            imgTele.SetActive(false);
            buttonF.SetActive(false);
            colliderOpen.enabled = true;
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
            Debug.Log("Đã save mở teleport");
            PlayerPrefs.SetInt(teleportID, 1); // Lưu trạng thái đã mở teleport
            PlayerPrefs.Save(); // Lưu thay đổi vào PlayerPrefs

        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Xóa dữ liệu lưu trữ");
            PlayerPrefs.DeleteAll(); // Xóa tất cả dữ liệu lưu trữ
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
