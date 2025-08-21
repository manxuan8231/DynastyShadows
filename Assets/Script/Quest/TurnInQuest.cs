using System.Collections;
using TMPro;
using UnityEngine;

public class TurnInQuest : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại
    public GameObject iconMap; // Icon hiển thị trên bản đồ
    public GameObject niceQuestUI;
    public GameObject questThuongNhan;//làm xong nhiệm vụ của bác lâm thì mới suất hiện thuognw nhân
    public GameObject questPointer; // Chỉ dẫn tới nhận nhiệm vụ đánh cá
    public GameObject quest1BacLam;//an sau khi hoan thanh

    //trang thai
    public enum QuestToStart { None, BacLam, LinhCanh }
    public QuestToStart questToStart = QuestToStart.None;
    //
    public string[] names; // Danh sách tên 
    public string[] content; // Nội dung hội thoại
    //
    private Coroutine coroutine; //tieep tục hội thoại
    public GameObject buttonF; // Nút F để tương tác với NPC
    public bool isContent = false;
    public bool isButtonF = false; // Kiểm tra trạng thái của nút F
    //nut skip
    public GameObject buttonSkip; // Nút Skip
    private bool isTyping = false; // Đang chạy từng chữ
    private bool skipPressed = false; // Người chơi đã bấm skip
    private bool isWaitingForNext = false; // Đang chờ người chơi bấm Skip để qua câu tiếp theo
    public GameObject buttonSkipAll; // Nút Skip All

    //tham chieu
    PlayerControllerState playerController; // Tham chiếu đến PlayerController
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    Quest1 quest1; // Tham chiếu đến QuestManager
   
    PlayerStatus playerStatus; // Tham chiếu đến PlayerStatus
    NPCScript npcScript; // Tham chiếu đến NPCScript
    AudioSource audioSource; // Tham chiếu đến AudioSource
    public AudioClip audioSkip; // Âm thanh khi bấm skip
    void Start()
    {
        // Lấy tham chiếu đến PlayerController và ComboAttack
        playerStatus = FindAnyObjectByType<PlayerStatus>(); // Lấy tham chiếu đến PlayerStatus
        quest1 = FindAnyObjectByType<Quest1>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
      
        if (npcScript == null)
        {
            npcScript = GetComponent<NPCScript>(); // Lấy tham chiếu đến NPCScript
        }
        audioSource = GetComponent<AudioSource>();
        // Ẩn panel và nút F khi bắt đầu
        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        questThuongNhan.SetActive(false); // Ẩn NPC ThuongNhan khi bắt đầu
        niceQuestUI.SetActive(false); // Ẩn UI nhiệm vụ đẹp khi bắt đầu
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
            isButtonF = false; // Đặt trạng thái hội thoại là false
            isContent = false; // Đặt lại trạng thái hội thoại
            npcScript.player.SetActive(false); // Ẩn nhân vật người chơi khi hội thoại bắt đầu
            npcScript.cam.SetActive(true); // Đặt priority của camera NPC cao hơn camera người chơi
            buttonSkipAll.SetActive(true);
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

        EndDialogueAndQuest();

    }


    public void OnSkipButtonPressed()
    {
        audioSource.PlayOneShot(audioSkip); // Phát âm thanh khi bấm skip
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
    public void OnSkipAllButtonPressed()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine); // Dừng toàn bộ đọc hội thoại
            coroutine = null;
        }

        audioSource?.PlayOneShot(audioSkip); // Âm thanh skip (nếu có)

        // Gọi thẳng đến kết thúc hội thoại và nhiệm vụ
        EndDialogueAndQuest();
    }
    private void EndDialogueAndQuest()
    {
        buttonSkip.SetActive(false);
        buttonSkipAll.SetActive(false);
        NPCPanel.SetActive(false);

        playerController.isController = true;
        comboAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (npcScript.player != null)
        {
            npcScript.player.SetActive(true);
            npcScript.cam.SetActive(false);
        }

        switch (questToStart)
        {
            case QuestToStart.BacLam:
                questPointer.SetActive(true);
                quest1.questPanel.SetActive(false);
                quest1.iconQuest.SetActive(false);
                iconMap.SetActive(false);
                playerStatus.IncreasedGold(100);
                questThuongNhan.SetActive(true);
                StartCoroutine(WaitQuestUI());
                quest1.questPointer2.SetActive(false);
                playerStatus.showSkill1 = true;
                Destroy(quest1BacLam, 3f);

                Debug.Log("Phần thưởng đã nhận");
                // Save game
                GameSaveData data = SaveManagerMan.LoadGame();
                data.dataQuest.isQuest1Map1 = true;
                data.skillTreeData.showSkill1 = playerStatus.showSkill1;
                SaveManagerMan.SaveGame(data);
                break;

            case QuestToStart.LinhCanh:
                // TODO: thêm logic
                break;
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

    private IEnumerator WaitQuestUI()
    {
        niceQuestUI.SetActive(true); // Hiện UI nhiệm vụ đẹp
        yield return new WaitForSeconds(5f);
        niceQuestUI.SetActive(false); // Ẩn UI nhiệm vụ đẹp sau 2 giây
    }
}
