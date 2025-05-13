using UnityEngine;

public class OpenTele : MonoBehaviour
{
    public GameObject imgTele;         // Panel Teleport
    public GameObject buttonF;      // UI nhấn F (ví dụ Text "Nhấn F để mở")

    private bool isInRange = false;
    public Collider colliderOpen; // Collider để kiểm tra va chạm
    void Start()
    {
        imgTele.SetActive(false);
        buttonF.SetActive(false);
        colliderOpen.enabled = true; // Bật collider để nhận va chạm
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            buttonF.SetActive(false);
            colliderOpen.enabled = false;
            imgTele.SetActive(false);
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
