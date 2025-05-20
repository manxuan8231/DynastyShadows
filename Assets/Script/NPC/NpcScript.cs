using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại

    public string[] names; // Danh sách tên (ai đang nói)
    public string[] content; // Nội dung hội thoại

    private Coroutine coroutine; //tieep tục hội thoại

    public GameObject buttonF; // Nút F để tương tác với NPC
    private bool isContent = true;
    private bool isButtonF = false; // Kiểm tra trạng thái của nút F
    //tham chieu
    PlayerController playerController; // Tham chiếu đến PlayerController
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    void Start()
    {
        // Lấy tham chiếu đến PlayerController và ComboAttack
        playerController = FindAnyObjectByType<PlayerController>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        // Ẩn panel và nút F khi bắt đầu
        NPCPanel.SetActive(false);
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        NPCName.text = "";
        NPCContent.text = "";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF)
        {   
            comboAttack.enabled = false; // Vô hiệu hóa ComboAttack
            playerController.isController = false; // Vô hiệu hóa PlayerController
            playerController.animator.SetBool("isWalking", false); // Dừng hoạt động của nhân vật
            playerController.animator.SetBool("isRunning", false); // Dừng hoạt động của nhân vật
            //
            NPCPanel.SetActive(true);
            coroutine = StartCoroutine(ReadContent());
            buttonF.SetActive(false); // Ẩn nút F khi bắt đầu hội thoại
            isButtonF = false; // Đặt trạng thái hội thoại là false
            isContent = false; // Đặt lại trạng thái hội thoại
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isContent)
        {
            buttonF.SetActive(true); // Hiện nút F khi vào vùng tương tác
            isButtonF = true; // Đặt trạng thái hội thoại là true
        }
    }
                                    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(false); // Ẩn nút F khi ra khỏi vùng tương tác
           
            isButtonF = false; // Đặt trạng thái hội thoại là false
            NPCPanel.SetActive(false);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private IEnumerator ReadContent()
    {
        // Lặp qua từng câu thoại và tên
        for (int i = 0; i < content.Length; i++)
        {
            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "Unknown"; // Hiển thị tên hoặc "Unknown" nếu không có tên

            foreach (var item in content[i])
            {
                NPCContent.text += item;
                yield return new WaitForSeconds(0.05f); // Tốc độ chạy chữ
            }
            yield return new WaitForSeconds(1f); // Thời gian ngừng giữa các câu
        }

        // Ẩn panel sau khi kết thúc hội thoại
        NPCPanel.SetActive(false);
        //
        playerController.isController = true; // Bật lại PlayerController
        comboAttack.enabled = true; // Bật lại ComboAttack
    }

    public void EndContent()// Kết thúc hội thoại
    {
        NPCPanel.SetActive(false);
        
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
