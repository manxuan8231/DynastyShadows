using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum QuestStage
{
    NotStarted,
    Quest5InProgress,
    Quest5Completed,
    Quest6InProgress,
    Quest6Completed,
    Quest7Stage1,
    Quest7Stage2,
    Quest7Stage3,
    Quest7Completed


}
public class NPCDialogueController : MonoBehaviour
{
    [Header("-----------------Data-----------------")]
    public DialogueData dialogueDataQuest5; // Dialogue cho quest 5
    public DialogueData dialogueDataQuest7; // Dialogue cho quest 6
    [Header("-----------------Content UI-----------------")]
    public GameObject questionGameCanvas;
    public TMP_Text nameTxt;
    public TMP_Text contentText;
    public GameObject btnF;
    public GameObject PanelContent;
    [Header("-----------------Quest UI-----------------")]
    public GameObject canvasQuest;
    public TMP_Text questContent;

    [Header("-----------------State UI-----------------")]
    public GameObject stateCanvas;
    public TMP_Text stateText;
    public TMP_Text missionName;
    public Image iconState;

    [Header("-----------------Other-----------------")]
    Animator animator;
    Coroutine Coroutine;
    public AudioCanvasState audioCanvasState;
    public QuestStage currentStage;
    [Header("-----------------Bool-----------------")]
    bool isOpen;
    bool isTyping;
    public bool isSkip;
    bool isWriteSkip;
    public bool isContent = true;
    public bool isActiveBtn = false;
    public bool hasFinishedDialogue = false;
    bool hasPlayedTalkingAnim = false;

    [Header("-----------------quest items-----------------")]
    public GameObject destinationQuest;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            HandleQuestProgression();
        }

        if (currentStage == QuestStage.NotStarted) return;

        if (isOpen && !isActiveBtn && isContent)
        {
            questionGameCanvas.SetActive(true);
            btnF.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (!hasPlayedTalkingAnim)
                {
                    animator.SetTrigger("Talk");
                    hasPlayedTalkingAnim = true;
                }

                isActiveBtn = true;
                isContent = false;
                btnF.SetActive(false);
                PanelContent.SetActive(true);

                DialogueData currentDialogue = GetCurrentDialogueData();
                if (currentDialogue != null)
                {
                    Coroutine = StartCoroutine(ReadContent(currentDialogue));
                }
            }
        }
    }
    private DialogueData GetCurrentDialogueData()
    {
        switch (currentStage)
        {
            case QuestStage.Quest5InProgress:
                return dialogueDataQuest5;
            case QuestStage.Quest7Stage1:
                return dialogueDataQuest7;

            default:
                return null;
        }
    }
    public void HandleQuestProgression()
    {
        switch (currentStage)
        {
            case QuestStage.Quest5InProgress:   
                currentStage = QuestStage.Quest5Completed;
                HandleQuestProgression();

                break;
            case QuestStage.Quest5Completed:
                StartCoroutine(Quest6Inpro());
                break;
            case QuestStage.Quest6InProgress:
                Debug.Log("Quest 6 In Progress");
                break;
            case QuestStage.Quest6Completed:
                StartCoroutine(Quest6Done());
                break;
            case QuestStage.Quest7Stage1:
                Debug.Log("Quest 7 Stage1");
                break;
            case QuestStage.Quest7Completed:
                Debug.Log("Quest 7 Completed");
                break;

            default:
                // Không có hành động nào khác
                break;
        }
    }
   IEnumerator Quest6Inpro()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Quest 6 In Progress");
        currentStage = QuestStage.Quest6InProgress;
        
    }
    IEnumerator Quest6Done()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Quest 6 Completed");
        currentStage = QuestStage.Quest7Stage1;
        isContent = true;
    }
  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = true;
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isOpen = false;
            btnF.SetActive(false);
        }
    }

    private IEnumerator ReadContent(DialogueData data)
    {
        var player = FindFirstObjectByType<PlayerControllerState>();
        player.enabled = false;
        player.animator.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        for (int i = 0; i < data.contentTextQuest.Length; i++)
        {
            contentText.text = "";
            nameTxt.text = data.nameTextQuest.Length > i ? data.nameTextQuest[i] : "???";
            isTyping = true;
            isWriteSkip = false;

            foreach (var c in data.contentTextQuest[i])
            {
                if (isSkip)
                {
                    contentText.text = data.contentTextQuest[i];
                    break;
                }

                contentText.text += c;
                yield return new WaitForSeconds(0.05f);
            }

            isTyping = false;
            isSkip = false;
            isWriteSkip = true;

            while (!isSkip) yield return null;

            isWriteSkip = false;
        }

        EndContent();
    }

    public void SkipBtn()
    {
        if (isTyping || isWriteSkip)
        {
            isSkip = true;
        }
    }

    public void EndContent()
    {
        var player = FindFirstObjectByType<PlayerControllerState>();
        player.enabled = true;
        player.animator.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        questionGameCanvas.SetActive(false);
        PanelContent.SetActive(false);
        animator.SetTrigger("Idle");

        if (Coroutine != null)
            StopCoroutine(Coroutine);

        // Reset dialogue state
        isContent = false;
        isActiveBtn = false;
        hasPlayedTalkingAnim = false;

        // Xử lý chuyển đổi quest stage
        DialogueData currentDialogue = GetCurrentDialogueData();
        HandleQuestProgression();
        StartCoroutine(ShowQuestState(currentDialogue));
        StartCoroutine(ShowQuestContent(currentDialogue));
        if (currentStage == QuestStage.Quest7Stage1)
        {
            destinationQuest.SetActive(true);
            currentStage = QuestStage.Quest7Stage2; // Chuyển sang giai đoạn tiếp theo của quest 7
        }
    }   
    IEnumerator ShowQuestState(DialogueData dialogue)
    {
        stateCanvas.SetActive(true);
        stateText.text = dialogue.stateText;
        missionName.text = dialogue.missionName;
        iconState.sprite = dialogue.iconState;
        yield return new WaitForSeconds(2.5f);
        stateCanvas.SetActive(false);
       
    }
    IEnumerator ShowQuestContent(DialogueData dialogue)
    {
        canvasQuest.SetActive(true);
        questContent.text = dialogue.contentQuestGame;
        yield return new WaitForSeconds(2.5f);
    }
}
   
