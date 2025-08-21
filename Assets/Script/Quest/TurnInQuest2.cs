using System.Collections;
using TMPro;
using UnityEngine;

public class TurnInQuest2 : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại
    public GameObject iconMap; // Icon hiển thị trên bản đồ
    public GameObject player;
    public GameObject cam;
    public GameObject danLang;


    public GameObject linhCanh;
    public GameObject thuongNhan;
    public GameObject niceQuestUI;
    public string[] names; // Danh sách tên 
    public string[] content; // Nội dung hội thoại
    //
    private Coroutine coroutine; //tieep tục hội thoại
    public GameObject buttonF; // Nút F để tương tác với NPC
    public bool isContent = false;
    public bool isButtonF = false; // Kiểm tra trạng thái của nút F
    //nut skip
    public GameObject buttonSkip; // Nút Skip
    public GameObject buttonSkipAll; // Nút Skip All

    private bool isTyping = false; // Đang chạy từng chữ
    private bool skipPressed = false; // Người chơi đã bấm skip
    private bool isWaitingForNext = false; // Đang chờ người chơi bấm Skip để qua câu tiếp theo

    //tham chieu
    PlayerControllerState playerController; // Tham chiếu đến PlayerController
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    Quest2 quest2; // Tham chiếu đến QuestManager
    PlayerStatus playerStatus; // Tham chiếu đến PlayerStatus
    KnightD knightD; // Tham chiếu đến KnightD

    public AudioSource audioSource; // Tham chiếu đến AudioSource
    public AudioClip audioSkip; // Âm thanh khi bấm skip

   

    void Start()
    {
        linhCanh.SetActive(false); // Ẩn linh canh khi bắt đầu
        thuongNhan.SetActive(false);
        // Lấy tham chiếu đến PlayerController và ComboAttack
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        quest2 = FindAnyObjectByType<Quest2>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        audioSource = GetComponent<AudioSource>();
        knightD = GetComponent<KnightD>(); // Lấy tham chiếu đến KnightD
        
        // Ẩn panel và nút F khi bắt đầu

        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonF.SetActive(false); // Ẩn nút F khi bắt đầu
        isButtonF = false; // Đặt trạng thái hội thoại là false
        niceQuestUI.SetActive(false); // Ẩn UI nhiệm vụ đẹp khi bắt đầu
        NPCName.text = "";
        NPCContent.text = "";

        player = FindAnyObjectByType<PlayerControllerState>().gameObject;

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
            player.SetActive(false); // Ẩn người chơi khi bắt đầu hội thoại
            cam.SetActive(true); // Đặt camera ưu tiên cao hơn để theo dõi NPC
            knightD. animator.SetBool("Talking", true); // Bật trạng thái Talking của animator
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

        // Kết thúc + nhiem vu
       
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

        if (player != null)
        {
            player.SetActive(true);
            cam.SetActive(false);
            knightD.animator.SetBool("Talking", false);
        }

        // phần thưởng
        quest2.questPanel.SetActive(false);
        quest2.iconQuest.SetActive(false);
        iconMap.SetActive(false);
        isButtonF = false;
        isContent = false;
        linhCanh.SetActive(true);
        thuongNhan.SetActive(true);
        playerStatus.showSkill2 = true;
        playerStatus.IncreasedGold(200);
        StartCoroutine(WaitQuestUI());

        Debug.Log("Phần thưởng đã nhận");

        // save
        GameSaveData data = SaveManagerMan.LoadGame();
        data.skillTreeData.showSkill2 = playerStatus.showSkill2;
        data.dataQuest.isQuest2Map1 = true;
        SaveManagerMan.SaveGame(data);

        Destroy(danLang, 1f);
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

    public void OnSkipAllButtonPressed()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        audioSource?.PlayOneShot(audioSkip);

        EndDialogueAndQuest();
    }

}
