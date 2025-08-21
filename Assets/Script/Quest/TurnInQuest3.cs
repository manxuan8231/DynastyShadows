using System.Collections;
using TMPro;
using UnityEngine;

public class TurnInQuest3 : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị hội thoại
    public TextMeshProUGUI NPCName; // Tên của NPC
    public TextMeshProUGUI NPCContent; // Nội dung hội thoại
    public GameObject icon3D; // Icon 3D của NPC
    public GameObject bacLamQuestMain;
    public GameObject questDesert5;
    public GameObject niceQuestUI;

    // trạng thái
    public enum QuestToStart { None, BacLam, LinhCanh }
    public QuestToStart questToStart = QuestToStart.None;

    public string[] names;
    public string[] content;

    private Coroutine coroutine;
    public GameObject buttonF;
    public bool isContent = false;
    public bool isButtonF = false;

    // skip
    public GameObject buttonSkip;
    public GameObject buttonSkipAll; // NEW
    private bool isTyping = false;
    private bool skipPressed = false;
    private bool isWaitingForNext = false;
    private bool skipAll = false; // NEW

    // tham chiếu
    PlayerControllerState playerController;
    ComboAttack comboAttack;
    Quest3 quest3;
    PlayerStatus playerStatus;
    NPCScript npcScript;
    AudioSource audioSource;
    public AudioClip audioSkip;

    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        quest3 = FindAnyObjectByType<Quest3>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        audioSource = GetComponent<AudioSource>();
        npcScript = GetComponent<NPCScript>();

        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonSkipAll.SetActive(false); // Ẩn Skip All lúc đầu
        buttonF.SetActive(false);
        bacLamQuestMain.SetActive(false);
        questDesert5.SetActive(false);
        niceQuestUI.SetActive(false);
        NPCName.text = "";
        NPCContent.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF == true)
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
            if (npcScript != null)
            {
                npcScript.player.SetActive(false);
                npcScript.cam.SetActive(true);
            }
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
        buttonSkipAll.SetActive(true); // Bật Skip All

        for (int i = 0; i < content.Length; i++)
        {
            if (skipAll) break; // Nếu bấm Skip All → thoát luôn

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

        EndDialogue(); // NEW: gom phần kết thúc
    }

    private void EndDialogue()
    {
        buttonSkip.SetActive(false);
        buttonSkipAll.SetActive(false); // Tắt Skip All
        NPCPanel.SetActive(false);
        playerController.isController = true;
        comboAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        switch (questToStart)
        {
            case QuestToStart.BacLam:
                Debug.Log("Phần thưởng đã nhận");
                break;
            case QuestToStart.LinhCanh:
                quest3.questPanel.SetActive(false);
                quest3.iconQuest.SetActive(false);
                quest3.pointerLinhCanh.SetActive(false);
                icon3D.SetActive(false);
                playerStatus.IncreasedGold(500);
                playerStatus.showSkill3 = true;
                bacLamQuestMain.SetActive(true);
                questDesert5.SetActive(true);
                StartCoroutine(WaitQuestUI());
                if (npcScript != null)
                {
                    npcScript.player.SetActive(true);
                    npcScript.cam.SetActive(false);
                }
                Debug.Log("Phần thưởng đã nhận");
                GameSaveData data = SaveManagerMan.LoadGame();
                data.skillTreeData.showSkill3 = playerStatus.showSkill3;
                data.dataQuest.isQuest3Map1 = true;
                SaveManagerMan.SaveGame(data);
                break;
        }
    }

    public void OnSkipButtonPressed()
    {
        audioSource.PlayOneShot(audioSkip);
        if (isTyping)
        {
            skipPressed = true;
        }
        else if (isWaitingForNext)
        {
            skipPressed = true;
        }
    }

    public void OnSkipAllButtonPressed() // NEW
    {
        audioSource.PlayOneShot(audioSkip);
        skipAll = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        EndDialogue();
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
