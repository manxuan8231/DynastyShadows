using UnityEngine;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;       // Panel Teleport
    public GameObject buttonF;       // UI nhấn F
    public GameObject saveButton; // Nút “Lưu vị trí”

    public string teleportID = "teleport_1"; // ID riêng cho mỗi teleport

    private bool isInRange = false;
    public Collider colliderOpen;
    public AudioSource audioSource;
    public AudioClip soundF;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        saveButton.SetActive(false);

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
            saveButton.SetActive(true); // hiện nút “Lưu vị trí”
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

    public void SaveTeleportPoint()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Vector3 pos = player.position;

        // Lưu vị trí
        PlayerPrefs.SetFloat("saved_x", pos.x);
        PlayerPrefs.SetFloat("saved_y", pos.y);
        PlayerPrefs.SetFloat("saved_z", pos.z);
        PlayerPrefs.SetString("current_teleport", teleportID);

        // Tắt các teleport khác
        foreach (var tele in FindObjectsOfType<OpenTele>())
        {
            if (tele != this)
            {
                tele.imgTele.SetActive(false);
                tele.saveButton.SetActive(false);
                tele.colliderOpen.enabled = true;
                PlayerPrefs.SetInt(tele.teleportID, 0);
            }
        }

        PlayerPrefs.SetInt(teleportID, 1);
        PlayerPrefs.Save();

        Debug.Log("Đã lưu vị trí tại teleport: " + teleportID);
    }


}
