using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class TurnInQuestThuongNhan : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại
    public GameObject niceQuestUI;
    public GameObject playerNpc;
    public GameObject player; // Tham chiếu đến đối tượng người chơi
    public GameObject cam;
    public CinemachineCamera camcine;
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
    public GameObject buttonSkipAll; // ✅ Nút Skip All
    private bool isTyping = false; // Đang chạy từng chữ
    private bool skipPressed = false; // Người chơi đã bấm skip
    private bool isWaitingForNext = false; // Đang chờ người chơi bấm Skip để qua câu tiếp theo
    private bool skipAll = false; // ✅ Người chơi bấm Skip All

    public bool isOpenShop = false; // Kiểm tra trạng thái của OpenShop
    //tham chieu
    PlayerControllerState playerController; // Tham chiếu đến PlayerController
    ComboAttack comboAttack; // Tham chiếu đến ComboAttack
    PlayerStatus playerStatus;
    OpenShop openShop;

    AudioSource audioSource; // Tham chiếu đến AudioSource
    public AudioClip audioSkip; // Âm thanh khi bấm skip
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        audioSource = GetComponent<AudioSource>();
        openShop = FindAnyObjectByType<OpenShop>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Ẩn panel và nút F khi bắt đầu
        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonSkipAll.SetActive(false); // ✅ Ẩn Skip All khi bắt đầu
        buttonF.SetActive(false);
        niceQuestUI.SetActive(false);
        NPCName.text = "";
        NPCContent.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            comboAttack.enabled = false;
            playerController.isController = false;
            playerController.animator.SetBool("isWalking", false);
            playerController.animator.SetBool("isRunning", false);

            NPCPanel.SetActive(true);
            coroutine = StartCoroutine(ReadContent());
            buttonF.SetActive(false);
            isButtonF = false;
            isContent = false;

            playerNpc.SetActive(true);
            player.SetActive(false);
            cam.SetActive(true);
            camcine.enabled = true;
            camcine.Priority = 50;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isContent)
        {
            buttonF.SetActive(true);
            isButtonF = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            buttonF.SetActive(false);
            isButtonF = false;
            NPCPanel.SetActive(false);
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    private IEnumerator ReadContent()
    {
        buttonSkip.SetActive(true);
        buttonSkipAll.SetActive(true); // ✅ Hiện Skip All

        for (int i = 0; i < content.Length; i++)
        {
            if (skipAll) break; // ✅ Nếu bấm Skip All thì thoát vòng lặp

            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "Unknown";

            isTyping = true;
            skipPressed = false;
            isWaitingForNext = false;

            foreach (var letter in content[i])
            {
                if (skipPressed || skipAll)
                {
                    NPCContent.text = content[i];
                    break;
                }

                NPCContent.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            skipPressed = false;
            isWaitingForNext = true;

            while (!skipPressed && !skipAll)
            {
                yield return null;
            }

            isWaitingForNext = false;
        }

        EndDialogue(); // ✅ Kết thúc hội thoại
    }

    private void EndDialogue()
    {
        buttonSkip.SetActive(false);
        buttonSkipAll.SetActive(false);
        NPCPanel.SetActive(false);

        playerController.isController = true;
        comboAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        openShop.enabled = true;
        isOpenShop = true;

        if (player != null)
        {
            player.SetActive(true);
            playerNpc.SetActive(false);
            cam.SetActive(false);
            camcine.Priority = 0;
            camcine.enabled = false;
        }

        // phần thưởng
        playerStatus.IncreasedGold(300);
        StartCoroutine(WaitQuestUI());

        // save
        GameSaveData data = SaveManagerMan.LoadGame();
        data.dataQuest.isQuestShopMap1 = true;
        SaveManagerMan.SaveGame(data);

        skipAll = false; // ✅ Reset
    }

    public void OnSkipButtonPressed()
    {
        audioSource.PlayOneShot(audioSkip);
        if (isTyping || isWaitingForNext)
        {
            skipPressed = true;
        }
    }

    public void OnSkipAllButtonPressed() // ✅ Hàm Skip All
    {
        audioSource.PlayOneShot(audioSkip);
        skipAll = true;
    }

    public void EndContent()
    {
        NPCPanel.SetActive(false);

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator WaitQuestUI()
    {
        niceQuestUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        niceQuestUI.SetActive(false);
    }
}
