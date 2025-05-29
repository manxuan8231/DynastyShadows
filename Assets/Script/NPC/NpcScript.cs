using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại
    public enum QuestToStart { None, BacLam, Village, LinhCanh, MainBacLam ,LinhCanhB}
    public QuestToStart questToStart = QuestToStart.None;


    public string[] names; // Danh sách tên (ai đang nói)
    public string[] content; // Nội dung hội thoại
    private Coroutine coroutine; //tieep tục hội thoại

    public GameObject buttonF; // Nút F để tương tác với NPC
    private bool isContent = true;
    private bool isButtonF = false; // Kiểm tra trạng thái của nút F
    //nut skip
    public GameObject buttonSkip; // Nút Skip
    private bool isTyping = false; // Đang chạy từng chữ
    private bool skipPressed = false; // Người chơi đã bấm skip
    private bool isWaitingForNext = false; // Đang chờ người chơi bấm Skip để qua câu tiếp theo

    //tham chieu
    PlayerController playerController; // Tham chiếu đến PlayerController
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    Quest1 quest1; // Tham chiếu đến QuestManager
    Quest2 quest2; // Tham chiếu đến QuestManager
    Quest3 quest3;
    QuestMainBacLam questMainBacLam; // Tham chiếu đến QuestManager

    AudioSource audioSource; // Tham chiếu đến AudioSource
    public AudioClip audioSkip; // Âm thanh khi bấm skip
    void Start()
    {
        // Lấy tham chiếu đến PlayerController và ComboAttack
        quest1 = FindAnyObjectByType<Quest1>();
        quest2 = FindAnyObjectByType<Quest2>();

        playerController = FindAnyObjectByType<PlayerController>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        audioSource = GetComponent<AudioSource>();
        quest3 = FindAnyObjectByType<Quest3>();
        questMainBacLam = FindAnyObjectByType<QuestMainBacLam>();
        // Ẩn panel và nút F khi bắt đầu
        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        NPCName.text = "";
        NPCContent.text = "";
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF)
        {   
            Cursor.lockState = CursorLockMode.None; // mở chuột
            Cursor.visible = true; // hiện chuột
            comboAttack.enabled = false; // Vô hiệu hóa ComboAttack
            playerController.isController = false; // Vô hiệu hóa PlayerController
            playerController.animator.SetBool("isWalking", false); // Dừng hoạt động của nhân vật
            playerController.animator.SetBool("isRunning", false); // Dừng hoạt động của nhân vật
            //
            NPCPanel.SetActive(true);
            coroutine = StartCoroutine(ReadContent());
            buttonF.SetActive(false); // Ẩn nút F khi bắt đầu hội thoại

           
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Bấm Skip trong lúc chữ đang chạy → hiện toàn bộ câu
                skipPressed = true;
            }
            else if (isWaitingForNext)
            {
                // Bấm Skip lần 2 → chuyển sang câu tiếp theo
                skipPressed = true;
            }
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
            Cursor.lockState = CursorLockMode.Locked; // mở chuột
            Cursor.visible = false; // hiện chuột
            comboAttack.enabled = true; //  ComboAttack
            playerController.isController = true; //  PlayerController
            playerController.animator.SetBool("isWalking", true); //  hoạt động của nhân vật
            playerController.animator.SetBool("isRunning", true); //  hoạt động của nhân vật
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private IEnumerator ReadContent()
    {
        buttonSkip.SetActive(true); // Hiện nút Skip

        for (int i = 0; i < content.Length; i++)
        {
            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "Unknown";

            isTyping = true;
            skipPressed = false;
            isWaitingForNext = false;

            foreach (var letter in content[i])
            {
                if (skipPressed)
                {
                    NPCContent.text = content[i]; // Hiện toàn bộ nội dung
                    break;
                }

                NPCContent.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            skipPressed = false;
            isWaitingForNext = true;

            // Đợi người chơi bấm Skip để qua câu tiếp theo
            while (!skipPressed)
            {
                yield return null;
            }

            isWaitingForNext = false;
        }

        // Kết thúc + nhiem vu
        buttonSkip.SetActive(false);
        NPCPanel.SetActive(false);
        playerController.isController = true;
        comboAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isButtonF = false; // Đặt trạng thái hội thoại là false
        isContent = false; // Đặt lại trạng thái hội thoại
        //nhiệm vụ yêu cầu
        switch (questToStart)
        {
            case QuestToStart.BacLam:
                quest1.StartQuestBacLam();
                break;
            case QuestToStart.Village:
                quest2.StartQuestVillage();
                break;
            case QuestToStart.LinhCanh:
                quest3.StartQuestLinhCanh();
                break;
            case QuestToStart.MainBacLam:
                questMainBacLam.StartQuestMainBacLam();
                break;
            case QuestToStart.LinhCanhB:
                questMainBacLam.StartQuestLinhCanhB();
                break;
        }

    }


    public void OnSkipButtonPressed()
    {
       
        if (isTyping)
        {
            // Bấm Skip trong lúc chữ đang chạy → hiện toàn bộ câu
            skipPressed = true;
        }
        else if (isWaitingForNext)
        {
            // Bấm Skip lần 2 → chuyển sang câu tiếp theo
            skipPressed = true;
        }
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
