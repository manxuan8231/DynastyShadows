using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopPanel; // Panel hiển thị cửa hàng
    public GameObject buttonF; // Nút F để mở cửa hàng

    public bool isButtonF;
    public bool isShopActive = false; // Biến để kiểm tra trạng thái của cửa hàng
    void Start()
    {
        shopPanel.SetActive(false); // Ẩn cửa hàng khi bắt đầu
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        isShopActive = false; // Đặt biến isButtonF ban đầu là false
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF == true)
        {
            // Đảo trạng thái cửa hàng
            bool isShopActive = !shopPanel.activeSelf;
            shopPanel.SetActive(isShopActive);

            // Dừng hoặc tiếp tục thời gian tùy theo trạng thái cửa hàng
            Time.timeScale = isShopActive ? 0f : 1f;

            Cursor.visible = isShopActive; // Hiện hoặc ẩn con trỏ chuột
            Cursor.lockState = isShopActive ? CursorLockMode.None : CursorLockMode.Locked; // Mở hoặc khóa con trỏ chuột
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isShopActive == true)
        {
            isButtonF = true; // Đặt biến isButtonF thành true khi người chơi vào vùng kích hoạt
            buttonF.SetActive(true); // Hiển thị nút F khi người chơi vào vùng kích hoạt
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isShopActive == true)
        {
            isButtonF = false; // Đặt biến isButtonF thành false khi người chơi rời khỏi vùng kích hoạt
            buttonF.SetActive(false); // Ẩn nút F khi người chơi rời khỏi vùng kích hoạt
        }
    }
}
