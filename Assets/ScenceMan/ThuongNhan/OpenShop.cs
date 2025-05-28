using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopPanel; // Panel hiển thị cửa hàng
    public GameObject buttonF; // Nút F để mở cửa hàng

    private bool isButtonF;
    
    void Start()
    {
        shopPanel.SetActive(false); // Ẩn cửa hàng khi bắt đầu
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF)
        {
            // Đảo trạng thái cửa hàng
            bool isShopActive = !shopPanel.activeSelf;
            shopPanel.SetActive(isShopActive);

            // Dừng hoặc tiếp tục thời gian tùy theo trạng thái cửa hàng
            Time.timeScale = isShopActive ? 0f : 1f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isButtonF = true; // Đặt biến isButtonF thành true khi người chơi vào vùng kích hoạt
            buttonF.SetActive(true); // Hiển thị nút F khi người chơi vào vùng kích hoạt
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isButtonF = false; // Đặt biến isButtonF thành false khi người chơi rời khỏi vùng kích hoạt
            buttonF.SetActive(false); // Ẩn nút F khi người chơi rời khỏi vùng kích hoạt
        }
    }
}
