using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject NPCPanel;
    public TextMeshProUGUI NPCName;
    public TextMeshProUGUI NPCContent;
    public GameObject buttonF;
    public GameObject buttonSkip;
    public GameObject player;
    public GameObject cam;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioSkip;

   
    public enum QuestToStart { None, BacLam, Village, LinhCanh, MainBacLam, LinhCanhB }
    public QuestToStart questToStart = QuestToStart.None;

    [Header("Dialogue")]
    public string[] names;
    public string[] content;

    private Coroutine coroutine;
    private bool isTyping = false;
    private bool skipPressed = false;
    private bool isWaitingForNext = false;
    private bool isContent = true;
    private bool isButtonF = false;
    

    private float lastSkipTime = 0f;
    private float skipCooldown = 0.2f;

    // tham chieu
    private PlayerControllerState playerController;
    private ComboAttack comboAttack;
    private Quest1 quest1;
    private Quest2 quest2;
    private Quest3 quest3;
    private QuestMainBacLam questMainBacLam;
    private Animator animator;

    void Start()
    {
        playerController = FindAnyObjectByType<PlayerControllerState>();
        comboAttack = FindAnyObjectByType<ComboAttack>();
        quest1 = FindAnyObjectByType<Quest1>();
        quest2 = FindAnyObjectByType<Quest2>();
        quest3 = FindAnyObjectByType<Quest3>();
        questMainBacLam = FindAnyObjectByType<QuestMainBacLam>();
       

        audioSource = GetComponent<AudioSource>();

        NPCPanel.SetActive(false);
        buttonF.SetActive(false);
        buttonSkip.SetActive(false);
       
        NPCName.text = "";
        NPCContent.text = "";
        
        player = FindAnyObjectByType<PlayerControllerState>().gameObject;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isButtonF && coroutine == null)
        {
            StartDialogue();
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

            comboAttack.enabled = true;
            playerController.isController = true;
            playerController.animator.SetBool("isWalking", true);
            playerController.animator.SetBool("isRunning", true);

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

    private void StartDialogue()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        comboAttack.enabled = false;
        playerController.isController = false;
        playerController.animator.SetBool("isWalking", false);
        playerController.animator.SetBool("isRunning", false);

        NPCPanel.SetActive(true);
        buttonSkip.SetActive(true);
        buttonF.SetActive(false);
        isButtonF = false;
        if(player != null)
        {
            player.SetActive(false);
            animator.SetBool("Talking", true);
            cam.SetActive (true);
        }
        coroutine = StartCoroutine(ReadContent());
    }

    private IEnumerator ReadContent()
    {
        for (int i = 0; i < content.Length; i++)
        {
           
            
            NPCContent.text = "";
            NPCName.text = names.Length > i ? names[i] : "???";

            isTyping = true;
            skipPressed = false;
            isWaitingForNext = false;

            foreach (char letter in content[i])
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
                yield return null;

            isWaitingForNext = false;
        }
        
        EndDialogueAndStartQuest();
    }

    private void EndDialogueAndStartQuest()
    {
        NPCPanel.SetActive(false);
        buttonSkip.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        comboAttack.enabled = true;
        playerController.isController = true;

        isContent = false;
        isButtonF = false;
        coroutine = null;
        if (player != null)
        {
            player.SetActive(true); 
         
            animator.SetBool("Talking", false);
            cam.SetActive(false);
        }

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
        if (Time.time - lastSkipTime < skipCooldown) return;
        lastSkipTime = Time.time;

       
        HandleSkip();
    }

    private void HandleSkip()
    {
        if (isTyping || isWaitingForNext)
        {
            audioSource?.PlayOneShot(audioSkip);
            skipPressed = true;
        }
    }

   public void IsContent()
    {
        isContent = false;
    }
}
