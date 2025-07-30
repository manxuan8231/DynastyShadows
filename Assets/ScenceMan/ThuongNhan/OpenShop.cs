using TMPro;
using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public GameObject shopPanel; // Panel hiển thị cửa hàng
    public GameObject buttonF; // Nút F để mở cửa hàng
    public TextMeshProUGUI textGold;
    public float gold;
    public bool isButtonF;

    //tham chieu
    TurnInQuestThuongNhan turnInQuestThuongNhan; // Tham chiếu đến TurnInQuestThuongNhan

    void Start()
    {
        shopPanel.SetActive(false); // Ẩn cửa hàng khi bắt đầu
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        turnInQuestThuongNhan = FindAnyObjectByType<TurnInQuestThuongNhan>(); // Lấy tham chiếu đến TurnInQuestThuongNhan
        textGold.text = $"{gold}";
    }


    void Update()
    {
        gold = TurnOffOnUI.gold;
        textGold.text = $"{gold}";
        if (TurnOffOnUI.pause) return;//khi ui khac dg bat thi ko cho mo shop
        if (Input.GetKeyDown(KeyCode.F) && isButtonF == true)
        {
            // Đảo trạng thái cửa hàng
            bool isShopActive = !shopPanel.activeSelf;
            shopPanel.SetActive(isShopActive);

            buttonF.SetActive(isShopActive);
            // Dừng hoặc tiếp tục thời gian tùy theo trạng thái cửa hàng
            Time.timeScale = isShopActive ? 0f : 1f;

            Cursor.visible = isShopActive; // Hiện hoặc ẩn con trỏ chuột
            Cursor.lockState = isShopActive ? CursorLockMode.None : CursorLockMode.Locked; // Mở hoặc khóa con trỏ chuột
        }
        else if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.T) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopPanel.activeSelf) // Nếu cửa hàng đang mở
            {
                shopPanel.SetActive(false); // Ẩn cửa hàng
                buttonF.SetActive(true );
            }
        }
       
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && turnInQuestThuongNhan.isOpenShop == true)  
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
