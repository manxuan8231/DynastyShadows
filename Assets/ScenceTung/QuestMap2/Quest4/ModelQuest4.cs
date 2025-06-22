using TMPro;
using UnityEngine;

public class ModelQuest4 : MonoBehaviour
{
    [Header("Tham chiếu------------")]
    public MissionPlay MissionPlay;

    [Header("CanvasContent")]
    public GameObject canvasContent;
    public GameObject btnF;
    public TMP_Text btnF_Text;

    [Header("Bool")]
    public bool isTake = false;

    void Start()
    {
        MissionPlay = FindAnyObjectByType<MissionPlay>(); // Tìm đối tượng MissionPlay trong scene
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            canvasContent.SetActive(true);
            btnF.SetActive(true); // Hiển thị nút F khi người chơi chạm vào mô hình
            btnF_Text.text = "F: Nhặt"; // Cập nhật văn bản nút F
            if (Input.GetKeyDown(KeyCode.F) && !isTake)
            {
                isTake = true; // Đặt cờ isTake thành true khi nhấn nút F
                MissionPlay.UpCount();
                canvasContent.SetActive(false); // Ẩn canvas khi nhấn nút F
                btnF.SetActive(false); // Ẩn nút F khi nhấn nút F
                Destroy(gameObject); // Xóa mô hình khỏi scene
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvasContent.SetActive(false); // Ẩn canvas khi người chơi rời khỏi mô hình
            btnF.SetActive(false); // Ẩn nút F khi người chơi rời khỏi mô hình
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
