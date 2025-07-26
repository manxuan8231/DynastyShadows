using System.Collections;
using TMPro;
using UnityEngine;

public class TurnInQuest5 : MonoBehaviour
{
    public GameObject NPCPanel;
    public TextMeshProUGUI NPCName;
    public TextMeshProUGUI NPCContent;
    public GameObject icon3D;
    public GameObject niceQuestUI;
    public GameObject arrowIcon;
    public GameObject questBoss;
    public string[] names;
    public string[] content;

    private Coroutine coroutine;
    public GameObject buttonF;
    public bool isContent = false;
    public bool isButtonF = false;

    public GameObject buttonSkip;
    private bool isTyping = false;
    private bool skipPressed = false;
    private bool isWaitingForNext = false;

    private float lastSkipTime = 0f;
    private float skipCooldown = 0.2f;
    //tham chieu
    PlayerControllerState playerController;
    ComboAttack comboAttack;
    public QuestDesert5 questDesert5;
    PlayerStatus playerStatus;
    NPCScript npcScript;
    public AudioSource audioSource;
    public AudioClip audioSkip;

    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        questDesert5 = FindAnyObjectByType<QuestDesert5>();
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        audioSource = GetComponent<AudioSource>();
        npcScript = FindAnyObjectByType<NPCScript>();
        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);
        buttonF.SetActive(false);
        arrowIcon.SetActive(false);
        niceQuestUI.SetActive(false);
        questBoss.SetActive(false);
        isButtonF = false;
        isContent = false;

        NPCName.text = "";
        NPCContent.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF && coroutine == null)
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
            npcScript.player.SetActive(false); // Ẩn nhân vật người chơi khi hội thoại bắt đầu
            npcScript.cam.SetActive(true); // Hiện camera NPC khi hội thoại bắt đầu
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isContent)
        {
            buttonF.SetActive(true);
            isButtonF = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            buttonF.SetActive(false);
            isButtonF = false;
            NPCPanel.SetActive(false);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            // Reset trạng thái
            isTyping = false;
            skipPressed = false;
            isWaitingForNext = false;
        }
    }

    private IEnumerator ReadContent()
    {
        buttonSkip.SetActive(true);

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
                    NPCContent.text = content[i];
                    break;
                }

                NPCContent.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            skipPressed = false;
            isWaitingForNext = true;

            while (!skipPressed)
            {
                yield return null;
            }

            isWaitingForNext = false;
        }

        // Kết thúc hội thoại
        Debug.Log("Kết thúc hội thoại");
        buttonSkip.SetActive(false);
        NPCPanel.SetActive(false);
        playerController.isController = true;
        comboAttack.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        questDesert5.questNameText.enabled = false;
        questDesert5.questPanel.SetActive(false);
        arrowIcon.SetActive(false);
        icon3D.SetActive(false);
        playerStatus.IncreasedGold(500); playerStatus.AddExp(500);
        StartCoroutine(WaitQuestUI());
        questBoss.SetActive(true);
        coroutine = null;
        npcScript.player.SetActive(true); // Ẩn nhân vật người chơi khi hội thoại bắt đầu
        npcScript.cam.SetActive(false); // Hiện camera NPC khi hội thoại bắt đầu
    }

    public void OnSkipButtonPressed()
    {
        if (Time.time - lastSkipTime < skipCooldown) return;
        lastSkipTime = Time.time;

        audioSource.PlayOneShot(audioSkip);

        if (isTyping || isWaitingForNext)
        {
            skipPressed = true;
        }
    }

    public void EndContent()
    {
        NPCPanel.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        // Reset trạng thái
        isTyping = false;
        skipPressed = false;
        isWaitingForNext = false;
    }

    private IEnumerator WaitQuestUI()
    {
        niceQuestUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        niceQuestUI.SetActive(false);
    }

    public void StartTurnInQuest5()
    {
        arrowIcon.SetActive(true);
        icon3D.SetActive(true);
        isContent = true;
        
    }
}
