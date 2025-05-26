using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopPanel; // Panel hiển thị cửa hàng
    public GameObject buttonF; // Nút F để mở cửa hàng
    
    void Start()
    {
        shopPanel.SetActive(false); // Ẩn cửa hàng khi bắt đầu
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
    }

   
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        { 
            shopPanel.SetActive(!shopPanel.activeSelf); // Mở hoặc đóng cửa hàng khi nhấn F
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            buttonF.SetActive(true); // Hiển thị nút F khi người chơi vào vùng kích hoạt
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(false); // Ẩn nút F khi người chơi rời khỏi vùng kích hoạt
        }
    }
}
